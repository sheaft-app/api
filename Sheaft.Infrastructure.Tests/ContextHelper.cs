using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Moq;
using Sheaft.Application.Interop;
using Sheaft.Localization;
using System;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.Tests.Common
{
    public static class ContextHelper
    {
        public static IAppDbContext GetInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString("N"))
                .Options;

            return new AppDbContext(options, Mock.Of<IStringLocalizer<MessageResources>>());
        }
    }
}
