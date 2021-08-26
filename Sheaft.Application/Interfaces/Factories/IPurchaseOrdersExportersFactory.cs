using System.Threading;
using System.Threading.Tasks;
using Sheaft.Application.Interfaces.Exporters;
using Sheaft.Domain;

namespace Sheaft.Application.Interfaces.Factories
{
    public interface IPurchaseOrdersExportersFactory
    {
        IPurchaseOrdersFileExporter GetExporter(RequestUser requestUser, string typename);
        Task<IPurchaseOrdersFileExporter> GetExporterAsync(RequestUser requestUser, CancellationToken token);
    }
}