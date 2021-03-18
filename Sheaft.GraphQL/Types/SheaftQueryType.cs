using HotChocolate.Types;
using HotChocolate.Types.Relay;
using Sheaft.Application.Security;
using Sheaft.GraphQL.Filters;
using Sheaft.GraphQL.Sorts;
using Sheaft.GraphQL.Types.Inputs;
using Sheaft.GraphQL.Types.Outputs;

namespace Sheaft.GraphQL.Types
{
    public class SheaftQueryType : ObjectType<SheaftQuery>
    {
        protected override void Configure(IObjectTypeDescriptor<SheaftQuery> descriptor)
        {
            //PROFILE
            descriptor.Field(c => c.GetMyUserProfile(default))
                .Name("me")
                .Authorize(Policies.AUTHENTICATED)
                .Type<UserType>()
                .UseSingleOrDefault()
                .UseSelection();

            descriptor.Field(c => c.GetFreshdeskTokenAsync(default))
                .Name("generateFreshdeskToken")
                .Authorize(Policies.AUTHENTICATED)
                .Type<NonNullType<StringType>>();

            //AGREEMENT
            descriptor.Field(c => c.GetAgreement(default, default))
                .Name("agreement")
                .Authorize(Policies.STORE_OR_PRODUCER)
                .Argument("input", c => c.Type<NonNullType<IdType>>())
                .Type<NonNullType<AgreementType>>()
                .UseSingleOrDefault()
                .UseSelection();

            descriptor.Field(c => c.GetAgreements(default))
                .Name("agreements")
                .Authorize(Policies.STORE_OR_PRODUCER)
                .Type<NonNullType<ListType<AgreementType>>>()
                .UsePaging<AgreementType>()
                .UseFiltering<AgreementFilterType>()
                .UseSorting<AgreementSortType>()
                .UseSelection();

            //LEADERBOARD
            descriptor.Field(c => c.GetCountryPoints(default, default))
                .Name("pointsPerCountry")
                .Argument("input", c => c.Type<IdType>())
                .Type<ListType<CountryPointsType>>()
                .UseSelection();

            descriptor.Field(c => c.GetCountryUsersPoints(default, default))
                .Name("userPointsPerCountry")
                .Argument("input", c => c.Type<IdType>())
                .Type<ListType<CountryUserPointsType>>()
                .UseSelection();

            descriptor.Field(c => c.GetDepartmentsPoints(default, default))
                .Name("pointsPerDepartment")
                .Argument("input", c => c.Type<IdType>())
                .Type<ListType<DepartmentPointsType>>()
                .UseSelection();

            descriptor.Field(c => c.GetDepartmentUsersPoints(default, default))
                .Name("userPointsPerDepartment")
                .Argument("input", c => c.Type<IdType>())
                .Type<ListType<DepartmentUserPointsType>>()
                .UseSelection();

            descriptor.Field(c => c.GetRegionsPoints(default, default))
                .Name("pointsPerRegion")
                .Argument("input", c => c.Type<IdType>())
                .Type<ListType<RegionPointsType>>()
                .UseSelection();

            descriptor.Field(c => c.GetRegionUsersPoints(default, default))
                .Name("userPointsPerRegion")
                .Argument("input", c => c.Type<IdType>())
                .Type<ListType<RegionUserPointsType>>()
                .UseSelection();

            descriptor.Field(c => c.GetMyPositionAsync(default))
                .Name("userPositionInCountry")
                .Authorize(Policies.CONSUMER)
                .Type<UserPositionType>()
                .UseSelection();

            descriptor.Field(c => c.GetMyPositionInDepartment(default))
                .Name("userPositionInDepartment")
                .Authorize(Policies.CONSUMER)
                .Type<UserPositionType>()
                .UseSelection();

            descriptor.Field(c => c.GetMyPositionInRegion(default))
                .Name("userPositionInRegion")
                .Authorize(Policies.CONSUMER)
                .Type<UserPositionType>()
                .UseSelection();

            descriptor.Field(c => c.GetMyRankInformationAsync(default))
                .Name("myRankInformation")
                .Authorize(Policies.CONSUMER)
                .Type<RankInformationType>()
                .UseSelection();

            //DELIVERY
            descriptor.Field(c => c.GetDeliveries(default))
                .Name("deliveries")
                .Authorize(Policies.PRODUCER)
                .Type<NonNullType<ListType<DeliveryModeType>>>()
                .UsePaging<DeliveryModeType>()
                .UseFiltering<DeliveryModeFilterType>()
                .UseSorting<DeliveryModeSortType>()
                .UseSelection();

            descriptor.Field(c => c.GetDelivery(default, default))
                .Name("delivery")
                .Authorize(Policies.PRODUCER)
                .Argument("input", c => c.Type<NonNullType<IdType>>())
                .Type<NonNullType<DeliveryModeType>>()
                .UseSingleOrDefault()
                .UseSelection();

            //LISTS
            descriptor.Field(c => c.GetDepartments(default))
                .Name("departments")
                .Type<NonNullType<ListType<DepartmentType>>>()
                .UseSelection();

            descriptor.Field(c => c.GetNationalities(default))
                .Name("nationalities")
                .Type<NonNullType<ListType<NationalityType>>>()
                .UseSelection();

            descriptor.Field(c => c.GetCountries(default))
                .Name("countries")
                .Type<NonNullType<ListType<CountryType>>>()
                .UseSelection();

            descriptor.Field(c => c.GetRegions(default))
                .Name("regions")
                .Type<NonNullType<ListType<RegionType>>>()
                .UseSelection();

            descriptor.Field(c => c.GetTags(default))
                .Name("tags")
                .Type<NonNullType<ListType<TagType>>>()
                .UseSorting<TagSortType>()
                .UseFiltering<TagFilterType>()
                .UseSelection();

            //ORDER
            descriptor.Field(c => c.GetCurrentOrder(default))
                .Name("currentOrder")
                .Authorize(Policies.REGISTERED)
                .Type<OrderType>()
                .UseFirstOrDefault();
            
            descriptor.Field(c => c.GetOrder(default, default))
                .Name("order")
                .Argument("input", c => c.Type<NonNullType<IdType>>())
                .Type<OrderType>()
                .UseSingleOrDefault();

            descriptor.Field(c => c.GetOrders(default))
                .Name("orders")
                .Authorize(Policies.REGISTERED)
                .Type<NonNullType<ListType<OrderType>>>()
                .UsePaging<OrderType>()
                .UseFiltering<OrderFilterType>()
                .UseSorting<OrderSortType>()
                .UseSelection();

            //JOB
            descriptor.Field(c => c.GetJob(default, default))
                .Name("job")
                .Authorize(Policies.REGISTERED)
                .Argument("input", c => c.Type<NonNullType<IdType>>())
                .Type<NonNullType<JobType>>()
                .UseSingleOrDefault()
                .UseSelection();

            descriptor.Field(c => c.GetJobs(default))
                .Name("jobs")
                .Authorize(Policies.REGISTERED)
                .Type<NonNullType<ListType<JobType>>>()
                .UsePaging<JobType>()
                .UseFiltering<JobFilterType>()
                .UseSorting<JobSortType>()
                .UseSelection();

            //NOTIFICATION
            descriptor.Field(c => c.GetNotifications(default))
                .Name("notifications")
                .Authorize(Policies.REGISTERED)
                .Type<NonNullType<ListType<NotificationType>>>()
                .UsePaging<NotificationType>()
                .UseFiltering<NotificationFilterType>()
                .UseSorting<NotificationSortType>()
                .UseSelection();

            descriptor.Field(c => c.GetUnreadNotificationsCount(default))
                .Name("unreadNotificationsCount")
                .Authorize(Policies.REGISTERED)
                .Type<NonNullType<IntType>>();

            //RETURNABLE
            descriptor.Field(c => c.GetReturnable(default, default))
                .Name("returnable")
                .Authorize(Policies.PRODUCER)
                .Argument("input", c => c.Type<NonNullType<IdType>>())
                .Type<NonNullType<ReturnableType>>()
                .UseSingleOrDefault()
                .UseSelection();

            descriptor.Field(c => c.GetReturnables(default))
                .Name("returnables")
                .Authorize(Policies.PRODUCER)
                .Type<NonNullType<ListType<ReturnableType>>>()
                .UsePaging<ReturnableType>()
                .UseFiltering<ReturnableFilterType>()
                .UseSorting<ReturnableSortType>()
                .UseSelection();

            //PRODUCER
            descriptor.Field(c => c.GetProducer(default, default))
                .Name("producer")
                .Argument("input", c => c.Type<NonNullType<IdType>>())
                .Type<NonNullType<ProducerType>>()
                .UseSingleOrDefault()
                .UseSelection();

            descriptor.Field(c => c.GetProducers(default))
                .Name("producers")
                .Type<NonNullType<ListType<ProducerType>>>()
                .UsePaging<ProducerType>()
                .UseSelection();

            descriptor.Field(c => c.GetProducerProducts(default, default))
                .Name("producerProducts")
                .Argument("input", c => c.Type<NonNullType<IdType>>())
                .Type<NonNullType<ListType<ProductType>>>()
                .UsePaging<ProductType>()
                .UseFiltering<ProductFilterType>()
                .UseSorting<ProductSortType>()
                .UseSelection();

            descriptor.Field(c => c.GetProducerAgreements(default, default))
                .Name("producerAgreements")
                .Authorize(Policies.STORE)
                .Argument("input", c => c.Type<NonNullType<IdType>>())
                .Type<NonNullType<ListType<AgreementType>>>()
                .UsePaging<AgreementType>()
                .UseFiltering<AgreementFilterType>()
                .UseSorting<AgreementSortType>()
                .UseSelection();

            descriptor.Field(c => c.GetProducersDeliveriesAsync(default, default))
                .Name("getDeliveriesForProducers")
                .Argument("input", c => c.Type<SearchProducersDeliveriesInputType>())
                .Type<NonNullType<ListType<ProducerDeliveriesType>>>()
                .UseSorting<ProducerDeliveriesSortType>()
                .UseSelection();

            //PRODUCT
            descriptor.Field(c => c.GetProduct(default, default))
                .Name("product")
                .Argument("input", c => c.Type<NonNullType<IdType>>())
                .Type<NonNullType<ProductType>>()
                .UseSingleOrDefault()
                .UseSelection();

            descriptor.Field(c => c.GetProducts(default))
                .Name("products")
                .Authorize(Policies.PRODUCER)
                .Type<NonNullType<ListType<ProductType>>>()
                .UsePaging<ProductType>()
                .UseFiltering<ProductFilterType>()
                .UseSorting<ProductSortType>()
                .UseSelection();

            descriptor.Field(c => c.HasProductsImportsInProgressAsync(default))
                .Name("productsImportInProgress")
                .Authorize(Policies.PRODUCER)
                .Type<NonNullType<BooleanType>>();

            //PURCHASE ORDER
            descriptor.Field(c => c.GetMyPurchaseOrders(default))
                .Name("myOrders")
                .Authorize(Policies.REGISTERED)
                .Type<NonNullType<ListType<PurchaseOrderType>>>()
                .UsePaging<PurchaseOrderType>()
                .UseFiltering<PurchaseOrderFilterType>()
                .UseSorting<PurchaseOrderSortType>()
                .UseSelection();

            descriptor.Field(c => c.GetPurchaseOrder(default, default))
                .Name("purchaseOrder")
                .Authorize(Policies.REGISTERED)
                .Argument("input", c => c.Type<NonNullType<IdType>>())
                .Type<NonNullType<PurchaseOrderType>>()
                .UseSingleOrDefault()
                .UseSelection();

            descriptor.Field(c => c.GetPurchaseOrders(default))
                .Name("purchaseOrders")
                .Authorize(Policies.PRODUCER)
                .Type<NonNullType<ListType<PurchaseOrderType>>>()
                .UsePaging<PurchaseOrderType>()
                .UseFiltering<PurchaseOrderFilterType>()
                .UseSorting<PurchaseOrderSortType>()
                .UseSelection();

            descriptor.Field(c => c.HasPickingOrdersExportsInProgressAsync(default))
                .Name("pickingOrdersExportInProgress")
                .Authorize(Policies.PRODUCER)
                .Type<NonNullType<BooleanType>>();

            //QUICKORDER
            descriptor.Field(c => c.GetQuickOrder(default, default))
                .Name("quickOrder")
                .Authorize(Policies.STORE)
                .Argument("input", c => c.Type<NonNullType<IdType>>())
                .Type<NonNullType<QuickOrderType>>()
                .UseSingleOrDefault()
                .UseSelection();

            descriptor.Field(c => c.GetQuickOrders(default))
                .Name("quickOrders")
                .Authorize(Policies.STORE)
                .Type<NonNullType<ListType<QuickOrderType>>>()
                .UsePaging<QuickOrderType>()
                .UseFiltering<QuickOrderFilterType>()
                .UseSorting<QuickOrderSortType>()
                .UseSelection();

            descriptor.Field(c => c.GetMyDefaultQuickOrder(default))
                .Name("defaultQuickOrder")
                .Authorize(Policies.STORE)
                .Type<NonNullType<QuickOrderType>>()
                .UseSingleOrDefault()
                .UseSelection();

            //DOCUMENT
            descriptor.Field(c => c.GetDocuments(default))
                .Name("documents")
                .Authorize(Policies.PRODUCER)
                .Type<NonNullType<ListType<DocumentType>>>()
                .UsePaging<DocumentType>()
                .UseSelection();

            //CONSUMER
            descriptor.Field(c => c.GetConsumer(default, default))
                .Name("consumer")
                .Authorize(Policies.CONSUMER)
                .Argument("input", c => c.Type<NonNullType<IdType>>())
                .Type<NonNullType<ConsumerType>>()
                .UseSingleOrDefault()
                .UseSelection();

            //STORE
            descriptor.Field(c => c.GetStore(default, default))
                .Name("store")
                .Authorize(Policies.OWNER)
                .Argument("input", c => c.Type<NonNullType<IdType>>())
                .Type<NonNullType<StoreType>>()
                .UseSingleOrDefault()
                .UseSelection();

            descriptor.Field(c => c.GetStoreProducts(default))
                .Name("storeAgreementsProducts")
                .Authorize(Policies.STORE)
                .Type<NonNullType<ListType<ProductType>>>()
                .UsePaging<ProductType>()
                .UseFiltering<ProductFilterType>()
                .UseSorting<ProductSortType>()
                .UseSelection();

            descriptor.Field(c => c.GetStoreAgreements(default, default))
                .Name("storeAgreements")
                .Authorize(Policies.PRODUCER)
                .Argument("input", c => c.Type<NonNullType<IdType>>())
                .Type<NonNullType<ListType<AgreementType>>>()
                .UsePaging<AgreementType>()
                .UseFiltering<AgreementFilterType>()
                .UseSorting<AgreementSortType>()
                .UseSelection();

            descriptor.Field(c => c.GetStoreDeliveriesForProducersAsync(default, default))
                .Name("getStoreDeliveriesForProducers")
                .Authorize(Policies.STORE)
                .Argument("input", c => c.Type<SearchProducersDeliveriesInputType>())
                .Type<NonNullType<ListType<ProducerDeliveriesType>>>()
                .UseSorting<ProducerDeliveriesSortType>()
                .UseSelection();

            //LEGAL
            descriptor.Field(c => c.GetConsumerLegals(default))
                .Name("getMyConsumerLegals")
                .Authorize(Policies.CONSUMER)
                .Type<ConsumerLegalType>()
                .UseSingleOrDefault()
                .UseSelection();

            descriptor.Field(c => c.GetBusinessLegals(default))
                .Name("getMyBusinessLegals")
                .Authorize(Policies.STORE_OR_PRODUCER)
                .Type<BusinessLegalType>()
                .UseSingleOrDefault()
                .UseSelection();

            //WEBPAYIN
            descriptor.Field(c => c.GetWebPayinTransaction(default, default))
                .Name("webPayinTransaction")
                .Authorize(Policies.REGISTERED)
                .Argument("input", c => c.Type<NonNullType<StringType>>())
                .Type<NonNullType<WebPayinType>>()
                .UseSingleOrDefault()
                .UseSelection();
            
            //PAYOUTS
            descriptor.Field(c => c.GetPayout(default, default))
                .Name("payout")
                .Authorize(Policies.PRODUCER)
                .Type<NonNullType<PayoutType>>()
                .UseSingleOrDefault()
                .UseSelection();
            
            descriptor.Field(c => c.GetPayouts(default))
                .Name("payouts")
                .Authorize(Policies.PRODUCER)
                .Type<ListType<PayoutType>>()
                .UsePaging<PayoutType>()
                .UseSorting<PayoutSortType>()
                .UseFiltering<PayoutFilterType>()
                .UseSelection();
            
            //DONATIONS
            descriptor.Field(c => c.GetDonation(default, default))
                .Name("donation")
                .Authorize(Policies.CONSUMER)
                .Type<NonNullType<DonationType>>()
                .UseSingleOrDefault()
                .UseSelection();
            
            descriptor.Field(c => c.GetDonations(default))
                .Name("donations")
                .Authorize(Policies.CONSUMER)
                .Type<ListType<DonationType>>()
                .UsePaging<DonationType>()
                .UseSorting<DonationSortType>()
                .UseFiltering<DonationFilterType>()
                .UseSelection();
            
            //WITHHOLDINGS
            descriptor.Field(c => c.GetWithholding(default, default))
                .Name("withholding")
                .Authorize(Policies.PRODUCER)
                .Type<NonNullType<WithholdingType>>()
                .UseSingleOrDefault()
                .UseSelection();
            
            descriptor.Field(c => c.GetWithholdings(default))
                .Name("withholdings")
                .Authorize(Policies.PRODUCER)
                .Type<ListType<WithholdingType>>()
                .UsePaging<WithholdingType>()
                .UseSorting<WithholdingSortType>()
                .UseFiltering<WithholdingFilterType>()
                .UseSelection();

            //SEARCH
            descriptor.Field(c => c.SearchBusinessWithSiretAsync(default, default))
                .Name("searchBusinessWithSiret")
                .Authorize(Policies.UNREGISTERED)
                .Argument("input", c => c.Type<NonNullType<StringType>>())
                .Type<SirenBusinessType>();

            descriptor.Field(c => c.SuggestProducersAsync(default, default))
                .Name("suggestProducers")
                .Argument("input", c => c.Type<NonNullType<SearchTermsInputType>>())
                .Type<ListType<SuggestProducerType>>();

            descriptor.Field(c => c.SearchProducersAsync(default, default))
                .Name("searchProducers")
                .Authorize(Policies.STORE)
                .Argument("input", c => c.Type<NonNullType<SearchTermsInputType>>())
                .Type<NonNullType<ProducersSearchType>>();

            descriptor.Field(c => c.SearchProductsAsync(default, default))
                .Name("searchProducts")
                .Argument("input", c => c.Type<NonNullType<SearchProductsInputType>>())
                .Type<NonNullType<ProductsSearchType>>();

            descriptor.Field(c => c.SearchStoresAsync(default, default))
                .Name("searchStores")
                .Authorize(Policies.PRODUCER)
                .Argument("input", c => c.Type<NonNullType<SearchTermsInputType>>())
                .Type<NonNullType<StoresSearchType>>();
        }
    }
}
