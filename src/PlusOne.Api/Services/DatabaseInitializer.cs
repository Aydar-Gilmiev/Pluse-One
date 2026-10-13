using Microsoft.EntityFrameworkCore;
using PlusOne.Api.Data;
using PlusOne.Api.Data.Entities;

namespace PlusOne.Api.Services;

public static class DatabaseInitializer
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        var contextDb = serviceProvider.GetRequiredService<PlusOneDbContext>();

        await contextDb.Database.EnsureCreatedAsync();
        await RenameLegacyPasswordColumnAsync(contextDb);

        await contextDb.Database.ExecuteSqlRawAsync("""
            CREATE TABLE IF NOT EXISTS Roles (
                Id INTEGER NOT NULL CONSTRAINT PK_Roles PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL
            );
            """);

        await contextDb.Database.ExecuteSqlRawAsync("""
            CREATE TABLE IF NOT EXISTS Users (
                Id INTEGER NOT NULL CONSTRAINT PK_Users PRIMARY KEY AUTOINCREMENT,
                UserName TEXT NOT NULL,
                Login TEXT NOT NULL,
                Password TEXT NOT NULL,
                RoleId INTEGER NOT NULL,
                CONSTRAINT FK_Users_Roles_RoleId FOREIGN KEY (RoleId) REFERENCES Roles (Id) ON DELETE RESTRICT
            );
            """);

        await contextDb.Database.ExecuteSqlRawAsync(
            "CREATE UNIQUE INDEX IF NOT EXISTS IX_Users_Login ON Users (Login);");

        await GetOrCreateRoleAsync(contextDb, "User");
        var adminRole = await GetOrCreateRoleAsync(contextDb, "Admin");

        var admin = await contextDb.Users.FirstOrDefaultAsync(item => item.Login == "admin");
        if (admin is null)
        {
            contextDb.Users.Add(new User
            {
                UserName = "Администратор",
                Login = "admin",
                Password = "admin123",
                RoleId = adminRole.Id
            });
            await contextDb.SaveChangesAsync();
        }
        else if (admin.Password != "admin123")
        {
            admin.Password = "admin123";
            await contextDb.SaveChangesAsync();
        }
    }

    private static async Task<Role> GetOrCreateRoleAsync(PlusOneDbContext contextDb, string roleName)
    {
        var role = await contextDb.Roles.FirstOrDefaultAsync(item => item.Name == roleName);
        if (role is not null)
        {
            return role;
        }

        role = new Role { Name = roleName };
        contextDb.Roles.Add(role);
        await contextDb.SaveChangesAsync();
        return role;
    }

    private static async Task RenameLegacyPasswordColumnAsync(PlusOneDbContext contextDb)
    {
        var connection = contextDb.Database.GetDbConnection();
        await connection.OpenAsync();

        try
        {
            await using var command = connection.CreateCommand();
            command.CommandText = "PRAGMA table_info(Users);";

            var hasLegacyColumn = false;
            var hasPasswordColumn = false;

            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var columnName = reader.GetString(1);
                hasLegacyColumn |= columnName == "PasswordHash";
                hasPasswordColumn |= columnName == "Password";
            }

            if (!hasLegacyColumn || hasPasswordColumn)
            {
                return;
            }

            await reader.CloseAsync();
            command.CommandText = "ALTER TABLE Users RENAME COLUMN PasswordHash TO Password;";
            await command.ExecuteNonQueryAsync();
        }
        finally
        {
            await connection.CloseAsync();
        }
    }
}
