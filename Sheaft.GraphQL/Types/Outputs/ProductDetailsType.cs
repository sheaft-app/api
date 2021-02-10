using HotChocolate.Types;
using HotChocolate.Types.Relay;
using Sheaft.Application.Common.Interfaces.Queries;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.GraphQL.Filters;
using Sheaft.GraphQL.Sorts;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class ProductDetailsType : SheaftOutputType<ProductDto>
    {
        protected override void Configure(IObjectTypeDescriptor<ProductDto> descriptor)
        {
            descriptor.Name("ProductDetailsDto");

            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.CreatedOn);
            descriptor.Field(c => c.Vat);
            descriptor.Field(c => c.VatPricePerUnit);
            descriptor.Field(c => c.OnSalePricePerUnit);
            descriptor.Field(c => c.WholeSalePricePerUnit);
            descriptor.Field(c => c.OnSalePrice);
            descriptor.Field(c => c.WholeSalePrice);
            descriptor.Field(c => c.VatPrice);
            descriptor.Field(c => c.Available);
            descriptor.Field(c => c.VisibleToStores);
            descriptor.Field(c => c.VisibleToConsumers);
            descriptor.Field(c => c.RatingsCount);
            descriptor.Field(c => c.QuantityPerUnit);
            descriptor.Field(c => c.Unit);
            descriptor.Field(c => c.Conditioning);
            descriptor.Field(c => c.UpdatedOn);
            descriptor.Field(c => c.Description);
            descriptor.Field(c => c.Weight);
            descriptor.Field(c => c.Rating);
            descriptor.Field(c => c.Picture);
            descriptor.Field(c => c.ImageLarge);
            descriptor.Field(c => c.ImageMedium);
            descriptor.Field(c => c.ImageSmall);
            descriptor.Field(c => c.IsReturnable);
            descriptor.Field("currentUserHasRatedProduct")
                .Type<NonNullType<BooleanType>>()
                .Resolver(async c =>
                {
                    var user = GetRequestUser(c.ContextData);
                    if (!user.IsAuthenticated)
                        return false;

                    return await c.Service<IProductQueries>().ProductIsRatedByUserAsync(c.Parent<ProductDto>().Id, user.Id, user, c.RequestAborted);
                });

            descriptor.Field(c => c.Name)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Reference)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Returnable)
                .Type<ReturnableType>();

            descriptor.Field(c => c.Ratings)
                .Type<ListType<RatingType>>()
                .UseSorting<RatingSortType>()
                .UseFiltering<RatingFilterType>()
                .UsePaging<RatingType>();

            descriptor.Field(c => c.Tags)
                .Type<ListType<TagType>>()
                .UseFiltering<TagFilterType>();

            descriptor.Field(c => c.Producer)
                .Type<NonNullType<BusinessProfileType>>();
        }
    }
}
