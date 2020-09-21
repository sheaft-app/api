using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types
{
    public class SearchProductType : SheaftOutputType<ProductDto>
    {
        protected override void Configure(IObjectTypeDescriptor<ProductDto> descriptor)
        {
            descriptor.Name("SearchProductDto");

            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.OnSalePricePerUnit);
            descriptor.Field(c => c.OnSalePrice);
            descriptor.Field(c => c.RatingsCount);
            descriptor.Field(c => c.QuantityPerUnit);
            descriptor.Field(c => c.Unit);
            descriptor.Field(c => c.Conditioning);
            descriptor.Field(c => c.Rating);
            descriptor.Field(c => c.ImageMedium);
            descriptor.Field(c => c.Picture);
            descriptor.Field(c => c.IsReturnable);

            descriptor.Field(c => c.Name)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Tags)
                .Type<ListType<SearchTagType>>();

            descriptor.Field(c => c.Producer)
                .Type<NonNullType<SearchProductProducerType>>();
        }
    }
}
