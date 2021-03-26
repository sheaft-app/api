﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Sheaft.Application.Interfaces.Business;
using Sheaft.Application.Interfaces.Factories;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Options;

namespace Sheaft.Business
{
    public class PickingOrdersExportersFactory : IPickingOrdersExportersFactory
    {
        private readonly IAppDbContext _context;
        private readonly Func<string, IPickingOrdersFileExporter> _resolver;
        private readonly ExportersOptions _options;

        public PickingOrdersExportersFactory(IAppDbContext context, IOptions<ExportersOptions> options, Func<string, IPickingOrdersFileExporter> resolver)
        {
            _context = context;
            _resolver = resolver;
            _options = options.Value;
        }

        public IPickingOrdersFileExporter GetExporter(RequestUser requestUser, string typename)
        {
            return InstanciateExporter(typename);
        }
        
        public async Task<IPickingOrdersFileExporter> GetExporterAsync(RequestUser requestUser, CancellationToken token)
        {
            var user = await _context.GetByIdAsync<User>(requestUser.Id, token);
            var setting = user.GetSetting(SettingKind.PickingOrdersExporter);
            
            return InstanciateExporter(setting?.Value ?? _options.PickingOrdersExporter);
        }

        private IPickingOrdersFileExporter InstanciateExporter(string typename)
        {
            return _resolver.Invoke(typename);
        }
    }
}