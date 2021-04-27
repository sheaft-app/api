using HotChocolate.Data.Sorting;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Sorts
{
    public class DepartmentSortType : SortInputType<DepartmentDto>
    {
        protected override void Configure(ISortInputTypeDescriptor<DepartmentDto> descriptor)
        {
            descriptor.Name("DepartmentSort");
            descriptor.BindFieldsExplicitly();
            descriptor.Field(c => c.Code);
        }
    }
}
