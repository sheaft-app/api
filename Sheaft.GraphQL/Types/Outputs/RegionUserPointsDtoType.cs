using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Models;
using Sheaft.Domain;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class RegionUserPointsDtoType : SheaftOutputType<RegionUserPointsDto>
    {
        protected override void Configure(IObjectTypeDescriptor<RegionUserPointsDto> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("RegionUserPoints");
            
            descriptor
                .Field(c => c.Points)
                .Name("points");

            descriptor
                .Field(c => c.Position)
                .Name("position");

            descriptor
                .Field("user")
                .UseDbContext<QueryDbContext>()
                .ResolveWith<RegionUserResolvers>(c => c.GetUser(default, default, default));

            descriptor
                .Field("department")
                .UseDbContext<QueryDbContext>()
                .ResolveWith<RegionUserResolvers>(c => c.GetRegion(default, default, default));
        }

        private class RegionUserResolvers
        {
            public Task<User> GetUser(RegionUserPointsDto regionUser, [ScopedService] QueryDbContext context,
                CancellationToken token)
            {
                return context.Users.SingleOrDefaultAsync(u => u.Id == regionUser.UserId, token);
            }
            
            public Task<Region> GetRegion(RegionUserPointsDto regionUser, [ScopedService] QueryDbContext context,
                CancellationToken token)
            {
                return context.Regions.SingleOrDefaultAsync(u => u.Id == regionUser.RegionId, token);
            }
        }
    }
}
