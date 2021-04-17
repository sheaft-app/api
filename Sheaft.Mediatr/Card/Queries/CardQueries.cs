using System;
using System.Linq;
using AutoMapper.QueryableExtensions;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Queries;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.Mediatr.Card.Queries
{
    public class CardQueries : ICardQueries
    {
        private readonly IAppDbContext _context;
        private readonly AutoMapper.IConfigurationProvider _configurationProvider;

        public CardQueries(IAppDbContext context, AutoMapper.IConfigurationProvider configurationProvider)
        {
            _context = context;
            _configurationProvider = configurationProvider;
        }

        public IQueryable<CardDto> GetCard(Guid id, RequestUser currentUser)
        {
            return _context.Cards
                .Get(c => c.Id == id && c.User.Id == currentUser.Id)
                .ProjectTo<CardDto>(_configurationProvider);
        }

        public IQueryable<CardDto> GetCards(RequestUser currentUser)
        {
            return _context.Cards
                .Get(c => c.User.Id == currentUser.Id)
                .ProjectTo<CardDto>(_configurationProvider);
        }
    }
}