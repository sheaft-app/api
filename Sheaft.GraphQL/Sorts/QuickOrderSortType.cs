using HotChocolate.Data.Sorting;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Sorts
{
    public class QuickOrderSortType : SortInputType<QuickOrderDto>
    {
        protected override void Configure(ISortInputTypeDescriptor<QuickOrderDto> descriptor)
        {
            descriptor.Name("QuickOrderSort");
            descriptor.BindFieldsExplicitly();
            descriptor.Field(c => c.Name);
        }
    }
}
