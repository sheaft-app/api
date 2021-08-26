using System.Threading;
using System.Threading.Tasks;
using Sheaft.Application.Interfaces.Importers;
using Sheaft.Domain;

namespace Sheaft.Application.Interfaces.Factories
{
    public interface IProductsImporterFactory
    {
        Task<IProductsFileImporter> GetImporterAsync(RequestUser requestUser, CancellationToken token);
        IProductsFileImporter GetImporter(RequestUser requestUser, string typename);
    }
}