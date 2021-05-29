using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace Sheaft.Mailing.Extensions
{
    public static class AgreementExtensions
    {
        public static StringContent GetNotificationContent(this Domain.Agreement agreement, string agreementId, string url, string name)
        {
            return new StringContent(JsonConvert.SerializeObject(agreement.GetNotificationData(agreementId, url, name)), Encoding.UTF8, "application/json");
        }

        public static AgreementMailerModel GetNotificationData(this Domain.Agreement agreement, string agreementId, string url,  string name)
        {
            return new AgreementMailerModel
            { 
                Name = name,
                Reason = agreement.Reason,
                AgreementId = agreementId, 
                CreatedOn = agreement.CreatedOn, 
                PortalUrl = $"{url}/#/agreements/{agreement.Id}" 
            };
        }
    }
}