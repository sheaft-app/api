using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Moq;
using Sheaft.Application.Interop;
using Sheaft.Infrastructure.Persistence;
using Sheaft.Localization;
using System;

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
