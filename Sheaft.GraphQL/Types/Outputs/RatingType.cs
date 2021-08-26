using System.Threading;
using System.Threading.Tasks;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.GraphQL.Ratings;
using Sheaft.GraphQL.Users;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class RatingType : SheaftOutputType<Rating>
    {
        protected override void Configure(IObjectTypeDescriptor<Rating> descriptor)
        {
            base.Configure(descriptor);

            descriptor
                .ImplementsNode()
                .IdField(c => c.Id)
                .ResolveNode(
                    (ctx, id) => ctx.DataLoader<RatingsByIdBatchDataLoader>().LoadAsync(id, ctx.RequestAborted));
            
            descriptor
                .Field(c => c.CreatedOn)
                .Name("createdOn");
                
            descriptor
                .Field(c => c.Value)
                .Name("value");
                
            descriptor
                .Field(c => c.UpdatedOn)
                .Name("updatedOn");
                
            descriptor
                .Field(c => c.Comment)
                .Name("comment");

            descriptor
                .Field(c => c.User)
                .Name("user")
                .ResolveWith<RatingResolvers>(c => c.GetUser(default, default, default))
                .Type<NonNullType<UserType>>();
        }

        private class RatingResolvers
        {
            public Task<User> GetUser(Rating rating, UsersByIdBatchDataLoader usersDataLoader, CancellationToken token)
            {
                return usersDataLoader.LoadAsync(rating.UserId, token);
            }
        }
    }
}
