using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Data.Sqlite;
using Sheaft.Infrastructure;

namespace Sheaft.UnitTests.Fakes;

#pragma warning disable CS8767
#pragma warning disable CS8618

internal class SqliteDbConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public SqliteDbConnectionFactory(string? connectionString = "DataSource=:memory:")
    {
        _connectionString = connectionString;
    }

    public DbConnection CreateConnection(DatabaseConnectionName connectionName)
    {
        return new SqliteConnection(_connectionString);
    }
}

[SuppressMessage("ReSharper", "UnassignedGetOnlyAutoProperty")]
public class FakeDbConnection : IDbConnection
{
    public void Dispose()
    {
    }

    public IDbTransaction BeginTransaction()
    {
        return new FakeDbTransaction(this);
    }

    public IDbTransaction BeginTransaction(IsolationLevel il)
    {
        return new FakeDbTransaction(this, il);
    }

    public void ChangeDatabase(string databaseName)
    {
    }

    public void Close()
    {
    }

    public IDbCommand CreateCommand()
    {
        return new FakeDbCommand();
    }

    public void Open()
    {
    }

    public string ConnectionString { get; set; }
    public int ConnectionTimeout { get; }
    public string Database { get; }
    public ConnectionState State { get; }
}

public class FakeDbTransaction : IDbTransaction
{
    public FakeDbTransaction(IDbConnection fakeDbConnection, IsolationLevel il = IsolationLevel.ReadCommitted)
    {
        Connection = fakeDbConnection;
        IsolationLevel = il;
    }

    public void Dispose()
    {
    }

    public void Commit()
    {
    }

    public void Rollback()
    {
    }

    public IDbConnection? Connection { get; }
    public IsolationLevel IsolationLevel { get; }
}

public class FakeDbCommand : IDbCommand
{
    public void Dispose()
    {
        
    }

    public void Cancel()
    {
    }

    public IDbDataParameter CreateParameter()
    {
        return null;
    }

    public int ExecuteNonQuery()
    {
        return 1;
    }

    public IDataReader ExecuteReader()
    {
        return null;
    }

    public IDataReader ExecuteReader(CommandBehavior behavior)
    {
        return null;
    }

    public object? ExecuteScalar()
    {
        return null;
    }

    public void Prepare()
    {
    }

    public string CommandText { get; set; }
    public int CommandTimeout { get; set; }
    public CommandType CommandType { get; set; }
    public IDbConnection? Connection { get; set; }
    public IDataParameterCollection Parameters { get; }
    public IDbTransaction? Transaction { get; set; }
    public UpdateRowSource UpdatedRowSource { get; set; }
}

#pragma warning restore CS8767
#pragma warning restore CS8618