using Dapper;
using Sheaft.Application.ProfileManagement;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.ProfileManagement;

internal class ProfileQueries : IProfileQueries
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public ProfileQueries(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<Result<ProfileDto>> GetProfile(ProfileId userIdentifier, CancellationToken token)
    {
        using var connection = _dbConnectionFactory.CreateConnection(DatabaseConnectionName.AppDatabase);
        connection.Open();

        try
        {
            token.ThrowIfCancellationRequested();

            var profiles = new Dictionary<string, ProfileDto>();
            await connection.QueryAsync<dynamic, dynamic, ProfileDto>(@"
               SELECT 
                      p.Identifier as UserIdentifier
                    , p.Firstname
                    , p.Lastname
                    , a.Email
                    , p.CreatedOn
                    , p.UpdatedOn
                    , g.Identifier as GroupIdentifier
                    , g.Name
                FROM [dbo].[Profiles] p
                    JOIN [dbo].[Accounts] a ON p.AccountId = a.Id
                    LEFT JOIN [dbo].[GroupMembers] gu ON p.Identifier = gu.ProfileIdentifier
                    LEFT JOIN [dbo].[Groups] g ON g.Id = gu.GroupId
                WHERE p.Identifier = @UserIdentifier
                GROUP BY 
                         p.Identifier
                       , p.Firstname
                       , p.Lastname
                       , a.Email
                       , p.CreatedOn
                       , p.UpdatedOn
                       , g.Identifier
                       , g.Name",
                (user, group) =>
                {
                    token.ThrowIfCancellationRequested();

                    if (!profiles.TryGetValue(user.UserIdentifier, out ProfileDto dto))
                        profiles.Add(user.UserIdentifier,
                            dto = new ProfileDto(user.UserIdentifier, user.Firstname, user.Lastname, user.Email,
                                user.CreatedOn, user.UpdatedOn));

                    return dto;
                },
                new {UserIdentifier = userIdentifier.Value},
                splitOn: "GroupIdentifier");

            token.ThrowIfCancellationRequested();

            return profiles.TryGetValue(userIdentifier.Value, out var profileDto)
                ? Result.Success(profileDto)
                : Result.Failure<ProfileDto>(ErrorKind.NotFound, "profile.not.found");
        }
        catch (Exception e)
        {
            return Result.Failure<ProfileDto>(ErrorKind.Unexpected, "database.error", e.Message);
        }
        finally
        {
            connection.Close();
        }
    }
}