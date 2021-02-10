using System;
using System.Linq;
using AutoMapper.QueryableExtensions;
using Sheaft.Application.Common.Extensions;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Queries;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Domain;

namespace Sheaft.Application.Consumer.Queries
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
                    .Get(c => c.Id == id)
                    .ProjectTo<ConsumerDto>(_configurationProvider);
        }
    }
}