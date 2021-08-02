using IdentityModel.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Options;

namespace Sheaft.Infrastructure.Services
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
                var contentToSend = new StringContent(string.Empty, Encoding.UTF8,"application/json");
                var type = content.GetType();
                if (type == typeof(StringContent))
                {
                    var stringContent = content as StringContent;
                    if (stringContent != null)
                        contentToSend = new StringContent(await stringContent.ReadAsStringAsync(), Encoding.UTF8,"application/json");
                }
                else if (type == typeof(string))
                {
                    var stringContent = content as string;
                    if (stringContent != null)
                        contentToSend = new StringContent(stringContent, Encoding.UTF8, "application/json");
                }
                else
                    contentToSend = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8,"application/json");

                await _httpClient.PostAsync(string.Format(_signalrOptions.NotifyUserUrl, userId, eventName), contentToSend);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
            }
        }
    }
}
