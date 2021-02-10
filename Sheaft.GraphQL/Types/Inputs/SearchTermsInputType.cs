using HotChocolate.Types;
using Sheaft.Application.Common.Models.Inputs;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class SearchTermsInputType : SheaftInputType<SearchTermsInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<SearchTermsInput> descriptor)
        {
            descriptor.Field(c => c.Latitude);
            descriptor.Field(c => c.Longitude);
            descriptor.Field(c => c.MaxDistance);
            descriptor.Field(c => c.Page);
            descriptor.Field(c => c.Sort);
            descriptor.Field(c => c.Tags);
            descriptor.Field(c => c.Take);
            descriptor.Field(c => c.Text);
        }
    }
}
