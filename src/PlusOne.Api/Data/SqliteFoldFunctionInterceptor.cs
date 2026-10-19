using System.Data.Common;
using System.Runtime.CompilerServices;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace PlusOne.Api.Data;

public sealed class SqliteFoldFunctionInterceptor : DbConnectionInterceptor
{
    private static readonly ConditionalWeakTable<SqliteConnection, object> Registered = new();

    public override void ConnectionOpened(DbConnection connection, ConnectionEndEventData eventData)
    {
        Register(connection);
        base.ConnectionOpened(connection, eventData);
    }

    public override Task ConnectionOpenedAsync(
        DbConnection connection,
        ConnectionEndEventData eventData,
        CancellationToken cancellationToken = default)
    {
        Register(connection);
        return base.ConnectionOpenedAsync(connection, eventData, cancellationToken);
    }

    private static void Register(DbConnection connection)
    {
        if (connection is not SqliteConnection sqlite)
        {
            return;
        }

        Registered.GetValue(sqlite, static sqliteConnection =>
        {
            sqliteConnection.CreateFunction(
                "fold_text",
                (string? value) => SqliteText.Fold(value),
                isDeterministic: true);
            return new object();
        });
    }
}
