using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Sheaft.Application.Models;
using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Application.Interfaces.Exporters
{
    public interface IBillingsFileExporter
    {
        Task<Result<ResourceExportDto>> ExportAsync(RequestUser user, IQueryable<Delivery> deliveriesQuery,
            CancellationToken token, DateTimeOffset? from = null, DateTimeOffset? to = null);
    }
}