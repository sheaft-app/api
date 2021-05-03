using HotChocolate.Data.Filters;
using Sheaft.Domain;

namespace Sheaft.GraphQL.Agreements
{
    public class AgreementFilterType : FilterInputType<Agreement>
    {
        protected override void Configure(IFilterInputTypeDescriptor<Agreement> descriptor)
        {
            descriptor.Name("AgreementFilter");
            descriptor.BindFieldsExplicitly();
            descriptor.Field(c => c.Id);
            descriptor.Field(c => c.Status);
            descriptor.Field(c => c.Store);
            descriptor.Field(c => c.Producer);
            descriptor.Field(c => c.CreatedByKind);
        }
    }
}
