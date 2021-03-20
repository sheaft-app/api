using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Sheaft.Application.Models;
using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Application.Interfaces.Business
{
    public interface IPickingOrdersFileExporter
    {
        Task<Result<ResourceExportDto>> ExportAsync(RequestUser user, IQueryable<PurchaseOrder> purchaseOrdersQuery, CancellationToken token);
    }
}