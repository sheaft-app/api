using Sheaft.Domain.StoreProcedures;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Sheaft.Infrastructure.Interop
{
    public interface IDapperContext
    {
        Task<UserPosition> GetUserPositionAsync(Guid userId);
        Task<UserPosition> GetUserPositionInDepartmentAsync(Guid userId, Guid departmentId);
        Task<UserPosition> GetUserPositionInRegionAsync(Guid userId, Guid regionId);
        Task<bool> SetNotificationAsReadAsync(Guid userId, Guid? groupId, DateTimeOffset readBefore, CancellationToken token);
    }
}