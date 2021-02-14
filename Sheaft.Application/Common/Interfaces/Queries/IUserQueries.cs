using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Domain;

namespace Sheaft.Application.Common.Interfaces.Queries
{
    public interface IUserQueries
    {
        Task<string> GetFreshdeskTokenAsync(RequestUser currentUser, CancellationToken token);
        IQueryable<UserDto> GetUser(Guid id, RequestUser currentUser);
        IQueryable<UserProfileDto> GetUserProfile(Guid id, RequestUser currentUser);
        IQueryable<ProfileInformationDto> GetUserProfileInformation(Guid id, RequestUser currentUser);
        Task<SirenBusinessDto> RetrieveSiretInfosAsync(string input, RequestUser currentUser, CancellationToken token);
        IQueryable<BusinessProfileDto> GetMyProfile(RequestUser currentUser);
    }
}