using IdentityModel.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sheaft.Options;
using Sheaft.Services.Interop;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Sheaft.Services
{
    public class SignalrService : ISignalrService
    {
        private readonly HttpClient _httpClient;
        private readonly SignalrOptions _signalrOptions;
        private readonly ILogger<SignalrService> _logger;

        public SignalrService(
            IOptionsSnapshot<SignalrOptions> signalrOptions,
            IHttpClientFactory httpClientFactory,
            ILogger<SignalrService> logger)
        {
            _logger = logger;
            _signalrOptions = signalrOptions.Value;

            _httpClient = httpClientFactory.CreateClient("signalR");
            _httpClient.BaseAddress = new Uri(_signalrOptions.Url);
            _httpClient.SetToken(_signalrOptions.Scheme, _signalrOptions.ApiKey);
        }

        public async Task SendNotificationToGroupAsync<T>(Guid groupId, string eventName, T content)
        {
            try
            {
                await _httpClient.PostAsync(string.Format(_signalrOptions.NotifyGroupUrl, groupId, eventName), new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json"));
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
            }
        }

        public async Task SendNotificationToUserAsync<T>(Guid userId, string eventName, T content)
        {
            try
            {
                await _httpClient.PostAsync(string.Format(_signalrOptions.NotifyUserUrl, userId, eventName), new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json"));
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
            }
        }
    }
}
