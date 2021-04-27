using HotChocolate.Data.Sorting;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Sorts
{
    public class TransferSortType : SortInputType<TransferDto>
    {
        protected override void Configure(ISortInputTypeDescriptor<TransferDto> descriptor)
        {
            descriptor.Name("TransferSort");
            descriptor.BindFieldsExplicitly();
            descriptor.Field(c => c.Status);
            descriptor.Field(c => c.CreatedOn);
            descriptor.Field(c => c.Credited);
            descriptor.Field(c => c.Debited);
            descriptor.Field(c => c.UpdatedOn);
            descriptor.Field(c => c.Fees);
        }
    }
}