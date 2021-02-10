using System.Net.Http;
using System.Text;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Sheaft.Application.Models.Mailer;

namespace Sheaft.Application.Common.Extensions
{
    public static class AgreementExtensions
    {
        public static StringContent GetNotificationContent(this Domain.Models.Agreement agreement, IConfiguration configuration, string name)
        {
            return new StringContent(JsonConvert.SerializeObject(agreement.GetNotificationDatas(configuration, name)), Encoding.UTF8, "application/json");
        }

        public static AgreementMailerModel GetNotificationDatas(this Domain.Models.Agreement agreement, IConfiguration configuration,  string name)
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