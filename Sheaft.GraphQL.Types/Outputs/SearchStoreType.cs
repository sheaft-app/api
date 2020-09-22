﻿using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types
{
    public class SearchStoreType : SheaftOutputType<StoreDto>
    {
        protected override void Configure(IObjectTypeDescriptor<StoreDto> descriptor)
        {
            descriptor.Name("SearchStoreDto");

            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.Phone);
            descriptor.Field(c => c.Picture);

            descriptor.Field(c => c.Tags)
                .Type<ListType<SearchTagType>>();

            descriptor.Field(c => c.Name)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Email)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Address)
                .Type<NonNullType<SearchAddressType>>();
        }
    }
}