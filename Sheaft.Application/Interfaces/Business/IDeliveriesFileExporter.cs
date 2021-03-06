using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Models;
using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Application.Interfaces.Business
{
    public interface IDeliveriesFileExporter
    {
        Task<Result<ResourceExportDto>> ExportAsync(RequestUser user, DateTimeOffset @from, DateTimeOffset to,
            IQueryable<Delivery> deliveriesQuery, CancellationToken token);
    }
}