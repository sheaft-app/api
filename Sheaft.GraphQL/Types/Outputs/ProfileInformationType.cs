using HotChocolate.Types;
using Sheaft.Application.Common.Models.Dto;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class ProfileInformationType : SheaftOutputType<ProfileInformationDto>
    {
        protected override void Configure(IObjectTypeDescriptor<ProfileInformationDto> descriptor)
        {
            descriptor.Field(c => c.Summary);
            descriptor.Field(c => c.Description);
            descriptor.Field(c => c.Facebook);
            descriptor.Field(c => c.Twitter);
            descriptor.Field(c => c.Instagram);
            descriptor.Field(c => c.Website);

            descriptor.Field(c => c.Pictures)
                .Type<ListType<PictureType>>();
        }
    }
}