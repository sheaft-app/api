using System;
using System.Linq;
using AutoMapper.QueryableExtensions;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Queries;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.Services.Department.Queries
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