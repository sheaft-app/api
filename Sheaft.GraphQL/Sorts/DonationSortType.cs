using HotChocolate.Data.Sorting;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Sorts
{
    public class DonationSortType : SortInputType<DonationDto>
    {
        protected override void Configure(ISortInputTypeDescriptor<DonationDto> descriptor)
        {
            descriptor.Name("DonationSort");
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