using HotChocolate.Data.Filters;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Filters
{
    public class AgreementFilterType : FilterInputType<AgreementDto>
    {
        protected override void Configure(IFilterInputTypeDescriptor<AgreementDto> descriptor)
        {
            descriptor.Name("AgreementFilter");
            descriptor.BindFieldsExplicitly();
            descriptor.Field(c => c.Id);
            descriptor.Field(c => c.Status);
        }
    }
}
