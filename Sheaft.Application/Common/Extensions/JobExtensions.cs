using Newtonsoft.Json;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.PickingOrders.Commands;
using Sheaft.Application.Product.Commands;
using Sheaft.Application.User.Commands;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Application.Common.Extensions
{
    public static class JobExtensions
    {
        public static void EnqueueJobCommand(this Domain.Job entity, ISheaftMediatr mediatr, RequestUser requestUser)
        {
            switch (entity.Kind)
            {
                case JobKind.ExportPickingOrders:
                    var exportPickingOrderCommand = JsonConvert.DeserializeObject<ExportPickingOrderCommand>(entity.Command);
                    mediatr.Post(new ExportPickingOrderCommand(requestUser) { JobId = exportPickingOrderCommand.JobId, PurchaseOrderIds = exportPickingOrderCommand.PurchaseOrderIds });
                    break;
                case JobKind.ExportUserData:
                    var exportUserDataCommand = JsonConvert.DeserializeObject<ExportUserDataCommand>(entity.Command);
                    mediatr.Post(new ExportUserDataCommand(requestUser) { Id = exportUserDataCommand.Id });
                    break;
                case JobKind.ImportProducts:
                    var importProductsCommand = JsonConvert.DeserializeObject<ImportProductsCommand>(entity.Command);
                    mediatr.Post(new ImportProductsCommand(requestUser) { Id = importProductsCommand.Id });
                    break;
            }
        }
    }
}