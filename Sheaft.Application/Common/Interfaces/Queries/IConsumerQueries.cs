using Sheaft.Application.Models;
using Sheaft.Core;
using System;
using System.Linq;

namespace Sheaft.Application.Queries
{
    public interface IConsumerQueries
    {
        IQueryable<ConsumerDto> GetConsumer(Guid id, RequestUser currentUser);
    }
}