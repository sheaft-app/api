using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class UpdateOrCreateCatalogPriceInputType : SheaftInputType<UpdateOrCreateCatalogPriceDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateOrCreateCatalogPriceDto> descriptor)
        {
            descriptor.Name("UpdateOrCreateCatalogPriceInput");
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.WholeSalePricePerUnit);
        }
    }
}