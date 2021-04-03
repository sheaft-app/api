﻿using HotChocolate.Types.Filters;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Filters
{
    public class JobFilterType : FilterInputType<JobDto>
    {
        protected override void Configure(IFilterInputTypeDescriptor<JobDto> descriptor)
        {
            descriptor.Name("JobFilter");
            descriptor.BindFieldsExplicitly();
            descriptor.Filter(c => c.Id).AllowEquals();
            descriptor.Filter(c => c.Status).AllowIn();
            descriptor.Filter(c => c.Kind).AllowIn();
            descriptor.Filter(c => c.Name).AllowContains();
        }
    }
}
