using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class AvailablePurchaseOrderDtoType : SheaftOutputType<AvailablePurchaseOrderDto>
    {
        protected override void Configure(IObjectTypeDescriptor<AvailablePurchaseOrderDto> descriptor)
        {
            base.Configure(descriptor);

            descriptor
                .Field(c => c.Id)
                .ID(nameof(PurchaseOrder))
                .Name("id");
            
            descriptor
                .Field(c => c.Reference)
                .Name("reference");
            
            descriptor
                .Field(c => c.Status)
                .Name("status");

            descriptor
                .Field(c => c.Address)
                .Name("address");

            descriptor
                .Field(c => c.Client)
                .Name("client");

            descriptor
                .Field(c => c.LinesCount)
                .Name("linesCount");

            descriptor
                .Field(c => c.ProductsCount)
                .Name("productsCount");

            descriptor
                .Field(c => c.ReturnablesCount)
                .Name("returnablesCount");

            descriptor
                .Field(c => c.TotalWeight)
                .Name("totalWeight");

            descriptor
                .Field(c => c.TotalOnSalePrice)
                .Name("totalOnSalePrice");

            descriptor
                .Field(c => c.TotalWholeSalePrice)
                .Name("totalWholeSalePrice");
        }
    }
}