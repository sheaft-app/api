using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Application.Interfaces.Services
{
    public interface IPictureService
    {
        Task<Result<string>> HandleUserPictureAsync(Domain.User user, Guid pictureId, string picture, CancellationToken token);
        Task<Result<string>> HandleUserProfileAsync(Domain.User user, string picture, CancellationToken token);
        Task<Result<string>> HandleProductPictureAsync(Domain.Product product, Guid pictureId, string picture, CancellationToken token);
        Task<Result<string>> HandleTagPictureAsync(Domain.Tag tag, string picture, CancellationToken token);
        Task<Result<string>> HandleTagIconAsync(Domain.Tag tag, string icon, CancellationToken token);
        string GetDefaultProductPicture(IEnumerable<Tag> tags);
    }
}