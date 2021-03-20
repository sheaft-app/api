using System.Threading;
using System.Threading.Tasks;
using Sheaft.Application.Interfaces.Business;
using Sheaft.Domain;

namespace Sheaft.Business
{
    public interface IPickingOrdersExportersFactory
    {
        IPickingOrdersFileExporter GetExporter(RequestUser requestUser, string typename);
        Task<IPickingOrdersFileExporter> GetExporterAsync(RequestUser requestUser, CancellationToken token);
    }
}