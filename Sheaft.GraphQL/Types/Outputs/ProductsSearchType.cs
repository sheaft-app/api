﻿using HotChocolate.Types;
using Sheaft.Application.Common.Models.Dto;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class ProductsSearchType : SheaftOutputType<ProductsSearchDto>
    {
        protected override void Configure(IObjectTypeDescriptor<ProductsSearchDto> descriptor)
        {
            descriptor.Field(c => c.Count);
            descriptor.Field(c => c.Products).Type<ListType<SearchProductType>>();
        }
    }
}