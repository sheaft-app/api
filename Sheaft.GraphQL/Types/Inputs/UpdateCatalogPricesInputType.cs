using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class UpdateCatalogPricesInputType : SheaftInputType<UpdateCatalogPricesDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateCatalogPricesDto> descriptor)
        {
            descriptor.Name("UpdateCatalogPricesInput");
            
            descriptor.Field(c => c.Prices)
                .Type<NonNullType<ListType<UpdateOrCreateCatalogPriceInputType>>>();
            
            descriptor.Field(c => c.Id)
                .Type<NonNullType<IdType>>();
        }
    }
}