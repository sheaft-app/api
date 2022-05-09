using System;
using System.Data;
using System.Data.Common;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.UnitTests.Fakes;

internal class FakeDbContextFactory : IDisposable
{
    private DbConnection? _connection;

    private DbContextOptions<AppDbContext> CreateOptions()
    {
        return new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(_connection).Options;
    }

    public AppDbContext CreateContext(DbConnection? connection = null)
    {
        if (_connection != null) 
            return new AppDbContext(CreateOptions());
        
        _connection = connection ?? new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        var options = CreateOptions();
        using (var context = new AppDbContext(options))
        {
            context.Database.EnsureCreated();
        }

        return new AppDbContext(CreateOptions());
    }

    public void Dispose()
    {
        _connection.Dispose();
        _connection = null;
    }
}