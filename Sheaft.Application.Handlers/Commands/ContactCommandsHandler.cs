using Sheaft.Application.Commands;
using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Core;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using IdentityModel.Client;
using System.Net.Http.Headers;
using Sheaft.Options;
using Microsoft.Extensions.Options;
using Sheaft.Interop.Enums;
using Microsoft.Extensions.Logging;

namespace Sheaft.Application.Handlers
{
    public class ContactCommandsHandler : CommandsHandler,
        IRequestHandler<CreateContactCommand, CommandResult<bool>>
    {
        private readonly SendgridOptions _sendgridOptions;
        private readonly HttpClient _httpClient;

        public ContactCommandsHandler(
            IOptionsSnapshot<SendgridOptions> sendgridOptions, 
            IHttpClientFactory httpClientFactory,
            ILogger<ContactCommandsHandler> logger) : base(logger)
        {
            _sendgridOptions = sendgridOptions.Value;

            _httpClient = httpClientFactory.CreateClient("sendgrid");
            _httpClient.BaseAddress = new Uri(_sendgridOptions.Url);
            _httpClient.SetToken("Bearer", _sendgridOptions.ApiKey);
        }

        public async Task<CommandResult<bool>> Handle(CreateContactCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {                
                var regex = new Regex(@"^[\w\.\-\+]+\@[\w\-]+\.\w{2,}$");
                if (!regex.Match(request.Email).Success)
                    return ValidationResult<bool>(MessageKind.EmailProvider_Newsletter_Email_Invalid);

                var obj = new Contact { ListIds = new List<string> { _sendgridOptions.Groups.Newsletter }.ToArray(), Contacts = new List<ContactElement> { new ContactElement { Email = request.Email.ToLowerInvariant(), FirstName = request.FirstName, CustomFields = new CustomFields { E1T = request.Role } } }.ToArray() };
                var httpRequest = new HttpRequestMessage(HttpMethod.Put, "/v3/marketing/contacts" )
                {
                    Content = new StringContent(JsonConvert.SerializeObject(obj)),
                };

                httpRequest.Content.Headers.Clear();
                httpRequest.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var recipientResult = await _httpClient.SendAsync(httpRequest, token);
                if((int)recipientResult.StatusCode >= 400)
                    return BadRequestResult<bool>(MessageKind.EmailProvider_Newsletter_RegisterFailure);

                return OkResult(true);
            });
        }

        public class Contact
        {
            [JsonProperty("list_ids")]
            public string[] ListIds { get; set; }

            [JsonProperty("contacts")]
            public ContactElement[] Contacts { get; set; }
        }

        public class ContactElement
        {
            [JsonProperty("email")]
            public string Email { get; set; }

            [JsonProperty("first_name")]
            public string FirstName { get; set; }

            [JsonProperty("custom_fields")]
            public CustomFields CustomFields { get; set; }
        }

        public class CustomFields
        {
            [JsonProperty("e1_T")]
            public string E1T { get; set; }
        }
    }
}