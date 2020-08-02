using Sheaft.Models.Dto;
using Sheaft.Interop;
using System;
using System.Linq;

namespace Sheaft.Application.Queries
{
    public interface IDepartmentQueries
    {
        IQueryable<DepartmentDto> GetDepartment(Guid id, IRequestUser currentUser);
        IQueryable<DepartmentDto> GetDepartments(IRequestUser currentUser);
        IQueryable<DepartmentDto> GetRegionDepartments(Guid regionId, IRequestUser currentUser);
    }
}