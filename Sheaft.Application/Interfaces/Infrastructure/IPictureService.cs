using System;
using System.Threading;
using System.Threading.Tasks;
using Sheaft.Core;

namespace Sheaft.Application.Interfaces.Infrastructure
{
    public interface IPictureService
    {
        Task<Result<string>> HandleUserPictureAsync(Domain.User user, string pictureResized, string originalPicture, CancellationToken token);
        Task<Result<string>> HandleProductPreviewAsync(Domain.Product entity, string pictureResized, string originalPicture, CancellationToken token);
        Task<Result<string>> HandleProfilePictureAsync(Domain.User user, Guid pictureId, string picture, CancellationToken token);
        Task<Result<string>> HandleProductPictureAsync(Domain.Product product, Guid pictureId, string picture, CancellationToken token);
        Task<Result<string>> HandleTagPictureAsync(Domain.Tag tag, string picture, CancellationToken token);
        Task<Result<string>> HandleTagIconAsync(Domain.Tag tag, string icon, CancellationToken token);
    }
}