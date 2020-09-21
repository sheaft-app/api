using System;
using System.Linq;
using Sheaft.Core;
using Sheaft.Domain.Models;
using AutoMapper.QueryableExtensions;
using Sheaft.Application.Models;
using Sheaft.Application.Interop;

namespace Sheaft.Application.Queries
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
            return _context.Users.OfType<Consumer>()
                    .Get(c => c.Id == id)
                    .ProjectTo<ConsumerDto>(_configurationProvider);
        }
    }
}