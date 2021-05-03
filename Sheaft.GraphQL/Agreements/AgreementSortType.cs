using HotChocolate.Data.Sorting;
using Sheaft.Domain;

namespace Sheaft.GraphQL.Agreements
{
    public class AgreementSortType : SortInputType<Agreement>
    {
        protected override void Configure(ISortInputTypeDescriptor<Agreement> descriptor)
        {
            descriptor.Name("AgreementSort");
            descriptor.BindFieldsExplicitly();
            descriptor.Field(c => c.Status);
            descriptor.Field(c => c.CreatedOn);
            descriptor.Field(c => c.CreatedByKind);
            descriptor.Field(c => c.Producer);
            descriptor.Field(c => c.Store);
        }
    }
}
