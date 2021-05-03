using System.Threading;
using System.Threading.Tasks;
using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.Domain;
using Sheaft.GraphQL.Catalogs;
using Sheaft.GraphQL.Regions;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class RegionPointsDtoType : SheaftOutputType<RegionPointsDto>
    {
        protected override void Configure(IObjectTypeDescriptor<RegionPointsDto> descriptor)
        {
            base.Configure(descriptor);
            
            descriptor
                .Field(c => c.Points)
                .Name("points");
                
            descriptor
                .Field(c => c.Position)
                .Name("position");
            
            descriptor
                .Field("region")
                .ResolveWith<RegionPointsResolvers>(c => c.GetRegion(default, default, default))
                .Type<NonNullType<RegionType>>();
                
            descriptor
                .Field(c => c.Users)
                .Name("usersCount");
        }

        private class RegionPointsResolvers
        {
            public Task<Region> GetRegion(RegionPointsDto regionPoints, RegionsByIdBatchDataLoader regionsDataLoader,
                CancellationToken token)
            {
                return regionsDataLoader.LoadAsync(regionPoints.RegionId, token);
            }
        }
    }
}
