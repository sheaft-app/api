using HotChocolate.Types.Sorting;
using Sheaft.Application.Common.Models.Dto;

namespace Sheaft.GraphQL.Sorts
{
    public class RegionSortType : SortInputType<RegionDto>
    {
        protected override void Configure(ISortInputTypeDescriptor<RegionDto> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Sortable(c => c.Code);
        }
    }
}
