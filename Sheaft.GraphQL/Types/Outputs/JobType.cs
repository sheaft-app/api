using System.Threading;
using System.Threading.Tasks;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.GraphQL.Jobs;
using Sheaft.GraphQL.Users;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class JobType : SheaftOutputType<Job>
    {
        protected override void Configure(IObjectTypeDescriptor<Job> descriptor)
        {
            base.Configure(descriptor);

            descriptor
                .ImplementsNode()
                .IdField(c => c.Id)
                .ResolveNode((ctx, id) => 
                    ctx.DataLoader<JobsByIdBatchDataLoader>().LoadAsync(id, ctx.RequestAborted));
            
            descriptor
                .Field(c => c.Status)
                .Name("status");
                
            descriptor
                .Field(c => c.Kind)
                .Name("kind");
                
            descriptor
                .Field(c => c.CreatedOn)
                .Name("createdOn");
                
            descriptor
                .Field(c => c.Archived)
                .Name("archived");
                
            descriptor
                .Field(c => c.Retried)
                .Name("retried");
                
            descriptor
                .Field(c => c.UpdatedOn)
                .Name("updatedOn");
                
            descriptor
                .Field(c => c.StartedOn)
                .Name("startedOn");
                
            descriptor
                .Field(c => c.CompletedOn)
                .Name("completedOn");
                
            descriptor
                .Field(c => c.Message)
                .Name("message");
                
            descriptor
                .Field(c => c.File)
                .Name("file");

            descriptor
                .Field(c => c.Name)
                .Name("name")
                .Type<NonNullType<StringType>>();

            descriptor
                .Field(c => c.User)
                .Name("user")
                .ResolveWith<JobResolvers>(c => c.GetUser(default, default, default))
                .Type<NonNullType<UserType>>();
        }

        private class JobResolvers
        {
            public Task<User> GetUser(Job job, UsersByIdBatchDataLoader usersDataLoader, CancellationToken token)
            {
                return usersDataLoader.LoadAsync(job.UserId, token);
            }
        }
    }
}
