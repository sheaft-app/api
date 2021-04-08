using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class UpdateAllCatalogPricesInputType : SheaftInputType<UpdateAllCatalogPricesDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateAllCatalogPricesDto> descriptor)
        {
            descriptor.Name("UpdateAllCatalogPricesInput");
            
            descriptor.Field(c => c.Percent);
            descriptor.Field(c => c.Id)
                .Type<NonNullType<IdType>>();
        }
    }
}