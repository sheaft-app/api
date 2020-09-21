using HotChocolate.Types.Sorting;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Sorts
{
    public class DepartmentSortType : SortInputType<DepartmentDto>
    {
        protected override void Configure(ISortInputTypeDescriptor<DepartmentDto> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Sortable(c => c.Code);
        }
    }
}
