using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class AvailableDeliveryBatchDtoType : SheaftOutputType<AvailableDeliveryBatchDto>
    {
        protected override void Configure(IObjectTypeDescriptor<AvailableDeliveryBatchDto> descriptor)
        {
            base.Configure(descriptor);
            
            descriptor
                .Field(c => c.Name)
                .Name("name");
            
            descriptor
                .Field(c => c.Day)
                .Name("day");
            
            descriptor
                .Field(c => c.From)
                .Name("from");
            
            descriptor
                .Field(c => c.To)
                .Name("to");
            
            descriptor
                .Field(c => c.ExpectedDeliveryDate)
                .Name("expectedDeliveryDate");
            
            descriptor
                .Field(c => c.PurchaseOrdersCount)
                .Name("purchaseOrdersCount");
            
            descriptor
                .Field(c => c.ClientsCount)
                .Name("clientsCount");
            
            descriptor
                .Field(c => c.ProductsCount)
                .Name("productsCount");
            
            descriptor
                .Field(c => c.Clients)
                .Name("clients")
                .Type<ListType<AvailableClientDeliveryDtoType>>();
        }
    }
}