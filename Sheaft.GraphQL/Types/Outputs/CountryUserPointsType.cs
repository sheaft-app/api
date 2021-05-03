using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Models;
using Sheaft.Domain;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class CountryUserPointsType : SheaftOutputType<CountryUserPointsDto>
    {
        protected override void Configure(IObjectTypeDescriptor<CountryUserPointsDto> descriptor)
        {
            base.Configure(descriptor);
            
            descriptor.Field(c => c.Points);
            descriptor.Field(c => c.Position);

            descriptor
                .Field("user")
                .UseDbContext<AppDbContext>()
                .ResolveWith<CountryUserResolvers>(c => c.GetUser(default, default, default));
        }

        private class CountryUserResolvers
        {
            public Task<User> GetUser(CountryUserPointsDto countryUser, [ScopedService] AppDbContext context,
                CancellationToken token)
            {
                return context.Users.SingleOrDefaultAsync(u => u.Id == countryUser.UserId, token);
            }
        }
    }
}
