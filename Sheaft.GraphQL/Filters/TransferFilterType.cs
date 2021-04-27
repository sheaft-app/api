using HotChocolate.Data.Filters;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Filters
{
    public class TransferFilterType : FilterInputType<TransferDto>
    {
        protected override void Configure(IFilterInputTypeDescriptor<TransferDto> descriptor)
        {
            descriptor.Name("TransferFilter");
            descriptor.BindFieldsExplicitly();
            descriptor.Field(c => c.Id);
            descriptor.Field(c => c.Status);
            descriptor.Field(c => c.CreatedOn);
            descriptor.Field(c => c.UpdatedOn);
        }
    }
}