using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class BrowserInfoInputType : SheaftInputType<BrowserInfoDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<BrowserInfoDto> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("BrowserInfoInput");
            
            descriptor
                .Field(c => c.JavaEnabled)
                .Name("javaEnabled")
                .Type<NonNullType<BooleanType>>();
                
            descriptor
                .Field(c => c.Language)
                .Name("language")
                .Type<NonNullType<StringType>>();
                
            descriptor
                .Field(c => c.ColorDepth)
                .Name("colorDepth")
                .Type<NonNullType<IntType>>();
                
            descriptor
                .Field(c => c.ScreenHeight)
                .Name("screenHeight")
                .Type<NonNullType<IntType>>();
                
            descriptor
                .Field(c => c.ScreenWidth)
                .Name("screenWidth")
                .Type<NonNullType<IntType>>();
                
            descriptor
                .Field(c => c.TimeZoneOffset)
                .Name("timeZoneOffset")
                .Type<NonNullType<StringType>>();
                
            descriptor
                .Field(c => c.UserAgent)
                .Name("userAgent")
                .Type<NonNullType<StringType>>();
                
            descriptor
                .Field(c => c.JavascriptEnabled)
                .Name("javascriptEnabled")
                .Type<NonNullType<BooleanType>>();
        }
    }
}