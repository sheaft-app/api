using System.Threading;
using System.Threading.Tasks;
using Sheaft.Application.Interfaces.Exporters;
using Sheaft.Domain;

namespace Sheaft.Application.Interfaces.Factories
{
    public interface IPickingOrdersExportersFactory
    {
        IPickingOrdersFileExporter GetExporter(RequestUser requestUser, string typename);
        Task<IPickingOrdersFileExporter> GetExporterAsync(RequestUser requestUser, CancellationToken token);
    }
}