using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class BrowserInfoInputType : SheaftInputType<BrowserInfoDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<BrowserInfoDto> descriptor)
        {
            descriptor.Name("BrowserInfoInput");
            descriptor.Field(c => c.AcceptHeader).Type<NonNullType<StringType>>();
            descriptor.Field(c => c.JavaEnabled).Type<NonNullType<BooleanType>>();
            descriptor.Field(c => c.Language).Type<NonNullType<StringType>>();
            descriptor.Field(c => c.ColorDepth).Type<NonNullType<IntType>>();
            descriptor.Field(c => c.ScreenHeight).Type<NonNullType<IntType>>();
            descriptor.Field(c => c.ScreenWidth).Type<NonNullType<IntType>>();
            descriptor.Field(c => c.TimeZoneOffset).Type<NonNullType<StringType>>();
            descriptor.Field(c => c.UserAgent).Type<NonNullType<StringType>>();
            descriptor.Field(c => c.JavascriptEnabled).Type<NonNullType<BooleanType>>();
        }
    }
}