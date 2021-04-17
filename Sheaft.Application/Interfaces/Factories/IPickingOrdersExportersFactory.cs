using System.Threading;
using System.Threading.Tasks;
using Sheaft.Application.Interfaces.Business;
using Sheaft.Domain;

namespace Sheaft.Application.Interfaces.Factories
{
    public interface IPickingOrdersExportersFactory
    {
        IPickingOrdersFileExporter GetExporter(RequestUser requestUser, string typename);
        Task<IPickingOrdersFileExporter> GetExporterAsync(RequestUser requestUser, CancellationToken token);
    }
}