using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class ResourceIdTimeSlotsInputType : SheaftInputType<ResourceIdTimeSlotsDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<ResourceIdTimeSlotsDto> descriptor)
        {
            descriptor.Name("ResourceIdTimeSlotsInput");
            descriptor.Field(c => c.SelectedHours);

            descriptor.Field(c => c.Id)
                .Type<NonNullType<IdType>>();
        }
    }
}
