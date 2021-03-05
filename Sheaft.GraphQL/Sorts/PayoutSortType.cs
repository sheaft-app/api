using HotChocolate.Types.Sorting;
using Sheaft.Application.Common.Models.Dto;

namespace Sheaft.GraphQL.Sorts
{
    public class PayoutSortType : SortInputType<PayoutDto>
    {
        protected override void Configure(ISortInputTypeDescriptor<PayoutDto> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Sortable(c => c.Status);
            descriptor.Sortable(c => c.CreatedOn);
            descriptor.Sortable(c => c.Credited);
            descriptor.Sortable(c => c.Debited);
            descriptor.Sortable(c => c.UpdatedOn);
            descriptor.Sortable(c => c.Fees);
        }
    }
}