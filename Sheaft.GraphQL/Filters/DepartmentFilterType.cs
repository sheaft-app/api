﻿using HotChocolate.Types.Filters;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Filters
{
    public class DepartmentFilterType : FilterInputType<DepartmentDto>
    {
        protected override void Configure(IFilterInputTypeDescriptor<DepartmentDto> descriptor)
        {
            descriptor.Name("DepartmentFilter");
            descriptor.BindFieldsExplicitly();
            descriptor.Filter(c => c.Id).AllowEquals();
            descriptor.Filter(c => c.Name).AllowContains();
            descriptor.Filter(c => c.Code).AllowEquals();
        }
    }
}
