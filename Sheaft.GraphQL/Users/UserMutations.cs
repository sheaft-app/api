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
using Sheaft.GraphQL.Types.Inputs;
using Sheaft.GraphQL.Types.Outputs;
using Sheaft.Mediatr.User.Commands;

namespace Sheaft.GraphQL.Users
{
    [ExtendObjectType(Name = "Mutation")]
    public class UserMutations : SheaftMutation
    {
        public UserMutations(
            ICurrentUserService currentUserService,
            IHttpContextAccessor httpContextAccessor)
            : base(currentUserService, httpContextAccessor)
        {
        }

        [GraphQLName("generateSponsoringCode")]
        [GraphQLType(typeof(StringType))]
        [Authorize(Policy = Policies.REGISTERED)]
        public async Task<string> GeneratedSponsoringCode([Service] ISheaftMediatr mediatr, CancellationToken token)
        {
            return await ExecuteAsync<GenerateUserCodeCommand, string>(mediatr, new GenerateUserCodeCommand(CurrentUser), token);
        }

        [GraphQLName("updateUserPicture")]
        [Authorize(Policy = Policies.REGISTERED)]
        [GraphQLType(typeof(UserType))]
        public async Task<User> UpdateUserPictureAsync(
            [GraphQLType(typeof(UpdateUserPictureInputType))] [GraphQLName("input")]
            UpdateUserPreviewCommand input, [Service] ISheaftMediatr mediatr,
            UsersByIdBatchDataLoader usersDataLoader, CancellationToken token)
        {
            await ExecuteAsync<UpdateUserPreviewCommand, string>(mediatr, input, token);
            return await usersDataLoader.LoadAsync(input.UserId, token);
        }

        [GraphQLName("addPictureToUser")]
        [Authorize(Policy = Policies.REGISTERED)]
        [GraphQLType(typeof(BooleanType))]
        public async Task<bool> AddPictureToUserAsync(
            [GraphQLType(typeof(AddPictureToUserInputType))] [GraphQLName("input")]
            AddPictureToUserCommand input, [Service] ISheaftMediatr mediatr,
            CancellationToken token)
        {
            return await ExecuteAsync(mediatr, input, token);
        }

        [GraphQLName("removeUserPictures")]
        [Authorize(Policy = Policies.REGISTERED)]
        [GraphQLType(typeof(BooleanType))]
        public async Task<bool> RemoveUserPicturesAsync(
            [GraphQLType(typeof(RemoveUserPicturesInputType))] [GraphQLName("input")]
            RemoveUserPicturesCommand input, [Service] ISheaftMediatr mediatr,
            CancellationToken token)
        {
            return await ExecuteAsync(mediatr, input, token);
        }

        [GraphQLName("closeAccount")]
        [Authorize(Policy = Policies.REGISTERED)]
        [GraphQLType(typeof(BooleanType))]
        public async Task<bool> RemoveUserAsync([Service] ISheaftMediatr mediatr, CancellationToken token)
        {
            return await ExecuteAsync(mediatr, new DeleteUserCommand(CurrentUser), token);
        }
    }
}