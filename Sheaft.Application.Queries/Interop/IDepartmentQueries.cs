using Sheaft.Models.Dto;
using Sheaft.Core;
using System;
using System.Linq;

namespace Sheaft.Application.Queries
{
    public interface IDepartmentQueries
    {
        IQueryable<DepartmentDto> GetDepartment(Guid id, RequestUser currentUser);
        IQueryable<DepartmentDto> GetDepartments(RequestUser currentUser);
        IQueryable<DepartmentDto> GetRegionDepartments(Guid regionId, RequestUser currentUser);
    }
}