using System;
using Microsoft.Extensions.Logging;
using Sheaft.Domain;
using EventId = Microsoft.Extensions.Logging.EventId;

namespace Sheaft.IntegrationTests.Fakes;

public class FakeLogger<T> : ILogger<T> 
{
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }

    public IDisposable BeginScope<TState>(TState state)
    {
        throw new NotImplementedException();
    }
}

public class FakeLogger<T, TU> : ILogger<IRepository<T, TU>> 
    where T : class, IAggregateRoot
{
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }

    public IDisposable BeginScope<TState>(TState state)
    {
        throw new NotImplementedException();
    }
}