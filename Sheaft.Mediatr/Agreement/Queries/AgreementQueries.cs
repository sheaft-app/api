using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper.QueryableExtensions;
using Microsoft.Extensions.Options;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Queries;
using Sheaft.Application.Models;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Options;

namespace Sheaft.Mediatr.Agreement.Queries
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
                        .Where(c => c.Id == id && c.StoreId == currentUser.Id)
                        .ProjectTo<AgreementDto>(_configurationProvider);
            }

            if (currentUser.IsInRole(_roleOptions.Producer.Value))
            {
                return _context.Agreements
                        .Where(c => c.Id == id && c.ProducerId == currentUser.Id)
                        .ProjectTo<AgreementDto>(_configurationProvider);
            }

            return new List<AgreementDto>().AsQueryable();
        }

        public IQueryable<AgreementDto> GetAgreements(RequestUser currentUser)
        {
            if (currentUser.IsInRole(_roleOptions.Store.Value))
            {
                return _context.Agreements
                        .Where(c => c.StoreId == currentUser.Id && c.Status != AgreementStatus.Cancelled && c.Status != AgreementStatus.Refused)
                        .ProjectTo<AgreementDto>(_configurationProvider);
            }

            if (currentUser.IsInRole(_roleOptions.Producer.Value))
            {
                return _context.Agreements
                        .Where(c => c.ProducerId == currentUser.Id && c.Status != AgreementStatus.Cancelled && c.Status != AgreementStatus.Refused)
                        .ProjectTo<AgreementDto>(_configurationProvider);
            }

            return new List<AgreementDto>().AsQueryable();
        }

        public IQueryable<AgreementDto> GetStoreAgreements(Guid storeId, RequestUser currentUser)
        {
            if (currentUser.IsInRole(_roleOptions.Producer.Value))
            {
                return _context.Agreements
                        .Where(c => c.StoreId == storeId && c.ProducerId == currentUser.Id && c.Status != AgreementStatus.Cancelled && c.Status != AgreementStatus.Refused)
                        .ProjectTo<AgreementDto>(_configurationProvider);
            }

            return new List<AgreementDto>().AsQueryable();
        }

        public IQueryable<AgreementDto> GetProducerAgreements(Guid producerId, RequestUser currentUser)
        {
            if (currentUser.IsInRole(_roleOptions.Store.Value))
            {
                return _context.Agreements
                        .Where(c => c.StoreId == currentUser.Id && c.ProducerId == producerId && c.Status != AgreementStatus.Cancelled && c.Status != AgreementStatus.Refused)
                        .ProjectTo<AgreementDto>(_configurationProvider);
            }

            return new List<AgreementDto>().AsQueryable();
        }
    }
}