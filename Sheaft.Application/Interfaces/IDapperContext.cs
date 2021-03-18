using System;
using System.Threading;
using System.Threading.Tasks;
using Sheaft.Domain.Views;

namespace Sheaft.Application.Interfaces
{
    public interface IDapperContext
    {
        Task<UserPosition> GetUserPositionAsync(Guid userId);
        Task<UserPosition> GetUserPositionInDepartmentAsync(Guid userId, Guid departmentId);
        Task<UserPosition> GetUserPositionInRegionAsync(Guid userId, Guid regionId);
        Task<bool> SetNotificationAsReadAsync(Guid userId, DateTimeOffset readBefore, CancellationToken token);
    }
}