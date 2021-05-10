using HotChocolate.Data.Filters;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Jobs
{
    public class JobFilterType : FilterInputType<JobDto>
    {
        protected override void Configure(IFilterInputTypeDescriptor<JobDto> descriptor)
        {
            descriptor.Name("JobFilter");
            descriptor.BindFieldsExplicitly();
            descriptor.Field(c => c.Id);
            descriptor.Field(c => c.Status);
            descriptor.Field(c => c.Kind);
            descriptor.Field(c => c.Name);
        }
    }
}
