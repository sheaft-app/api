using System.Net.Http;
using System.Text;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Sheaft.Application.Mailings;

namespace Sheaft.Application.Extensions
{
    public static class AgreementExtensions
    {
        public static StringContent GetNotificationContent(this Domain.Agreement agreement, IConfiguration configuration, string name)
        {
            return new StringContent(JsonConvert.SerializeObject(agreement.GetNotificationData(configuration, name)), Encoding.UTF8, "application/json");
        }

        public static AgreementMailerModel GetNotificationData(this Domain.Agreement agreement, IConfiguration configuration,  string name)
        {
            return new AgreementMailerModel
            { 
                Name = name,
                Reason = agreement.Reason,
                AgreementId = agreement.Id, 
                CreatedOn = agreement.CreatedOn, 
                PortalUrl = $"{configuration.GetValue<string>("Portal:url")}/#/agreements/{agreement.Id}" 
            };
        }
    }
}