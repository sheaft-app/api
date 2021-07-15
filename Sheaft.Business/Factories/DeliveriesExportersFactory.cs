﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces.Business;
using Sheaft.Application.Interfaces.Factories;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Infrastructure.Persistence;
using Sheaft.Options;

namespace Sheaft.Business.Factories
{
    public class DeliveriesExportersFactory : IDeliveriesExportersFactory
    {
        private readonly IAppDbContext _context;
        private readonly Func<string, IDeliveriesFileExporter> _resolver;
        private readonly ExportersOptions _options;

        public DeliveriesExportersFactory(IAppDbContext context, IOptions<ExportersOptions> options, Func<string, IDeliveriesFileExporter> resolver)
        {
            _context = context;
            _resolver = resolver;
            _options = options.Value;
        }

        public IDeliveriesFileExporter GetExporter(RequestUser requestUser, string typename)
        {
            return InstanciateExporter(requestUser, typename);
        }
        
        public async Task<IDeliveriesFileExporter> GetExporterAsync(RequestUser requestUser, CancellationToken token)
        {
            var user = await _context.Users.SingleAsync(e => e.Id == requestUser.Id, token);
            var setting = user.GetSetting(SettingKind.DeliveriesExporter);
            
            return InstanciateExporter(requestUser, setting?.Value ?? _options.DeliveriesExporter);
        }

        private IDeliveriesFileExporter InstanciateExporter(RequestUser requestUser, string typename)
        {
            return _resolver.Invoke(typename);
        }
    }
}