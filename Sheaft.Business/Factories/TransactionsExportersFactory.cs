using System;
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
using Sheaft.Options;

namespace Sheaft.Business.Factories
{
    public class TransactionsExportersFactory : ITransactionsExportersFactory
    {
        private readonly IAppDbContext _context;
        private readonly ExportersOptions _options;

        public TransactionsExportersFactory(IAppDbContext context, IOptions<ExportersOptions> options)
        {
            _context = context;
            _options = options.Value;
        }

        public ITransactionsFileExporter GetExporter(RequestUser requestUser, string typename)
        {
            return InstanciateExporter(requestUser, typename);
        }
        
        public async Task<ITransactionsFileExporter> GetExporterAsync(RequestUser requestUser, CancellationToken token)
        {
            var user = await _context.Users.SingleAsync(e => e.Id == requestUser.Id, token);
            var setting = user.GetSetting(SettingKind.PickingOrdersExporter);
            
            return InstanciateExporter(requestUser, setting?.Value ?? _options.TransactionsExporter);
        }

        private static ITransactionsFileExporter InstanciateExporter(RequestUser requestUser, string typename)
        {
            var type = Type.GetType(typename);
            if (type == null)
                throw new ArgumentException($"Invalid typename configured for user: {requestUser.Id} transactions exporter.");

            if (!(Activator.CreateInstance(type) is ITransactionsFileExporter exporter))
                throw new ArgumentException($"Invalid type used {type.FullName} for transactions exporter.");

            return exporter;
        }
    }
}