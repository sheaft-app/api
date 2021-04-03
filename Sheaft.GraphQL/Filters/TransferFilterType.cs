﻿using HotChocolate.Types.Filters;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Filters
{
    public class TransferFilterType : FilterInputType<TransferDto>
    {
        protected override void Configure(IFilterInputTypeDescriptor<TransferDto> descriptor)
        {
            descriptor.Name("TransferFilter");
            descriptor.BindFieldsExplicitly();
            descriptor.Filter(c => c.Id).AllowEquals();
            descriptor.Filter(c => c.Status).AllowIn();
            descriptor.Filter(c => c.CreatedOn).AllowGreaterThanOrEquals();
            descriptor.Filter(c => c.CreatedOn).AllowLowerThanOrEquals();
            descriptor.Filter(c => c.UpdatedOn).AllowGreaterThanOrEquals();
            descriptor.Filter(c => c.UpdatedOn).AllowLowerThanOrEquals();
        }
    }
}