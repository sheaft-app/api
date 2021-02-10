using HotChocolate.Types.Sorting;
using Sheaft.Application.Common.Models.Dto;

namespace Sheaft.GraphQL.Sorts
{
    public class ReturnableSortType : SortInputType<ReturnableDto>
    {
        protected override void Configure(ISortInputTypeDescriptor<ReturnableDto> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Sortable(c => c.Name);
            descriptor.Sortable(c => c.CreatedOn);
        }
    }
}
