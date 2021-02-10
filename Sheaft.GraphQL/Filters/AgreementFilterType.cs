﻿using HotChocolate.Types.Filters;
using Sheaft.Application.Common.Models.Dto;

namespace Sheaft.GraphQL.Filters
{
    public class AgreementFilterType : FilterInputType<AgreementDto>
    {
        protected override void Configure(IFilterInputTypeDescriptor<AgreementDto> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Filter(c => c.Id).AllowEquals();
            descriptor.Filter(c => c.Status).AllowIn();
        }
    }
}
