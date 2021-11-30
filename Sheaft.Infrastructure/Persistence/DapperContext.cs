using System;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Sheaft.Application.Configurations;
using Sheaft.Application.Persistence;

namespace Sheaft.Infrastructure.Persistence
{
    internal class DapperContext : IDapperContext
    {
        private readonly AppDatabaseConfiguration _databaseConfiguration;

        public DapperContext(IOptionsSnapshot<AppDatabaseConfiguration> databaseOptions)
        {
            _databaseConfiguration = databaseOptions.Value;
        }

        public async Task<bool> SetNotificationAsReadAsync(Guid userId, DateTimeOffset readBefore,
            CancellationToken token)
        {
            using (var connection = new SqlConnection(_databaseConfiguration.ConnectionString))
            {
                await connection.ExecuteAsync(
                    "exec [app].[MarkUserNotificationsAsRead] @UserUid, @ReadBefore",
                    new {UserUid = userId, ReadBefore = readBefore});

                return true;
            }
        }
    }
}