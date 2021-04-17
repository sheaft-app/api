using System;
using System.Linq;
using AutoMapper.QueryableExtensions;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Queries;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.Mediatr.Consumer.Queries
{
    public class ConsumerQueries : IConsumerQueries
    {
        private readonly IAppDbContext _context;
        private readonly AutoMapper.IConfigurationProvider _configurationProvider;

        public ConsumerQueries(IAppDbContext context, AutoMapper.IConfigurationProvider configurationProvider)
        {
            _configurationProvider = configurationProvider;
            _context = context;
        }

        public IQueryable<ConsumerDto> GetConsumer(Guid id, RequestUser currentUser)
        {
            return _context.Users.OfType<Domain.Consumer>()
                    .Where(c => c.Id == id)
                    .ProjectTo<ConsumerDto>(_configurationProvider);
        }
    }
}