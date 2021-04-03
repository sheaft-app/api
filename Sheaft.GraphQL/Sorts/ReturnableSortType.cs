using HotChocolate.Types.Sorting;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Sorts
{
    public class ReturnableSortType : SortInputType<ReturnableDto>
    {
        protected override void Configure(ISortInputTypeDescriptor<ReturnableDto> descriptor)
        {
            descriptor.Name("ReturnableSort");
            descriptor.BindFieldsExplicitly();
            descriptor.Sortable(c => c.Name);
            descriptor.Sortable(c => c.CreatedOn);
        }
    }
}
