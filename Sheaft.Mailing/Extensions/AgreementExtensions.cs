using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace Sheaft.Mailing.Extensions
{
    public static class AgreementExtensions
    {
        public static StringContent GetNotificationContent(this Domain.Agreement agreement, string url, string name)
        {
            return new StringContent(JsonConvert.SerializeObject(agreement.GetNotificationData(url, name)), Encoding.UTF8, "application/json");
        }

        public static AgreementMailerModel GetNotificationData(this Domain.Agreement agreement, string url,  string name)
        {
            return new AgreementMailerModel
            { 
                Name = name,
                Reason = agreement.Reason,
                AgreementId = agreement.Id, 
                CreatedOn = agreement.CreatedOn, 
                PortalUrl = $"{url}/#/agreements/{agreement.Id}" 
            };
        }
    }
}