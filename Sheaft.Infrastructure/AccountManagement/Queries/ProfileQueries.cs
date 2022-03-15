using Dapper;
using Sheaft.Application.AccountManagement;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.AccountManagement;

internal class ProfileQueries : IProfileQueries
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public ProfileQueries(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<Result<ProfileDto>> GetProfile(ProfileId userIdentifier, CancellationToken token)
    {
        await using var connection = _dbConnectionFactory.CreateConnection(DatabaseConnectionName.AppDatabase);

        try
        {
            await connection.OpenAsync(token);
            
            var result = await connection.QuerySingleOrDefaultAsync<ProfileDto>(@"
               SELECT 
                      p.Identifier as Id
                    , p.User_Firstname as Firstname
                    , p.User_Lastname as Lastname
                    , a.Email
                    , p.CreatedOn
                    , p.UpdatedOn
                FROM [dbo].[Profiles] p
                    JOIN [dbo].[Accounts] a ON p.AccountId = a.Id
                WHERE p.Identifier = @UserIdentifier
                GROUP BY 
                         p.Identifier
                       , p.User_Firstname
                       , p.User_Lastname
                       , a.Email
                       , p.CreatedOn
                       , p.UpdatedOn",
                new {UserIdentifier = userIdentifier.Value});

            token.ThrowIfCancellationRequested();

            return result != null
                ? Result.Success(result)
                : Result.Failure<ProfileDto>(ErrorKind.NotFound, "profile.not.found");
        }
        catch (Exception e)
        {
            return Result.Failure<ProfileDto>(ErrorKind.Unexpected, "database.error", e.Message);
        }
        finally
        {
            await connection.CloseAsync();
        }
    }
}