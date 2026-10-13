namespace PlusOne.Api.Data.Entities;

public sealed class User
{
    public int Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Login { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public int RoleId { get; set; }
    public Role Role { get; set; } = null!;
}
