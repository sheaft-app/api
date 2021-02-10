using System.Threading;
using System.Threading.Tasks;
using Sheaft.Application.Common.Models;

namespace Sheaft.Application.Common.Interfaces.Services
{
    public interface IPictureService
    {
        Task<Result<string>> HandleUserPictureAsync(Domain.User user, string picture, string originalPicture, CancellationToken token);
        Task<Result<string>> HandleProductPictureAsync(Domain.Product product, string picture, string originalPicture, CancellationToken token);
        Task<Result<string>> HandleTagPictureAsync(Domain.Tag tag, string picture, CancellationToken token);
        Task<Result<string>> HandleTagIconAsync(Domain.Tag tag, string icon, CancellationToken token);
    }
}