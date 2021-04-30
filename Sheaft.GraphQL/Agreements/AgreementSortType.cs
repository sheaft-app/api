using HotChocolate.Data.Sorting;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.GraphQL.Sorts
{
    public class AgreementSortType : SortInputType<Agreement>
    {
        protected override void Configure(ISortInputTypeDescriptor<Agreement> descriptor)
        {
            descriptor.Name("AgreementSort");
            descriptor.BindFieldsExplicitly();
            descriptor.Field(c => c.Status);
            descriptor.Field(c => c.CreatedOn);
        }
    }
}
