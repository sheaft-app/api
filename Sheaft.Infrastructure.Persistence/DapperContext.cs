using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Dapper;
using Sheaft.Domain.StoreProcedures;
using System.Linq;
using System.Threading;
using Sheaft.Options;
using Microsoft.Extensions.Options;
using Sheaft.Application.Interop;

namespace Sheaft.Infrastructure.Persistence
{
    public class DapperContext : IDapperContext
    {
        private readonly AppDatabaseOptions _databaseOptions;

        public DapperContext(IOptionsSnapshot<AppDatabaseOptions> databaseOptions)
        {
            _databaseOptions = databaseOptions.Value;
        }

        public async Task<UserPosition> GetUserPositionInDepartmentAsync(Guid userId, Guid departmentId)
        {
            using (var connection = new SqlConnection(_databaseOptions.ConnectionString))
            {
                var result = await connection.QueryAsync<UserPosition>("exec [dbo].[UserPositionInDepartment] @DepartmentId, @UserId", new { UserId = userId, DepartmentId = departmentId });
                return result.SingleOrDefault();
            }
        }

        public async Task<UserPosition> GetUserPositionInRegionAsync(Guid userId, Guid regionId)
        {
            using (var connection = new SqlConnection(_databaseOptions.ConnectionString))
            {
                var result = await connection.QueryAsync<UserPosition>("exec [dbo].[UserPositionInRegion] @RegionId, @UserId", new { UserId = userId, RegionId = regionId });
                return result.SingleOrDefault();
            }
        }

        public async Task<UserPosition> GetUserPositionAsync(Guid userId)
        {
            using (var connection = new SqlConnection(_databaseOptions.ConnectionString))
            {
                var result = await connection.QueryAsync<UserPosition>("exec [dbo].[UserPositionInCountry] @UserId", new { UserId = userId });
                return result.SingleOrDefault();
            }
        }

        public async Task<bool> SetNotificationAsReadAsync(Guid userId, DateTimeOffset readBefore, CancellationToken token)
        {
            using (var connection = new SqlConnection(_databaseOptions.ConnectionString))
            {
                var result = await connection.ExecuteAsync("exec [dbo].[MarkUserNotificationsAsRead] @UserUid, @ReadBefore", new { UserUid = userId, ReadBefore = readBefore });
                return result > 0;
            }
        }
    }
}