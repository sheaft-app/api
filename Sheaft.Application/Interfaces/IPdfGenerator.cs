using System.Threading;
using System.Threading.Tasks;
using Sheaft.Domain.Common;

namespace Sheaft.Application.Interfaces
{
    public interface IPdfGenerator
    {
        Task<Result<byte[]>> GeneratePdfAsync<T>(string filename, string templateId, T data, CancellationToken token);
    }
}