using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace Sheaft.Mailing.Extensions
{
    public static class ObservationExtensions
    {
        public static StringContent GetNotificationContent(this Domain.Observation observation, string observationId, string url, string producerId)
        {
            return new StringContent(JsonConvert.SerializeObject(observation.GetNotificationData(observationId, url, null, producerId)), Encoding.UTF8, "application/json");
        }

        public static ObservationMailerModel GetNotificationData(this Domain.Observation observation, string observationId, string url, string comment, string producerId)
        {
            return new ObservationMailerModel
            { 
                ProducerId = producerId,
                Producer = observation.Producer.Name,
                User = observation.User.Name,
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
                ObservationId = observationId,
                PortalUrl = url,
            };
        }
    }
}