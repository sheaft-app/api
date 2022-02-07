using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Sheaft.Domain.Common
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection(string connectionName = null);
    }
    
    public interface IRepository<T>
        where T: class, IEntity
    {
        Task<Result<T>> Get(Guid identifier, CancellationToken token);
        Task<Result> Save(T entity, CancellationToken token);
    }
}