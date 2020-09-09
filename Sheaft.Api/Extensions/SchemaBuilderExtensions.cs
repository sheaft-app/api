using HotChocolate;
using Sheaft.GraphQL.Enums;
using Sheaft.GraphQL.Types;

namespace Sheaft.Api
{
    public static class SchemaBuilderExtensions
    {
        public static ISchemaBuilder RegisterTypes(this ISchemaBuilder services)
        {
            services.AddType<AddressType>();
            services.AddType<AgreementType>();
            services.AddType<DeliveryModeType>();
            services.AddType<TimeSlotType>();
            services.AddType<UserType>();
            services.AddType<StoreType>();
            services.AddType<ProducerType>();
            services.AddType<DepartmentType>();
            services.AddType<RegionType>();
            services.AddType<LevelType>();
            services.AddType<RewardType>();
            services.AddType<ConsumerType>();
            services.AddType<UserProfileType>();
            services.AddType<ExpectedDeliveryType>();
            services.AddType<RankInformationType>();
            services.AddType<SirenCompanyType>();
            services.AddType<SirenAddressType>();
            services.AddType<SirenLegalsType>();
            services.AddType<JobType>();
            services.AddType<NotificationType>();
            services.AddType<PackagingType>();
            services.AddType<PurchaseOrderType>();
            services.AddType<RatingType>();
            services.AddType<PurchaseOrderProductQuantityType>();
            services.AddType<QuickOrderType>();
            services.AddType<TagType>();
            services.AddType<UserPositionType>();
            services.AddType<ProducerDeliveriesType>();
            services.AddType<CountryPointsType>();
            services.AddType<RegionPointsType>();
            services.AddType<DepartmentPointsType>();
            services.AddType<CountryUserPointsType>();
            services.AddType<RegionUserPointsType>();
            services.AddType<DepartmentUserPointsType>();
            services.AddType<SearchStoreType>();
            services.AddType<SearchProducerType>();
            services.AddType<ProductType>();

            services.AddType<AddressKindEnumType>();
            services.AddType<AgreementStatusEnumType>();
            services.AddType<OrderStatusEnumType>();
            services.AddType<JobKindEnumType>();
            services.AddType<OwnerKindEnumType>();
            services.AddType<TagKindEnumType>();
            services.AddType<ProcessStatusEnumType>();
            services.AddType<NotificationKindEnumType>();
            services.AddType<PointKindEnumType>();
            services.AddType<DeliveryKindEnumType>();
            services.AddType<DayOfWeekEnumType>();
            services.AddType<ProfileKindEnumType>();
            services.AddType<UnitKindEnumType>();
            services.AddType<ProfileKindEnumType>();
            services.AddType<PaymentKindEnumType>();
            services.AddType<WalletKindEnumType>();

            services.AddType<CreateAgreementInputType>();
            services.AddType<CreatePurchaseOrderInputType>();
            services.AddType<CreatePurchaseOrdersInputType>();
            services.AddType<CreateQuickOrderInputType>();
            services.AddType<CreateDeliveryModeInputType>();
            services.AddType<ExportPickingOrdersInputType>();
            services.AddType<GenerateUserSponsoringCodeInputType>();
            services.AddType<IdInputType>();
            services.AddType<IdsInputType>();
            services.AddType<IdsWithReasonInputType>();
            services.AddType<IdTimeSlotGroupInputType>();
            services.AddType<IdWithReasonInputType>();
            services.AddType<PackagingInputType>();
            services.AddType<ProducerExpectedDeliveryInputType>();
            services.AddType<SearchProducersDeliveriesInputType>();
            services.AddType<ProductInputType>();
            services.AddType<ProductQuantityInputType>();
            services.AddType<RateProductInputType>();
            services.AddType<RegisterProducerInputType>();
            services.AddType<RegisterStoreInputType>();
            services.AddType<RegisterConsumerInputType>();
            services.AddType<RegisterNewsletterInputType>();
            services.AddType<RegisterOwnerInputType>();
            services.AddType<SearchTermsInputType>();
            services.AddType<SetProductsAvailabilityInputType>();
            services.AddType<TimeSlotGroupInputType>();
            services.AddType<UpdateStoreInputType>();
            services.AddType<UpdateProducerInputType>();
            services.AddType<UpdateDeliveryModeInputType>();
            services.AddType<UpdateIdProductsQuantitiesInputType>();
            services.AddType<UpdatePackagingInputType>();
            services.AddType<UpdatePictureInputType>();
            services.AddType<UpdateProductInputType>();
            services.AddType<UpdateQuickOrderInputType>();
            services.AddType<UpdateConsumerInputType>();
            services.AddType<AddressInputType>();

            return services;
        }
    }
}
