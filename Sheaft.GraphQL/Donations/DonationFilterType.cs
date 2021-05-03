using HotChocolate.Data.Filters;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Donations
{
    public class DonationFilterType : FilterInputType<DonationDto>
    {
        protected override void Configure(IFilterInputTypeDescriptor<DonationDto> descriptor)
        {
            descriptor.Name("DonationFilter");
            descriptor.BindFieldsExplicitly();
            descriptor.Field(c => c.Id);
            descriptor.Field(c => c.Status);
            descriptor.Field(c => c.CreatedOn);
            descriptor.Field(c => c.UpdatedOn);
        }
    }
}