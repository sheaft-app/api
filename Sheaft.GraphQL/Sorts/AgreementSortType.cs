using HotChocolate.Data.Sorting;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.GraphQL.Sorts
{
    public class AgreementSortType : SortInputType<AgreementDto>
    {
        protected override void Configure(ISortInputTypeDescriptor<AgreementDto> descriptor)
        {
            descriptor.Name("AgreementSort");
            descriptor.BindFieldsExplicitly();
            descriptor.Field(c => c.Status);
            descriptor.Field(c => c.CreatedOn);
        }
    }
}
