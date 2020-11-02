using Sheaft.Core;
using Sheaft.Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Sheaft.Application.Interop
{
    public interface IPictureService
    {
        Task<Result<string>> HandleUserPictureAsync(User user, string picture, CancellationToken token);
        Task<Result<string>> HandleProductPictureAsync(Product product, string picture, CancellationToken token);
        Task<Result<string>> HandleTagPictureAsync(Tag tag, string picture, CancellationToken token);
        Task<Result<string>> HandleTagIconAsync(Tag tag, string icon, CancellationToken token);
    }
}