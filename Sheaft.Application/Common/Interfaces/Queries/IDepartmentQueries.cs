using System;
using System.Linq;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Domain;

namespace Sheaft.Application.Common.Interfaces.Queries
{
    public interface IDepartmentQueries
    {
        IQueryable<DepartmentDto> GetDepartment(Guid id, RequestUser currentUser);
        IQueryable<DepartmentDto> GetDepartments(RequestUser currentUser);
        IQueryable<DepartmentDto> GetRegionDepartments(Guid regionId, RequestUser currentUser);
    }
}