using HotChocolate.Types;
using Sheaft.GraphQL.Enums;
using Sheaft.Models.Inputs;

namespace Sheaft.GraphQL.Types
{
    public class CreateAgreementInputType : SheaftInputType<CreateAgreementInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreateAgreementInput> descriptor)
        {
            descriptor.Field(c => c.DeliveryModeId)
                .Type<NonNullType<IdType>>();

            descriptor.Field(c => c.SelectedHours)
                .Type<ListType<TimeSlotGroupInputType>>();

            descriptor.Field(c => c.StoreId)
                .Type<NonNullType<IdType>>();
        }
    }
    public class CreatePurchaseOrderInputType : SheaftInputType<CreatePurchaseOrderInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreatePurchaseOrderInput> descriptor)
        {
            descriptor.Field(c => c.Comment);
            descriptor.Field(c => c.ExpectedDeliveryDate);

            descriptor.Field(c => c.DeliveryModeId)
                .Type<NonNullType<IdType>>();

            descriptor.Field(c => c.ProducerId)
                .Type<NonNullType<IdType>>();

            descriptor.Field(c => c.Products)
                .Type<NonNullType<ListType<ProductQuantityInputType>>>();
        }
    }
    public class CreatePurchaseOrdersInputType : SheaftInputType<CreatePurchaseOrdersInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreatePurchaseOrdersInput> descriptor)
        {
            descriptor.Field(c => c.ProducersExpectedDeliveries)
                .Type<NonNullType<ListType<ProducerExpectedDeliveryInputType>>>();

            descriptor.Field(c => c.Products)
                .Type<NonNullType<ListType<ProductQuantityInputType>>>();
        }
    }
    public class CreateQuickOrderInputType : SheaftInputType<CreateQuickOrderInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreateQuickOrderInput> descriptor)
        {
            descriptor.Field(c => c.Description);
            descriptor.Field(c => c.Products);

            descriptor.Field(c => c.Name)
                .Type<NonNullType<StringType>>();
        }
    }
    public class CreateDeliveryModeInputType : SheaftInputType<CreateDeliveryModeInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreateDeliveryModeInput> descriptor)
        {
            descriptor.Field(c => c.Description);
            descriptor.Field(c => c.Kind);
            descriptor.Field(c => c.Name);
            descriptor.Field(c => c.LockOrderHoursBeforeDelivery);

            descriptor.Field(c => c.Address)
                .Type<AddressInputType>();


            descriptor.Field(c => c.OpeningHours)
                .Type<NonNullType<ListType<TimeSlotGroupInputType>>>();
        }
    }
    public class UpdateDeliveryModeInputType : SheaftInputType<UpdateDeliveryModeInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateDeliveryModeInput> descriptor)
        {
            descriptor.Field(c => c.Description);
            descriptor.Field(c => c.Kind);
            descriptor.Field(c => c.LockOrderHoursBeforeDelivery);

            descriptor.Field(c => c.Id)
                .Type<NonNullType<IdType>>();

            descriptor.Field(c => c.Address)
                .Type<AddressInputType>();

            descriptor.Field(c => c.Name)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.OpeningHours)
                .Type<NonNullType<ListType<TimeSlotGroupInputType>>>();
        }
    }
    public class ExportPickingOrdersInputType : SheaftInputType<ExportPickingOrdersInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<ExportPickingOrdersInput> descriptor)
        {
            descriptor.Field(c => c.Name);
            descriptor.Field(c => c.ExpectedDeliveryDate);

            descriptor.Field(c => c.PurchaseOrderIds)
                .Type<NonNullType<ListType<IdType>>>();
        }
    }
    public class GenerateUserSponsoringCodeInputType : SheaftInputType<GenerateUserSponsoringCodeInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<GenerateUserSponsoringCodeInput> descriptor)
        {
            descriptor.Field(c => c.Id)
                .Type<NonNullType<IdType>>();
        }
    }
    public class IdInputType : SheaftInputType<IdInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<IdInput> descriptor)
        {
            descriptor.Field(c => c.Id)
                .Type<NonNullType<IdType>>();
        }
    }
    public class IdsInputType : SheaftInputType<IdsInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<IdsInput> descriptor)
        {
            descriptor.Field(c => c.Ids)
                .Type<NonNullType<ListType<IdType>>>();
        }
    }
    public class IdsWithReasonInputType : SheaftInputType<IdsWithReasonInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<IdsWithReasonInput> descriptor)
        {
            descriptor.Field(c => c.Reason);

            descriptor.Field(c => c.Ids)
                .Type<NonNullType<ListType<IdType>>>();
        }
    }
    public class IdTimeSlotGroupInputType : SheaftInputType<IdTimeSlotGroupInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<IdTimeSlotGroupInput> descriptor)
        {
            descriptor.Field(c => c.SelectedHours);

            descriptor.Field(c => c.Id)
                .Type<NonNullType<IdType>>();
        }
    }
    public class IdWithReasonInputType : SheaftInputType<IdWithReasonInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<IdWithReasonInput> descriptor)
        {
            descriptor.Field(c => c.Reason);

            descriptor.Field(c => c.Id)
                .Type<NonNullType<IdType>>();
        }
    }
    public class PackagingInputType : SheaftInputType<CreatePackagingInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreatePackagingInput> descriptor)
        {
            descriptor.Field(c => c.Vat);
            descriptor.Field(c => c.Description);
            descriptor.Field(c => c.WholeSalePrice);

            descriptor.Field(c => c.Name)
                .Type<NonNullType<StringType>>();
        }
    }
    public class ProducerExpectedDeliveryInputType : SheaftInputType<ProducerExpectedDeliveryInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<ProducerExpectedDeliveryInput> descriptor)
        {
            descriptor.Field(c => c.ProducerId).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.ExpectedDeliveryDate);
            descriptor.Field(c => c.Comment);
            descriptor.Field(c => c.DeliveryModeId).Type<NonNullType<IdType>>();
        }
    }
    public class SearchProducersDeliveriesInputType : SheaftInputType<SearchProducersDeliveriesInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<SearchProducersDeliveriesInput> descriptor)
        {
            descriptor.Field(c => c.Kinds)
                .Type<NonNullType<ListType<DeliveryKindEnumType>>>();

            descriptor.Field(c => c.Ids)
                .Type<NonNullType<ListType<IdType>>>();
        }
    }
    public class ProductInputType : SheaftInputType<CreateProductInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreateProductInput> descriptor)
        {
            descriptor.Field(c => c.Available);
            descriptor.Field(c => c.Description);
            descriptor.Field(c => c.PackagingId).Type<IdType>();
            descriptor.Field(c => c.Picture);
            descriptor.Field(c => c.QuantityPerUnit);
            descriptor.Field(c => c.Unit);
            descriptor.Field(c => c.Vat);
            descriptor.Field(c => c.Weight);
            descriptor.Field(c => c.WholeSalePricePerUnit);

            descriptor.Field(c => c.Name)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Reference)
                .Type<StringType>();

            descriptor.Field(c => c.Tags)
                .Type<NonNullType<ListType<IdType>>>();
        }
    }
    public class ProductQuantityInputType : SheaftInputType<ProductQuantityInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<ProductQuantityInput> descriptor)
        {
            descriptor.Field(c => c.Quantity);

            descriptor.Field(c => c.Id)
                .Type<NonNullType<IdType>>();
        }
    }
    public class RateProductInputType : SheaftInputType<RateProductInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<RateProductInput> descriptor)
        {
            descriptor.Field(c => c.Value);
            descriptor.Field(c => c.Comment);

            descriptor.Field(c => c.Id)
                .Type<NonNullType<IdType>>();
        }
    }
    public class RegisterStoreInputType : SheaftInputType<RegisterStoreInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<RegisterStoreInput> descriptor)
        {
            descriptor.Field(c => c.OpenForNewBusiness);
            descriptor.Field(c => c.Description);
            descriptor.Field(c => c.Phone);
            descriptor.Field(c => c.Picture);
            descriptor.Field(c => c.VatIdentifier);

            descriptor.Field(c => c.Siret)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Address)
                .Type<NonNullType<AddressInputType>>();

            descriptor.Field(c => c.Email)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Name)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.OpeningHours)
                .Type<ListType<TimeSlotGroupInputType>>();

            descriptor.Field(c => c.Tags)
                .Type<ListType<IdType>>();

            descriptor.Field(c => c.Owner)
                .Type<NonNullType<RegisterOwnerInputType>>();
        }
    }
    public class RegisterProducerInputType : SheaftInputType<RegisterProducerInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<RegisterProducerInput> descriptor)
        {
            descriptor.Field(c => c.OpenForNewBusiness);
            descriptor.Field(c => c.Description);
            descriptor.Field(c => c.Phone);
            descriptor.Field(c => c.Picture);
            descriptor.Field(c => c.VatIdentifier);

            descriptor.Field(c => c.Siret)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Address)
                .Type<NonNullType<AddressInputType>>();

            descriptor.Field(c => c.Email)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Name)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Tags)
                .Type<ListType<IdType>>();

            descriptor.Field(c => c.Owner)
                .Type<NonNullType<RegisterOwnerInputType>>();
        }
    }
    public class RegisterConsumerInputType : SheaftInputType<RegisterConsumerInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<RegisterConsumerInput> descriptor)
        {
            descriptor.Field(c => c.Anonymous);
            descriptor.Field(c => c.DepartmentId).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.Phone);
            descriptor.Field(c => c.Picture);
            descriptor.Field(c => c.SponsoringCode);

            descriptor.Field(c => c.Id)
                .Type<NonNullType<IdType>>();

            descriptor.Field(c => c.Email)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.FirstName)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.LastName)
                .Type<NonNullType<StringType>>();
        }
    }
    public class RegisterNewsletterInputType : SheaftInputType<RegisterNewsletterInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<RegisterNewsletterInput> descriptor)
        {
            descriptor.Field(c => c.Email)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.FirstName)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Role)
                .Type<NonNullType<StringType>>();
        }
    }
    public class RegisterOwnerInputType : SheaftInputType<RegisterOwnerInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<RegisterOwnerInput> descriptor)
        {
            descriptor.Field(c => c.Phone);
            descriptor.Field(c => c.Picture);
            descriptor.Field(c => c.SponsoringCode);

            descriptor.Field(c => c.Id)
                .Type<NonNullType<IdType>>();

            descriptor.Field(c => c.Email)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.FirstName)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.LastName)
                .Type<NonNullType<StringType>>();
        }
    }
    public class SearchTermsInputType : SheaftInputType<SearchTermsInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<SearchTermsInput> descriptor)
        {
            descriptor.Field(c => c.Latitude);
            descriptor.Field(c => c.Longitude);
            descriptor.Field(c => c.MaxDistance);
            descriptor.Field(c => c.Page);
            descriptor.Field(c => c.Sort);
            descriptor.Field(c => c.Tags);
            descriptor.Field(c => c.Take);
            descriptor.Field(c => c.Text);
        }
    }
    public class SetProductsAvailabilityInputType : SheaftInputType<SetProductsAvailabilityInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<SetProductsAvailabilityInput> descriptor)
        {
            descriptor.Field(c => c.Available);

            descriptor.Field(c => c.Ids)
                .Type<NonNullType<ListType<IdType>>>();
        }
    }
    public class TimeSlotGroupInputType : SheaftInputType<TimeSlotGroupInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<TimeSlotGroupInput> descriptor)
        {
            descriptor.Field(c => c.From);
            descriptor.Field(c => c.To);

            descriptor.Field(c => c.Days)
                .Type<NonNullType<ListType<DayOfWeekEnumType>>>();
        }
    }
    public class UpdateStoreInputType : SheaftInputType<UpdateStoreInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateStoreInput> descriptor)
        {
            descriptor.Field(c => c.OpenForNewBusiness);
            descriptor.Field(c => c.Description);
            descriptor.Field(c => c.OpeningHours);
            descriptor.Field(c => c.Phone);
            descriptor.Field(c => c.VatIdentifier);
            descriptor.Field(c => c.Picture);

            descriptor.Field(c => c.Siret)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Id)
                .Type<NonNullType<IdType>>();

            descriptor.Field(c => c.Address)
                .Type<NonNullType<AddressInputType>>();

            descriptor.Field(c => c.Email)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Name)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Tags)
                .Type<ListType<IdType>>();

            descriptor.Field(c => c.OpeningHours)
                .Type<ListType<TimeSlotGroupInputType>>();
        }
    }
    public class UpdateProducerInputType : SheaftInputType<UpdateProducerInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateProducerInput> descriptor)
        {
            descriptor.Field(c => c.OpenForNewBusiness);
            descriptor.Field(c => c.Description);
            descriptor.Field(c => c.Phone);
            descriptor.Field(c => c.VatIdentifier);
            descriptor.Field(c => c.Picture);

            descriptor.Field(c => c.Siret)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Id)
                .Type<NonNullType<IdType>>();

            descriptor.Field(c => c.Address)
                .Type<NonNullType<AddressInputType>>();

            descriptor.Field(c => c.Email)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Name)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Tags)
                .Type<ListType<IdType>>();
        }
    }
    public class UpdateIdProductsQuantitiesInputType : SheaftInputType<UpdateIdProductsQuantitiesInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateIdProductsQuantitiesInput> descriptor)
        {
            descriptor.Field(c => c.Id)
                .Type<NonNullType<IdType>>();

            descriptor.Field(c => c.Products)
                .Type<NonNullType<ListType<ProductQuantityInputType>>>();
        }
    }
    public class UpdatePackagingInputType : SheaftInputType<UpdatePackagingInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdatePackagingInput> descriptor)
        {
            descriptor.Field(c => c.Description);
            descriptor.Field(c => c.Vat);
            descriptor.Field(c => c.WholeSalePrice);

            descriptor.Field(c => c.Id)
                .Type<NonNullType<IdType>>();
            
            descriptor.Field(c => c.Name)
                .Type<NonNullType<StringType>>();
        }
    }
    public class UpdatePictureInputType : SheaftInputType<UpdatePictureInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdatePictureInput> descriptor)
        {
            descriptor.Field(c => c.Picture);

            descriptor.Field(c => c.Id)
                .Type<NonNullType<IdType>>();
        }
    }
    public class UpdateProductInputType : SheaftInputType<UpdateProductInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateProductInput> descriptor)
        {
            descriptor.Field(c => c.Available);
            descriptor.Field(c => c.Description);
            descriptor.Field(c => c.PackagingId).Type<IdType>();
            descriptor.Field(c => c.Picture);
            descriptor.Field(c => c.QuantityPerUnit);
            descriptor.Field(c => c.Unit);
            descriptor.Field(c => c.Vat);
            descriptor.Field(c => c.Weight);
            descriptor.Field(c => c.WholeSalePricePerUnit);

            descriptor.Field(c => c.Id)
                .Type<NonNullType<IdType>>();

            descriptor.Field(c => c.Name)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Reference)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Tags)
                .Type<NonNullType<ListType<IdType>>>();
        }
    }
    public class UpdateQuickOrderInputType : SheaftInputType<UpdateQuickOrderInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateQuickOrderInput> descriptor)
        {
            descriptor.Field(c => c.Description);

            descriptor.Field(c => c.Id)
                .Type<NonNullType<IdType>>();

            descriptor.Field(c => c.Name)
                .Type<NonNullType<StringType>>();
        }
    }
    public class UpdateConsumerInputType : SheaftInputType<UpdateConsumerInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateConsumerInput> descriptor)
        {
            descriptor.Field(c => c.Phone);
            descriptor.Field(c => c.Picture);

            descriptor.Field(c => c.Email)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.FirstName)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.LastName)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Id)
                .Type<NonNullType<IdType>>();
        }
    }
    public class AddressInputType : SheaftInputType<AddressInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<AddressInput> descriptor)
        {
            descriptor.Field(c => c.Line2);
            descriptor.Field(c => c.Latitude);
            descriptor.Field(c => c.Longitude);

            descriptor.Field(c => c.Line1)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Zipcode)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.City)
                .Type<NonNullType<StringType>>();
        }
    }
}
