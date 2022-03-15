using System.Data.Common;
using Microsoft.Data.SqlClient;

namespace Sheaft.Infrastructure;

public interface IDbConnectionFactory
{
    DbConnection CreateConnection(DatabaseConnectionName connectionName);
}

internal class SqlDbConnectionFactory : IDbConnectionFactory
{
    private readonly IDictionary<DatabaseConnectionName, string> _connectionStrings;

    public SqlDbConnectionFactory(IDictionary<DatabaseConnectionName, string> connectionStrings)
    {
        _connectionStrings = connectionStrings;
    }

    public DbConnection CreateConnection(DatabaseConnectionName connectionName)
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