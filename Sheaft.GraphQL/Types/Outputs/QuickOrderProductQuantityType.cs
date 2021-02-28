using HotChocolate.Types;
using Sheaft.Application.Common.Models.Dto;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class QuickOrderProductQuantityType : SheaftOutputType<QuickOrderProductQuantityDto>
    {
        protected override void Configure(IObjectTypeDescriptor<QuickOrderProductQuantityDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.Quantity);
            descriptor.Field(c => c.Vat);
            descriptor.Field(c => c.UnitOnSalePrice);
            descriptor.Field(c => c.UnitVatPrice);
            descriptor.Field(c => c.UnitWholeSalePrice);
            descriptor.Field(c => c.UnitVatPrice);
            descriptor.Field(c => c.UnitWeight);

            descriptor.Field(c => c.Name)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Reference)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Returnable)
                .Type<ReturnableType>();

            descriptor.Field(c => c.Producer)
                .Type<NonNullType<UserType>>();
        }
    }
}
