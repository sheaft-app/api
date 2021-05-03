using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;
using Microsoft.AspNetCore.Http;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Security;
using Sheaft.Domain;
using Sheaft.GraphQL.Types.Outputs;
using Sheaft.Mediatr.ProfileInformation.Commands;
using Sheaft.Mediatr.User.Commands;

namespace Sheaft.GraphQL.Users
{
    [ExtendObjectType(Name = "Mutation")]
    public class UserMutations : SheaftMutation
    {
        public UserMutations(
            ISheaftMediatr mediator,
            ICurrentUserService currentUserService,
            IHttpContextAccessor httpContextAccessor)
            : base(mediator, currentUserService, httpContextAccessor)
        {
        }

        [GraphQLName("generateSponsoringCode")]
        [GraphQLType(typeof(StringType))]
        [Authorize(Policy = Policies.REGISTERED)]
        public async Task<string> GeneratedSponsoringCode([GraphQLName("input")] GenerateUserCodeCommand input, CancellationToken token)
        {
            return await ExecuteAsync<GenerateUserCodeCommand, string>(input, token);
        }
        
        [GraphQLName("updateUserPicture")]
        [Authorize(Policy = Policies.REGISTERED)]
        [GraphQLType(typeof(UserType))]
        public async Task<User> UpdateUserPictureAsync([GraphQLName("input")] UpdateUserPreviewCommand input,
            UsersByIdBatchDataLoader usersDataLoader, CancellationToken token)
        {
            await ExecuteAsync<UpdateUserPreviewCommand, string>(input, token);
            return await usersDataLoader.LoadAsync(input.UserId, token);
        }
        
        [GraphQLName("addPictureToUser")]
        [Authorize(Policy = Policies.REGISTERED)]
        [GraphQLType(typeof(BooleanType))]
        public async Task<bool> AddPictureToUserAsync([GraphQLName("input")] AddPictureToUserCommand input, CancellationToken token)
        {
            return await ExecuteAsync(input, token);
        }
        
        [GraphQLName("removeUserPictures")]
        [Authorize(Policy = Policies.REGISTERED)]
        [GraphQLType(typeof(BooleanType))]
        public async Task<bool> RemoveUserPicturesAsync([GraphQLName("input")] RemoveUserPicturesCommand input, CancellationToken token)
        {
            return await ExecuteAsync(input, token);
        }
        
        [GraphQLName("closeAccount")]
        [Authorize(Policy = Policies.REGISTERED)]
        [GraphQLType(typeof(BooleanType))]
        public async Task<bool> RemoveUserAsync([GraphQLName("input")] RemoveUserCommand input, CancellationToken token)
        {
            return await ExecuteAsync(input, token);
        }
    }
}