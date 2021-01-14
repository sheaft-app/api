using System;
using System.Linq;
using AutoMapper.QueryableExtensions;
using Sheaft.Application.Interop;
using Sheaft.Application.Models;
using Sheaft.Core;

namespace Sheaft.Application.Queries
{
    public class CardRegistrationQueries : ICardRegistrationQueries
    {
        private readonly IAppDbContext _context;
        private readonly AutoMapper.IConfigurationProvider _configurationProvider;

        public CardRegistrationQueries(IAppDbContext context, AutoMapper.IConfigurationProvider configurationProvider)
        {
            _context = context;
            _configurationProvider = configurationProvider;
        }

        public IQueryable<CardRegistrationDto> GetCardRegistration(Guid id, RequestUser currentUser)
        {
            return _context.CardRegistrations
                    .Get(c => c.Id == id && c.User.Id == currentUser.Id)
                    .ProjectTo<CardRegistrationDto>(_configurationProvider);
        }

        public IQueryable<CardRegistrationDto> GetCardRegistrations(RequestUser currentUser)
        {
            return _context.CardRegistrations
                    .Get(c => c.User.Id == currentUser.Id)
                    .ProjectTo<CardRegistrationDto>(_configurationProvider);
        }
    }
}