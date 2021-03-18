using System;
using System.Linq;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.Application.Interfaces.Queries
{
    public interface IDepartmentQueries
    {
        IQueryable<DepartmentDto> GetDepartment(Guid id, RequestUser currentUser);
        IQueryable<DepartmentDto> GetDepartments(RequestUser currentUser);
        IQueryable<DepartmentDto> GetRegionDepartments(Guid regionId, RequestUser currentUser);
    }
}