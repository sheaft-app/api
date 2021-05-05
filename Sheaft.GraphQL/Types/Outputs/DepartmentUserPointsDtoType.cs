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
    public class DepartmentUserPointsDtoType : SheaftOutputType<DepartmentUserPointsDto>
    {
        protected override void Configure(IObjectTypeDescriptor<DepartmentUserPointsDto> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("DepartmentUserPoints");

            descriptor
                .Field(c => c.Points)
                .Name("points");

            descriptor
                .Field(c => c.Position)
                .Name("position");

            descriptor
                .Field("user")
                .UseDbContext<AppDbContext>()
                .ResolveWith<DepartmentUserResolvers>(c => c.GetUser(default, default, default));

            descriptor
                .Field("department")
                .UseDbContext<AppDbContext>()
                .ResolveWith<DepartmentUserResolvers>(c => c.GetDepartment(default, default, default));
        }

        private class DepartmentUserResolvers
        {
            public Task<User> GetUser(DepartmentUserPointsDto departmentUser, [ScopedService] AppDbContext context,
                CancellationToken token)
            {
                return context.Users.SingleOrDefaultAsync(u => u.Id == departmentUser.UserId, token);
            }
            
            public Task<Department> GetDepartment(DepartmentUserPointsDto departmentUser, [ScopedService] AppDbContext context,
                CancellationToken token)
            {
                return context.Departments.SingleOrDefaultAsync(u => u.Id == departmentUser.DepartmentId, token);
            }
        }
    }
}