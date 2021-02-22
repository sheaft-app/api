using HotChocolate.Types;
using Sheaft.Application.Common.Models.Dto;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class PictureType : SheaftOutputType<PictureDto>
    {
        protected override void Configure(IObjectTypeDescriptor<PictureDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            
            descriptor.Field(c => c.Url);
        }
    }
}