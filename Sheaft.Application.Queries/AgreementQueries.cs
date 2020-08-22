using System;
using System.Collections.Generic;
using System.Linq;
using Sheaft.Infrastructure.Interop;
using Sheaft.Models.Dto;
using Sheaft.Core;
using Sheaft.Core.Extensions;
using Sheaft.Domain.Models;
using AutoMapper.QueryableExtensions;
using Sheaft.Infrastructure;
using Microsoft.Extensions.Options;
using Sheaft.Options;

namespace Sheaft.Application.Queries
{
    public class AgreementQueries : IAgreementQueries
    {
        private readonly IAppDbContext _context;
        private readonly AutoMapper.IConfigurationProvider _configurationProvider;
        private readonly RoleOptions _roleOptions;

        public AgreementQueries(IAppDbContext context, AutoMapper.IConfigurationProvider configurationProvider, IOptionsSnapshot<RoleOptions> roleOptions)
        {
            _roleOptions = roleOptions.Value;
            _context = context;
            _configurationProvider = configurationProvider;
        }

        public IQueryable<AgreementDto> GetAgreement(Guid id, RequestUser currentUser)
        {
            try
            {
                if (currentUser.IsInRole(_roleOptions.Store.Value))
                    return _context.Agreements
                            .Get(c => c.Id == id && c.Store.Id == currentUser.CompanyId && !c.Delivery.RemovedOn.HasValue)
                            .ProjectTo<AgreementDto>(_configurationProvider);

                if (currentUser.IsInRole(_roleOptions.Producer.Value))
                    return _context.Agreements
                            .Get(c => c.Id == id && c.Delivery.Producer.Id == currentUser.CompanyId && !c.Delivery.RemovedOn.HasValue)
                            .ProjectTo<AgreementDto>(_configurationProvider);

                return new List<AgreementDto>().AsQueryable();
            }
            catch (Exception e)
            {
                return new List<AgreementDto>().AsQueryable();
            }
        }

        public IQueryable<AgreementDto> GetAgreements(RequestUser currentUser)
        {
            try
            {
                if (currentUser.IsInRole(_roleOptions.Store.Value))
                    return _context.Agreements
                            .Get(c => c.Store.Id == currentUser.CompanyId && !c.Delivery.RemovedOn.HasValue)
                            .ProjectTo<AgreementDto>(_configurationProvider);

                if (currentUser.IsInRole(_roleOptions.Producer.Value))
                    return _context.Agreements
                            .Get(c => c.Delivery.Producer.Id == currentUser.CompanyId && !c.Delivery.RemovedOn.HasValue)
                            .ProjectTo<AgreementDto>(_configurationProvider);

                return new List<AgreementDto>().AsQueryable();
            }
            catch (Exception e)
            {
                return new List<AgreementDto>().AsQueryable();
            }
        }

        public IQueryable<AgreementDto> GetStoreAgreements(Guid storeId, RequestUser currentUser)
        {
            try
            {
                if (currentUser.IsInRole(_roleOptions.Producer.Value))
                    return _context.Agreements
                            .Get(c => c.Store.Id == storeId && c.Delivery.Producer.Id == currentUser.CompanyId && !c.Delivery.RemovedOn.HasValue)
                            .ProjectTo<AgreementDto>(_configurationProvider);

                return new List<AgreementDto>().AsQueryable();
            }
            catch (Exception e)
            {
                return new List<AgreementDto>().AsQueryable();
            }
        }

        public IQueryable<AgreementDto> GetProducerAgreements(Guid producerId, RequestUser currentUser)
        {
            try
            {
                if (currentUser.IsInRole(_roleOptions.Store.Value))
                    return _context.Agreements
                            .Get(c => c.Store.Id == currentUser.CompanyId && c.Delivery.Producer.Id == producerId && !c.Delivery.RemovedOn.HasValue)
                            .ProjectTo<AgreementDto>(_configurationProvider);

                return new List<AgreementDto>().AsQueryable();
            }
            catch (Exception e)
            {
                return new List<AgreementDto>().AsQueryable();
            }
        }

        private static IQueryable<AgreementDto> GetAsDto(IQueryable<Agreement> query)
        {
            return query
                .Select(a => new AgreementDto
                {
                    Id = a.Id,
                    CreatedOn = a.CreatedOn,
                    Delivery = new AgreementDeliveryModeDto
                    {
                        Address = a.Delivery.Address != null ? new AddressDto
                        {
                            City = a.Delivery.Address.City,
                            Latitude = a.Delivery.Address.Latitude,
                            Line1 = a.Delivery.Address.Line1,
                            Line2 = a.Delivery.Address.Line2,
                            Longitude = a.Delivery.Address.Longitude,
                            Zipcode = a.Delivery.Address.Zipcode
                        } : null,
                        Producer = new CompanyProfileDto
                        {
                            Id = a.Delivery.Producer.Id,
                            Email = a.Delivery.Producer.Email,
                            Name = a.Delivery.Producer.Name,
                            Phone = a.Delivery.Producer.Phone,
                            Picture = a.Delivery.Producer.Picture,
                            Address = new AddressDto
                            {
                                City = a.Delivery.Producer.Address.City,
                                Latitude = a.Delivery.Producer.Address.Latitude,
                                Line1 = a.Delivery.Producer.Address.Line1,
                                Line2 = a.Delivery.Producer.Address.Line2,
                                Longitude = a.Delivery.Producer.Address.Longitude,
                                Zipcode = a.Delivery.Producer.Address.Zipcode
                            },
                        },
                        CreatedOn = a.Delivery.CreatedOn,
                        Description = a.Delivery.Description,
                        Id = a.Delivery.Id,
                        Kind = a.Delivery.Kind,
                        LockOrderHoursBeforeDelivery = a.Delivery.LockOrderHoursBeforeDelivery,
                        Name = a.Delivery.Name,
                        UpdatedOn = a.Delivery.UpdatedOn
                    },
                    Reason = a.Reason,
                    RemovedOn = a.RemovedOn,
                    SelectedHours = a.SelectedHours.Select(sh => new TimeSlotDto
                    {
                        Day = sh.Day,
                        From = sh.From,
                        To = sh.To
                    }),
                    Status = a.Status,
                    Store = new CompanyProfileDto
                    {
                        Id = a.Store.Id,
                        Email = a.Store.Email,
                        Name = a.Store.Name,
                        Phone = a.Store.Phone,
                        Picture = a.Store.Picture,
                        Address = new AddressDto
                        {
                            City = a.Delivery.Producer.Address.City,
                            Latitude = a.Delivery.Producer.Address.Latitude,
                            Line1 = a.Delivery.Producer.Address.Line1,
                            Line2 = a.Delivery.Producer.Address.Line2,
                            Longitude = a.Delivery.Producer.Address.Longitude,
                            Zipcode = a.Delivery.Producer.Address.Zipcode
                        }
                    },
                    UpdatedOn = a.UpdatedOn
                });
        }
    }
}