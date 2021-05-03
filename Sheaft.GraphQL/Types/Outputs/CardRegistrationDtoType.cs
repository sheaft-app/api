using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class CardRegistrationDtoType : SheaftOutputType<CardRegistrationDto>
    {
        protected override void Configure(IObjectTypeDescriptor<CardRegistrationDto> descriptor)
        {
            base.Configure(descriptor);
            
            descriptor
                .Field(c => c.Identifier)
                .Name("identifier");
                
            descriptor
                .Field(c => c.AccessKey)
                .Name("accessKey");
                
            descriptor
                .Field(c => c.PreRegistrationData)
                .Name("preRegistrationData");
                
            descriptor
                .Field(c => c.Url)
                .Name("url");
        }
    }
}