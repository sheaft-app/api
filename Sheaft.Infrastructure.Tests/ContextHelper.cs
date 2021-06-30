using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using Microsoft.Extensions.Configuration;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.Tests.Common
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
