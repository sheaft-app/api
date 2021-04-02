using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.GraphQL.Enums;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class CreateCatalogInputType : SheaftInputType<CreateCatalogDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreateCatalogDto> descriptor)
        {
            descriptor.Name("CreateCatalogInput");
            descriptor.Field(c => c.Name);

            descriptor.Field(c => c.Kind)
                .Type<NonNullType<CatalogKindEnumType>>();

            descriptor.Field(c => c.IsAvailable);
            descriptor.Field(c => c.IsDefault);
        }
    }
    public class UpdateCatalogInputType : SheaftInputType<UpdateCatalogDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateCatalogDto> descriptor)
        {
            descriptor.Name("UpdateCatalogInput");
            descriptor.Field(c => c.Name);

            descriptor.Field(c => c.Id)
                .Type<NonNullType<IdType>>();

            descriptor.Field(c => c.IsAvailable);
            descriptor.Field(c => c.IsDefault);
        }
    }
    public class CloneCatalogInputType : SheaftInputType<CloneCatalogDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CloneCatalogDto> descriptor)
        {
            descriptor.Name("CloneCatalogInput");
            descriptor.Field(c => c.Name);

            descriptor.Field(c => c.Id)
                .Type<NonNullType<IdType>>();

            descriptor.Field(c => c.Percent);
        }
    }
    public class AddProductsToCatalogInputType : SheaftInputType<AddProductsToCatalogDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<AddProductsToCatalogDto> descriptor)
        {
            descriptor.Name("AddProductsToCatalogInput");
            descriptor.Field(c => c.Products)
                .Type<NonNullType<ListType<UpdateOrCreateCatalogPriceInputType>>>();

            descriptor.Field(c => c.Id)
                .Type<NonNullType<IdType>>();
        }
    }
    public class RemoveProductsFromCatalogInputType : SheaftInputType<RemoveProductsFromCatalogDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<RemoveProductsFromCatalogDto> descriptor)
        {
            descriptor.Name("RemoveProductsFromCatalogInput");
            descriptor.Field(c => c.ProductIds)
                .Type<NonNullType<ListType<IdType>>>();

            descriptor.Field(c => c.Id)
                .Type<NonNullType<IdType>>();
        }
    }
    public class UpdateAllCatalogPricesInputType : SheaftInputType<UpdateAllCatalogPricesDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateAllCatalogPricesDto> descriptor)
        {
            descriptor.Name("UpdateAllCatalogPricesInput");
            
            descriptor.Field(c => c.Percent);
            descriptor.Field(c => c.Id)
                .Type<NonNullType<IdType>>();
        }
    }
    public class UpdateCatalogPricesInputType : SheaftInputType<UpdateCatalogPricesDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateCatalogPricesDto> descriptor)
        {
            descriptor.Name("UpdateCatalogPricesInput");
            
            descriptor.Field(c => c.Prices)
                .Type<NonNullType<ListType<UpdateOrCreateCatalogPriceInputType>>>();
            
            descriptor.Field(c => c.Id)
                .Type<NonNullType<IdType>>();
        }
    }
}