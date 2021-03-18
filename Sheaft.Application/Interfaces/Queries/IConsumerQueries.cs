using System;
using System.Linq;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.Application.Interfaces.Queries
{
    public interface IConsumerQueries
    {
        IQueryable<ConsumerDto> GetConsumer(Guid id, RequestUser currentUser);
    }
}