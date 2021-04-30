using HotChocolate.Data.Filters;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.GraphQL.Filters
{
    public class AgreementFilterType : FilterInputType<Agreement>
    {
        protected override void Configure(IFilterInputTypeDescriptor<Agreement> descriptor)
        {
            descriptor.Name("AgreementFilter");
            descriptor.BindFieldsExplicitly();
            descriptor.Field(c => c.Id);
            descriptor.Field(c => c.Status);
        }
    }
}
