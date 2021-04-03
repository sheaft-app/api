﻿using HotChocolate.Types.Sorting;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Sorts
{
    public class TransferSortType : SortInputType<TransferDto>
    {
        protected override void Configure(ISortInputTypeDescriptor<TransferDto> descriptor)
        {
            descriptor.Name("TransferSort");
            descriptor.BindFieldsExplicitly();
            descriptor.Sortable(c => c.Status);
            descriptor.Sortable(c => c.CreatedOn);
            descriptor.Sortable(c => c.Credited);
            descriptor.Sortable(c => c.Debited);
            descriptor.Sortable(c => c.UpdatedOn);
            descriptor.Sortable(c => c.Fees);
        }
    }
}