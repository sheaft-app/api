using HotChocolate.Types;
using Sheaft.Application.Common.Models.Inputs;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class UpdateUserProfileInputType : SheaftInputType<UpdateUserProfileInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateUserProfileInput> descriptor)
        {
            descriptor.Field(c => c.Summary);
            descriptor.Field(c => c.Description);
            descriptor.Field(c => c.Website);
            descriptor.Field(c => c.Facebook);
            descriptor.Field(c => c.Twitter);
            descriptor.Field(c => c.Instagram);
            
            descriptor.Field(c => c.Id)
                .Type<NonNullType<IdType>>();
        }
    }
}