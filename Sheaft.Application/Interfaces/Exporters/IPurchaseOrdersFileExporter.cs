using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Sheaft.Application.Models;
using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Application.Interfaces.Exporters
{
    public interface IPurchaseOrdersFileExporter
    {
        Task<Result<ResourceExportDto>> ExportAsync(RequestUser user, DateTimeOffset from, DateTimeOffset to, IQueryable<PurchaseOrder> purchaseOrdersQuery, CancellationToken token);
    }
}