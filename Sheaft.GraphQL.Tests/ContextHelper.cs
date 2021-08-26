using System;
using Microsoft.EntityFrameworkCore;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.GraphQL.Tests
{
    public static class ContextHelper
    {
        public static QueryDbContext GetInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<QueryDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString("N"))
                .Options;

            return new QueryDbContext(options);
        }
    }
}
