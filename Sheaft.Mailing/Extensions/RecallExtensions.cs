using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace Sheaft.Mailing.Extensions
{
    public static class RecallExtensions
    {
        public static StringContent GetNotificationContent(this Domain.Recall recall, string recallId, string url, string username)
        {
            return new StringContent(JsonConvert.SerializeObject(recall.GetNotificationData(recallId, url, null, username)), Encoding.UTF8, "application/json");
        }

        public static RecallMailerModel GetNotificationData(this Domain.Recall recall, string recallId, string url, string comment,string username)
        {
            return new RecallMailerModel
            { 
                ProducerName = recall.Producer.Name,
                User = username,
                Comment = comment,
                Batches = recall.Batches?.Select(b => new BatchMailerModel
                {
                    Number = b.Batch.Number,
                    DDM = b.Batch.DDM,
                    DLC = b.Batch.DLC,
                }).ToList() ?? new List<BatchMailerModel>(),
                Products = recall.Products?.Select(b => new ProductMailerModel
                {
                    Name = b.Name,
                    Reference = b.Reference
                }).ToList() ?? new List<ProductMailerModel>(),
                CreatedOn = recall.CreatedOn, 
                SaleEndedOn = recall.SaleEndedOn,
                SaleStartedOn = recall.SaleStartedOn,
                RecallId = recallId,
                PortalUrl = $"{url}/#/public/recalls/{recallId}" 
            };
        }
    }
}