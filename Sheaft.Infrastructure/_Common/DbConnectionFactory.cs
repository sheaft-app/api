using System.Data;
using Microsoft.Data.SqlClient;

namespace Sheaft.Infrastructure;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection(DatabaseConnectionName connectionName);
}

internal class DbConnectionFactory : IDbConnectionFactory
{
    private readonly IDictionary<DatabaseConnectionName, string> _connectionStrings;

    public DbConnectionFactory(IDictionary<DatabaseConnectionName, string> connectionStrings)
    {
        _connectionStrings = connectionStrings;
    }

    public IDbConnection CreateConnection(DatabaseConnectionName connectionName)
    {
        if (_connectionStrings.TryGetValue(connectionName, out var connectionString))
            return new SqlConnection(connectionString);

        throw new ArgumentNullException();
    }
}

public enum DatabaseConnectionName
{
    AppDatabase
}