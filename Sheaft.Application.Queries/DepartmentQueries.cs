using System;
using System.Collections.Generic;
using System.Linq;
using Sheaft.Infrastructure.Interop;
using Sheaft.Models.Dto;
using Sheaft.Interop;
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

        public IQueryable<DepartmentDto> GetDepartment(Guid id, IRequestUser currentUser)
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

        public IQueryable<DepartmentDto> GetDepartments(IRequestUser currentUser)
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

        public IQueryable<DepartmentDto> GetRegionDepartments(Guid regionId, IRequestUser currentUser)
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

        private static IQueryable<DepartmentDto> GetAsDto(IQueryable<Department> query)
        {
            return query
                .Select(c => new DepartmentDto
                {
                    Id = c.Id,
                    Code = c.Code,
                    Name = c.Name,
                    Level = new LevelDto
                    {
                        Id = c.Level.Id,
                        UpdatedOn = c.Level.UpdatedOn,
                        CreatedOn = c.Level.CreatedOn,
                        Number = c.Level.Number,
                        Name = c.Level.Name,
                        RequiredPoints = c.Level.RequiredPoints,
                        Rewards = c.Level.Rewards.Select(r =>
                            new RewardDto
                            {
                                Id = r.Id,
                                Contact = r.Contact,
                                CreatedOn = r.CreatedOn,
                                Description = r.Description,
                                Email = r.Email,
                                Image = r.Image,
                                Name = r.Name,
                                Phone = r.Phone,
                                UpdatedOn = r.UpdatedOn,
                                Url = r.Url
                            })
                    }
                });
        }
    }
}