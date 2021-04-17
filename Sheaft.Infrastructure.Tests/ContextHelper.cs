using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Moq;
using Sheaft.Localization;
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
        public static IAppDbContext GetInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString("N"))
                .Options;

            return new AppDbContext(options, Mock.Of<IStringLocalizer<MessageResources>>(), Mock.Of<ISheaftMediatr>(), Mock.Of<IConfiguration>());
        }
    }
}
