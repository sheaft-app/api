using HotChocolate.Types;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.GraphQL.Filters;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class ProducerType : SheaftOutputType<ProducerDto>
    {
        protected override void Configure(IObjectTypeDescriptor<ProducerDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.Phone);
            descriptor.Field(c => c.Picture);
            descriptor.Field(c => c.Description);
            descriptor.Field(c => c.VatIdentifier);
            descriptor.Field(c => c.OpenForNewBusiness);
            descriptor.Field(c => c.NotSubjectToVat);
            descriptor.Field(c => c.FirstName);
            descriptor.Field(c => c.LastName);
            descriptor.Field(c => c.Email);
            descriptor.Field(c => c.Siret);
            descriptor.Field(c => c.Name);

            descriptor.Field(c => c.Address)
                .Type<NonNullType<AddressType>>();

            descriptor.Field(c => c.Tags)
                .Type<ListType<TagType>>()
                .UseFiltering<TagFilterType>();
        }
    }
}
