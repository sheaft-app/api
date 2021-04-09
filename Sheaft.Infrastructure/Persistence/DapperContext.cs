using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Threading;
using Dapper;
using Microsoft.Extensions.Options;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Domain.Views;
using Sheaft.Options;

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
                var result = await connection.QueryAsync<UserPosition>(
                    "exec [app].[UserPositionInDepartment] @DepartmentId, @UserId",
                    new {UserId = userId, DepartmentId = departmentId});
                return result.SingleOrDefault();
            }
        }

        public async Task<UserPosition> GetUserPositionInRegionAsync(Guid userId, Guid regionId)
        {
            using (var connection = new SqlConnection(_databaseOptions.ConnectionString))
            {
                var result = await connection.QueryAsync<UserPosition>(
                    "exec [app].[UserPositionInRegion] @RegionId, @UserId", new {UserId = userId, RegionId = regionId});
                return result.SingleOrDefault();
            }
        }

        public async Task<UserPosition> GetUserPositionAsync(Guid userId)
        {
            using (var connection = new SqlConnection(_databaseOptions.ConnectionString))
            {
                var result = await connection.QueryAsync<UserPosition>("exec [app].[UserPositionInCountry] @UserId",
                    new {UserId = userId});
                return result.SingleOrDefault();
            }
        }

        public async Task<bool> SetNotificationAsReadAsync(Guid userId, DateTimeOffset readBefore,
            CancellationToken token)
        {
            using (var connection = new SqlConnection(_databaseOptions.ConnectionString))
            {
                await connection.ExecuteAsync(
                    "exec [app].[MarkUserNotificationsAsRead] @UserUid, @ReadBefore",
                    new {UserUid = userId, ReadBefore = readBefore});

                return true;
            }
        }
    }
}