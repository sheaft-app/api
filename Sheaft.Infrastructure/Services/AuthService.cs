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
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Models;
using Sheaft.Application.Services;
using Sheaft.Core;
using Sheaft.Core.Enums;
using Sheaft.Domain;
using Sheaft.Options;

namespace Sheaft.Infrastructure.Services
{
    public class AuthService : SheaftService, IAuthService
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

        public async Task<Result> UpdateUserAsync(IdentityUserDto user, CancellationToken token)
        {
            var oidcResult = await _httpClient.PutAsync(_authOptions.Actions.Profile,
                new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json"), token);

            if (!oidcResult.IsSuccessStatusCode)
                return Failure("Une erreur est survenue pendant la mise à jour du profil sur le serveur d'identité", await oidcResult.Content.ReadAsStringAsync().ConfigureAwait(false));

            return Success();
        }

        public async Task<Result> UpdateUserPictureAsync(IdentityPictureDto user, CancellationToken token)
        {
            var oidcResult = await _httpClient.PutAsync(_authOptions.Actions.Picture,
                new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json"), token);

            if (!oidcResult.IsSuccessStatusCode)
                return Failure("Une erreur est survenue pendant le chargement de l'image sur le serveur d'identité", await oidcResult.Content.ReadAsStringAsync().ConfigureAwait(false));

            return Success();
        }

        public async Task<Result> RemoveUserAsync(Guid userId, CancellationToken token)
        {
            var oidcResult =
                await _httpClient.DeleteAsync(string.Format(_authOptions.Actions.Delete, userId.ToString("N")), token);
            if (!oidcResult.IsSuccessStatusCode)
                return Failure("Une erreur est survenue pendant la suppression de l'utilisateur sur le serveur d'identité.",
                    await oidcResult.Content.ReadAsStringAsync());

            return Success();
        }
    }
}