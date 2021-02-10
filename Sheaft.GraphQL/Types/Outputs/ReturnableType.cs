using HotChocolate.Types;
using Sheaft.Application.Common.Models.Dto;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class ReturnableType : SheaftOutputType<ReturnableDto>
    {
        protected override void Configure(IObjectTypeDescriptor<ReturnableDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.CreatedOn);
            descriptor.Field(c => c.Vat);
            descriptor.Field(c => c.VatPrice);
            descriptor.Field(c => c.OnSalePrice);
            descriptor.Field(c => c.WholeSalePrice);
            descriptor.Field(c => c.VatPrice);
            descriptor.Field(c => c.UpdatedOn);
            descriptor.Field(c => c.Description);

            descriptor.Field(c => c.Name)
                .Type<StringType>();
        }
    }
}
