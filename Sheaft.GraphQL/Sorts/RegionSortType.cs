using HotChocolate.Data.Sorting;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Sorts
{
    public class RegionSortType : SortInputType<RegionDto>
    {
        protected override void Configure(ISortInputTypeDescriptor<RegionDto> descriptor)
        {
            descriptor.Name("RegionSort");
            descriptor.BindFieldsExplicitly();
            descriptor.Field(c => c.Code);
        }
    }
}
