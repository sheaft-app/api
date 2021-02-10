using HotChocolate.Types.Sorting;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Sorts
{
    public class AgreementSortType : SortInputType<AgreementDto>
    {
        protected override void Configure(ISortInputTypeDescriptor<AgreementDto> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Sortable(c => c.Status);
            descriptor.Sortable(c => c.CreatedOn);
        }
    }
}
