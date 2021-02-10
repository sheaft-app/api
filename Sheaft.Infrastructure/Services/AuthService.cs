using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sheaft.Core;
using Sheaft.Application.Models;
using Sheaft.Options;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;
using IdentityModel.Client;
using Newtonsoft.Json;
using System.Text;
using Sheaft.Application.Interop;
using Sheaft.Domain.Enums;

namespace Sheaft.Infrastructure.Services
{
    public class AuthService : BaseService, IAuthService
    {
        private readonly AuthOptions _authOptions;
        private readonly HttpClient _httpClient;

        public AuthService(
            IHttpClientFactory httpClientFactory,
            IOptionsSnapshot<AuthOptions> authOptions,
            ILogger<AuthService> logger) : base(logger)
        {
            _authOptions = authOptions.Value;

            _httpClient = httpClientFactory.CreateClient("auth");
            _httpClient.BaseAddress = new Uri(_authOptions.Url);
            _httpClient.SetToken(_authOptions.Scheme, _authOptions.ApiKey);
        }

        public async Task<Result<bool>> UpdateUserAsync(IdentityUserInput user, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var oidcResult = await _httpClient.PutAsync(_authOptions.Actions.Profile,
                    new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json"), token);

                if (!oidcResult.IsSuccessStatusCode)
                    return Failed<bool>(new Exception(await oidcResult.Content.ReadAsStringAsync().ConfigureAwait(false)));

                return Ok(true);
            });
        }

        public async Task<Result<bool>> UpdateUserPictureAsync(IdentityPictureInput user, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var oidcResult = await _httpClient.PutAsync(_authOptions.Actions.Picture,
                    new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json"), token);

                if (!oidcResult.IsSuccessStatusCode)
                    return Failed<bool>(new Exception(await oidcResult.Content.ReadAsStringAsync().ConfigureAwait(false)));

                return Ok(true);
            });
        }

        public async Task<Result<bool>> RemoveUserAsync(Guid userId, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var oidcResult = await _httpClient.DeleteAsync(string.Format(_authOptions.Actions.Delete, userId.ToString("N")), token);
                if (!oidcResult.IsSuccessStatusCode)
                    return BadRequest<bool>(MessageKind.Oidc_DeleteProfile_Error, await oidcResult.Content.ReadAsStringAsync().ConfigureAwait(false));

                return Ok(true);
            });
        }
    }
}
