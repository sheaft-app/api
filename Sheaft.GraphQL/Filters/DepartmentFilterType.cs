using HotChocolate.Data.Filters;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Filters
{
    public class DepartmentFilterType : FilterInputType<DepartmentDto>
    {
        protected override void Configure(IFilterInputTypeDescriptor<DepartmentDto> descriptor)
        {
            descriptor.Name("DepartmentFilter");
            descriptor.BindFieldsExplicitly();
            descriptor.Field(c => c.Id);
            descriptor.Field(c => c.Name);
            descriptor.Field(c => c.Code);
        }
    }
}
