using System;
using System.Linq;
using AutoMapper.QueryableExtensions;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Queries;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Domain;

namespace Sheaft.Application.Department.Queries
{
    public class DepartmentQueries : IDepartmentQueries
    {
        private readonly IAppDbContext _context;
        private readonly AutoMapper.IConfigurationProvider _configurationProvider;

        public DepartmentQueries(IAppDbContext context, AutoMapper.IConfigurationProvider configurationProvider)
        {
            _context = context;
            _configurationProvider = configurationProvider;
        }

        public IQueryable<DepartmentDto> GetDepartment(Guid id, RequestUser currentUser)
        {
            return _context.Departments
                    .Where(d => d.Id == id)
                    .ProjectTo<DepartmentDto>(_configurationProvider);
        }

        public IQueryable<DepartmentDto> GetDepartments(RequestUser currentUser)
        {
            return _context.Departments
                    .ProjectTo<DepartmentDto>(_configurationProvider);
        }

        public IQueryable<DepartmentDto> GetRegionDepartments(Guid regionId, RequestUser currentUser)
        {
            return _context.Departments
                    .Where(d => d.Region.Id == regionId)
                    .ProjectTo<DepartmentDto>(_configurationProvider);
        }
    }
}