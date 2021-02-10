using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;
using IdentityModel.Client;
using Newtonsoft.Json;
using System.Text;
using Microsoft.AspNetCore.Http;
using Sheaft.Application.Common;
using Sheaft.Application.Common.Extensions;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Models;
using Sheaft.Application.Common.Models.Inputs;
using Sheaft.Application.Common.Options;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Infrastructure.Services
{
    public class AuthService : BaseService, IAuthService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthOptions _authOptions;
        private readonly HttpClient _httpClient;

        public AuthService(
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor,
            IOptionsSnapshot<AuthOptions> authOptions,
            ILogger<AuthService> logger) : base(logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _authOptions = authOptions.Value;

            _httpClient = httpClientFactory.CreateClient("auth");
            _httpClient.BaseAddress = new Uri(_authOptions.Url);
            _httpClient.SetToken(_authOptions.Scheme, _authOptions.ApiKey);
        }

        public Result<RequestUser> GetCurrentUserInfo()
        {
            if (_httpContextAccessor.HttpContext == null)
                return Result<RequestUser>.Success(new RequestUser());

            if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
                return Result<RequestUser>.Success(
                    _httpContextAccessor.HttpContext.User.ToIdentityUser(_httpContextAccessor.HttpContext
                        .TraceIdentifier));

            return Result<RequestUser>.Success(new RequestUser(_httpContextAccessor.HttpContext.TraceIdentifier));
        }

        public async Task<Result> UpdateUserAsync(IdentityUserInput user, CancellationToken token)
        {
            var oidcResult = await _httpClient.PutAsync(_authOptions.Actions.Profile,
                new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json"), token);

            if (!oidcResult.IsSuccessStatusCode)
                return Failure(new Exception(await oidcResult.Content.ReadAsStringAsync().ConfigureAwait(false)));

            return Success();
        }

        public async Task<Result> UpdateUserPictureAsync(IdentityPictureInput user, CancellationToken token)
        {
            var oidcResult = await _httpClient.PutAsync(_authOptions.Actions.Picture,
                new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json"), token);

            if (!oidcResult.IsSuccessStatusCode)
                return Failure(new Exception(await oidcResult.Content.ReadAsStringAsync().ConfigureAwait(false)));

            return Success();
        }

        public async Task<Result> RemoveUserAsync(Guid userId, CancellationToken token)
        {
            var oidcResult =
                await _httpClient.DeleteAsync(string.Format(_authOptions.Actions.Delete, userId.ToString("N")), token);
            if (!oidcResult.IsSuccessStatusCode)
                return Failure(MessageKind.Oidc_DeleteProfile_Error,
                    await oidcResult.Content.ReadAsStringAsync());

            return Success();
        }
    }
}