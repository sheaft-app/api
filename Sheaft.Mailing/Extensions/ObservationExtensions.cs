using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace Sheaft.Mailing.Extensions
{
    public static class ObservationExtensions
    {
        public static StringContent GetNotificationContent(this Domain.Observation observation, string observationId, string url, string username)
        {
            return new StringContent(JsonConvert.SerializeObject(observation.GetNotificationData(observationId, url, null, username)), Encoding.UTF8, "application/json");
        }

        public static ObservationMailerModel GetNotificationData(this Domain.Observation observation, string observationId, string url, string comment, string username)
        {
            return new ObservationMailerModel
            { 
                Producer = observation.Producer.Name,
                User = username,
                Comment = comment,
                Batches = observation.Batches?.Select(b => new BatchMailerModel
                {
                    Number = b.Batch.Number,
                    DDM = b.Batch.DDM,
                    DLC = b.Batch.DLC,
                }).ToList() ?? new List<BatchMailerModel>(),
                Products = observation.Products?.Select(b => new ProductMailerModel
                {
                    Name = b.Name,
                    Reference = b.Reference
                }).ToList() ?? new List<ProductMailerModel>(),
                CreatedOn = observation.CreatedOn, 
                PortalUrl = $"{url}/#/observations/{observationId}" 
            };
        }
    }
}