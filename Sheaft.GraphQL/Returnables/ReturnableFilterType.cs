using HotChocolate.Data.Filters;
using Sheaft.Domain;

namespace Sheaft.GraphQL.Returnables
{
    public class ReturnableFilterType : FilterInputType<Returnable>
    {
        protected override void Configure(IFilterInputTypeDescriptor<Returnable> descriptor)
        {
            descriptor.Name("ReturnableFilter");
            descriptor.BindFieldsExplicitly();
            descriptor.Field(c => c.Id);
            descriptor.Field(c => c.Name);
            descriptor.Field(c => c.Kind);
        }
    }
}
