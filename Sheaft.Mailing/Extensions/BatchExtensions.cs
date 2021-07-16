using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace Sheaft.Mailing.Extensions
{
    public static class BatchExtensions
    {
        public static StringContent GetObservationNotificationContent(this Domain.Batch batch, string batchId, string batchObservationId, string url, string username)
        {
            return new StringContent(JsonConvert.SerializeObject(batch.GetObservationNotificationData(batchId, batchObservationId, url, null, username)), Encoding.UTF8, "application/json");
        }

        public static BatchMailerModel GetObservationNotificationData(this Domain.Batch batch, string batchId, string batchObservationId, string url, string comment,string username)
        {
            return new BatchMailerModel
            { 
                Username = username,
                Comment = comment,
                Number = batch.Number,
                DDM = batch.DDM,
                DLC = batch.DLC,
                CreatedOn = batch.CreatedOn, 
                PortalUrl = $"{url}/#/batches/{batchId}/observations/{batchObservationId}" 
            };
        }
    }
}