using HotChocolate.Data.Sorting;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Returnables
{
    public class ReturnableSortType : SortInputType<ReturnableDto>
    {
        protected override void Configure(ISortInputTypeDescriptor<ReturnableDto> descriptor)
        {
            descriptor.Name("ReturnableSort");
            descriptor.BindFieldsExplicitly();
            descriptor.Field(c => c.Name);
            descriptor.Field(c => c.CreatedOn);
            descriptor.Field(c => c.OnSalePrice);
        }
    }
}
