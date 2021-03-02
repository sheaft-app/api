﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Moq;
using Sheaft.Localization;
using System;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
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

            return new AppDbContext(options, Mock.Of<IStringLocalizer<MessageResources>>(), Mock.Of<ISheaftMediatr>());
        }
    }
}