using System;
using System.Collections.Generic;
using System.Linq;
using Sheaft.Infrastructure.Interop;
using Sheaft.Models.Dto;
using Sheaft.Core;
using Sheaft.Domain.Models;
using AutoMapper.QueryableExtensions;
using Sheaft.Infrastructure;

namespace Sheaft.Application.Queries
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
            try
            {
                return _context.Departments
                        .Get(d => d.Id == id)
                        .ProjectTo<DepartmentDto>(_configurationProvider);
            }
            catch (Exception e)
            {
                return new List<DepartmentDto>().AsQueryable();
            }
        }

        public IQueryable<DepartmentDto> GetDepartments(RequestUser currentUser)
        {
            try
            {
                return _context.Departments
                        .Get()
                        .ProjectTo<DepartmentDto>(_configurationProvider);
            }
            catch (Exception e)
            {
                return new List<DepartmentDto>().AsQueryable();
            }
        }

        public IQueryable<DepartmentDto> GetRegionDepartments(Guid regionId, RequestUser currentUser)
        {
            try
            {
                return _context.Departments
                        .Get(d => d.Region.Id == regionId)
                        .ProjectTo<DepartmentDto>(_configurationProvider);
            }
            catch (Exception e)
            {
                return new List<DepartmentDto>().AsQueryable();
            }
        }
    }
}