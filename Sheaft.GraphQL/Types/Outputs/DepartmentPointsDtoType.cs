using System.Threading;
using System.Threading.Tasks;
using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.Domain;
using Sheaft.GraphQL.Catalogs;
using Sheaft.GraphQL.Departments;
using Sheaft.GraphQL.Regions;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class DepartmentPointsDtoType : SheaftOutputType<DepartmentPointsDto>
    {
        protected override void Configure(IObjectTypeDescriptor<DepartmentPointsDto> descriptor)
        {
            base.Configure(descriptor);
            
            descriptor
                .Field(c => c.Points)
                .Name("points");
                
            descriptor
                .Field(c => c.Position)
                .Name("position");
            
            descriptor
                .Field("department")
                .ResolveWith<DepartmentsPointsResolvers>(c => c.GetDepartment(default, default, default))
                .Type<NonNullType<RegionType>>();
            
            descriptor
                .Field("region")
                .ResolveWith<DepartmentsPointsResolvers>(c => c.GetRegion(default, default, default))
                .Type<NonNullType<RegionType>>();
                
            descriptor
                .Field(c => c.Users)
                .Name("usersCount");
        }

        private class DepartmentsPointsResolvers
        {
            public Task<Department> GetDepartment(DepartmentPointsDto regionPoints, DepartmentsByIdBatchDataLoader departmentsDataLoader,
                CancellationToken token)
            {
                return departmentsDataLoader.LoadAsync(regionPoints.DepartmentId, token);
            }
            
            public Task<Region> GetRegion(DepartmentPointsDto regionPoints, RegionsByIdBatchDataLoader regionsDataLoader,
                CancellationToken token)
            {
                return regionsDataLoader.LoadAsync(regionPoints.RegionId, token);
            }
        }
    }
}
