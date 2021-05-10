using System;
using System.Linq;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using Microsoft.AspNetCore.Http;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Domain;
using Sheaft.GraphQL.Types.Outputs;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.GraphQL.Departments
{
    [ExtendObjectType(Name = "Query")]
    public class DepartmentQueries : SheaftQuery
    {
        public DepartmentQueries(
            ICurrentUserService currentUserService,
            IHttpContextAccessor httpContextAccessor)
            :base(currentUserService, httpContextAccessor)
        {
        }

        [GraphQLName("department")]
        [GraphQLType(typeof(DepartmentType))]
        [UseDbContext(typeof(QueryDbContext))]
        [UseSingleOrDefault]
        public IQueryable<Department> Get([ID] Guid id, [ScopedService] QueryDbContext context)
        {
            SetLogTransaction(id);
            return context.Departments
                .Where(d => d.Id == id);
        }
        
        [GraphQLName("departments")]
        [GraphQLType(typeof(ListType<DepartmentType>))]
        [UseDbContext(typeof(QueryDbContext))]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Department> GetAll([ScopedService] QueryDbContext context)
        {
            SetLogTransaction();
            return context.Departments;
        }
    }
}