using HotChocolate.Data.Sorting;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Sorts
{
    public class JobSortType : SortInputType<JobDto>
    {
        protected override void Configure(ISortInputTypeDescriptor<JobDto> descriptor)
        {
            descriptor.Name("JobSort");
            descriptor.BindFieldsExplicitly();
            descriptor.Field(c => c.Status);
            descriptor.Field(c => c.CreatedOn);
            descriptor.Field(c => c.Name);
        }
    }
}
