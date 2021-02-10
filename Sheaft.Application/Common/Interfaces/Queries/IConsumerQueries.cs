using System;
using System.Linq;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Domain;

namespace Sheaft.Application.Common.Interfaces.Queries
{
    public interface IConsumerQueries
    {
        IQueryable<ConsumerDto> GetConsumer(Guid id, RequestUser currentUser);
    }
}