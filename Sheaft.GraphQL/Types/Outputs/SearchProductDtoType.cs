using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class SearchProductDtoType : SheaftOutputType<SearchProductDto>
    {
        protected override void Configure(IObjectTypeDescriptor<SearchProductDto> descriptor)
        {
            base.Configure(descriptor);
            
            descriptor
                .Field(c => c.OnSalePricePerUnit)
                .Name("onSalePricePerUnit");
                
            descriptor
                .Field(c => c.OnSalePrice)
                .Name("onSalePrice");
                
            descriptor
                .Field(c => c.RatingsCount)
                .Name("ratingsCount");
                
            descriptor
                .Field(c => c.QuantityPerUnit)
                .Name("quantityPerUnit");
                
            descriptor
                .Field(c => c.Unit)
                .Name("unit");
                
            descriptor
                .Field(c => c.Available)
                .Name("available");
                
            descriptor
                .Field(c => c.Conditioning)
                .Name("conditioning");
                
            descriptor
                .Field(c => c.Rating)
                .Name("rating");
                
            descriptor
                .Field(c => c.ImageMedium)
                .Name("imageMedium");
                
            descriptor
                .Field(c => c.ImageSmall)
                .Name("imageSmall");
                
            descriptor
                .Field(c => c.ImageLarge)
                .Name("imageLarge");
                
            descriptor
                .Field(c => c.Picture)
                .Name("picture");
                
            descriptor
                .Field(c => c.IsReturnable)
                .Name("isReturnable");
                
            descriptor
                .Field(c => c.Id)
                .Name("id")
                .Type<NonNullType<IdType>>();
            
            descriptor
                .Field(c => c.Name)
                .Name("name")
                .Type<NonNullType<StringType>>();

            descriptor
                .Field(c => c.Tags)
                .Name("tags")
                .Type<ListType<SearchTagDtoType>>();

            descriptor
                .Field(c => c.Producer)
                .Name("producer")
                .Type<NonNullType<SearchProductProducerDtoType>>();
        }
    }
}
