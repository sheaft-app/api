using System;
using System.Collections.Generic;
using System.Linq;
using Sheaft.Application.Models;
using Sheaft.Core;
using Sheaft.Core.Extensions;
using AutoMapper.QueryableExtensions;
using Microsoft.Extensions.Options;
using Sheaft.Options;
using Sheaft.Application.Interop;

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
            if (currentUser.IsInRole(_roleOptions.Store.Value))
            {
                return _context.Agreements
                        .Get(c => c.Id == id && c.Store.Id == currentUser.Id && !c.Delivery.RemovedOn.HasValue)
                        .ProjectTo<AgreementDto>(_configurationProvider);
            }

            if (currentUser.IsInRole(_roleOptions.Producer.Value))
            {
                return _context.Agreements
                        .Get(c => c.Id == id && c.Delivery.Producer.Id == currentUser.Id && !c.Delivery.RemovedOn.HasValue)
                        .ProjectTo<AgreementDto>(_configurationProvider);
            }

            return new List<AgreementDto>().AsQueryable();
        }

        public IQueryable<AgreementDto> GetAgreements(RequestUser currentUser)
        {
            if (currentUser.IsInRole(_roleOptions.Store.Value))
            {
                return _context.Agreements
                        .Get(c => c.Store.Id == currentUser.Id && !c.Delivery.RemovedOn.HasValue)
                        .ProjectTo<AgreementDto>(_configurationProvider);
            }

            if (currentUser.IsInRole(_roleOptions.Producer.Value))
            {
                return _context.Agreements
                        .Get(c => c.Delivery.Producer.Id == currentUser.Id && !c.Delivery.RemovedOn.HasValue)
                        .ProjectTo<AgreementDto>(_configurationProvider);
            }

            return new List<AgreementDto>().AsQueryable();
        }

        public IQueryable<AgreementDto> GetStoreAgreements(Guid storeId, RequestUser currentUser)
        {
            if (currentUser.IsInRole(_roleOptions.Producer.Value))
            {
                return _context.Agreements
                        .Get(c => c.Store.Id == storeId && c.Delivery.Producer.Id == currentUser.Id && !c.Delivery.RemovedOn.HasValue)
                        .ProjectTo<AgreementDto>(_configurationProvider);
            }

            return new List<AgreementDto>().AsQueryable();
        }

        public IQueryable<AgreementDto> GetProducerAgreements(Guid producerId, RequestUser currentUser)
        {
            if (currentUser.IsInRole(_roleOptions.Store.Value))
            {
                return _context.Agreements
                        .Get(c => c.Store.Id == currentUser.Id && c.Delivery.Producer.Id == producerId && !c.Delivery.RemovedOn.HasValue)
                        .ProjectTo<AgreementDto>(_configurationProvider);
            }

            return new List<AgreementDto>().AsQueryable();
        }
    }
}