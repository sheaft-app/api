using HotChocolate.Types;
using HotChocolate.Types.Relay;
using Sheaft.Application.Security;
using Sheaft.GraphQL.Filters;
using Sheaft.GraphQL.Sorts;
using Sheaft.GraphQL.Types.Outputs;

namespace Sheaft.GraphQL.Types
{
    public class SheaftMutationType : ObjectType<SheaftMutation>
    {
        protected override void Configure(IObjectTypeDescriptor<SheaftMutation> descriptor)
        {
            descriptor.Field(c => c.GenerateUserSponsoringCodeAsync(default))
                .Name("generateUserSponsoringCode")
                .Authorize(Policies.REGISTERED)
                .Type<NonNullType<StringType>>();

            //PURCHASE ORDERS
            descriptor.Field(c => c.AcceptPurchaseOrdersAsync(default, default))
                .Name("acceptPurchaseOrders")
                .Authorize(Policies.PRODUCER)
                .Type<NonNullType<ListType<PurchaseOrderType>>>()
                .UsePaging<PurchaseOrderType>()
                .UseFiltering<PurchaseOrderFilterType>()
                .UseSorting<PurchaseOrderSortType>()
                .UseSelection();

            descriptor.Field(c => c.CompletePurchaseOrdersAsync(default, default))
                .Name("completePurchaseOrders")
                .Authorize(Policies.PRODUCER)
                .Type<NonNullType<ListType<PurchaseOrderType>>>()
                .UsePaging<PurchaseOrderType>()
                .UseFiltering<PurchaseOrderFilterType>()
                .UseSorting<PurchaseOrderSortType>()
                .UseSelection();

            descriptor.Field(c => c.DeliverPurchaseOrdersAsync(default, default))
                .Name("deliverPurchaseOrders")
                .Authorize(Policies.PRODUCER)
                .Type<NonNullType<ListType<PurchaseOrderType>>>()
                .UsePaging<PurchaseOrderType>()
                .UseFiltering<PurchaseOrderFilterType>()
                .UseSorting<PurchaseOrderSortType>()
                .UseSelection();

            descriptor.Field(c => c.ProcessPurchaseOrdersAsync(default, default))
                .Name("processPurchaseOrders")
                .Authorize(Policies.PRODUCER)
                .Type<NonNullType<ListType<PurchaseOrderType>>>()
                .UsePaging<PurchaseOrderType>()
                .UseFiltering<PurchaseOrderFilterType>()
                .UseSorting<PurchaseOrderSortType>()
                .UseSelection();

            descriptor.Field(c => c.ShipPurchaseOrdersAsync(default, default))
                .Name("shipPurchaseOrders")
                .Authorize(Policies.PRODUCER)
                .Type<NonNullType<ListType<PurchaseOrderType>>>()
                .UsePaging<PurchaseOrderType>()
                .UseFiltering<PurchaseOrderFilterType>()
                .UseSorting<PurchaseOrderSortType>()
                .UseSelection();

            descriptor.Field(c => c.RefusePurchaseOrdersAsync(default, default))
                .Name("refusePurchaseOrders")
                .Authorize(Policies.PRODUCER)
                .Type<NonNullType<ListType<PurchaseOrderType>>>()
                .UsePaging<PurchaseOrderType>()
                .UseFiltering<PurchaseOrderFilterType>()
                .UseSorting<PurchaseOrderSortType>()
                .UseSelection();

            descriptor.Field(c => c.CancelPurchaseOrdersAsync(default, default))
                .Name("cancelPurchaseOrders")
                .Authorize(Policies.REGISTERED)
                .Type<NonNullType<ListType<PurchaseOrderType>>>()
                .UsePaging<PurchaseOrderType>()
                .UseFiltering<PurchaseOrderFilterType>()
                .UseSorting<PurchaseOrderSortType>()
                .UseSelection();

            descriptor.Field(c => c.DeletePurchaseOrdersAsync(default))
                .Name("deletePurchaseOrders")
                .Type<NonNullType<BooleanType>>()
                .Authorize(Policies.REGISTERED);

            //AGREEMENTS
            descriptor.Field(c => c.CreateAgreementAsync(default, default))
                .Name("createAgreement")
                .Authorize(Policies.STORE_OR_PRODUCER)
                .Type<NonNullType<AgreementType>>()
                .UseSingleOrDefault()
                .UseSelection();

            descriptor.Field(c => c.AcceptAgreementAsync(default, default))
                .Name("acceptAgreement")
                .Authorize(Policies.STORE_OR_PRODUCER)
                .Type<NonNullType<AgreementType>>()
                .UseSingleOrDefault()
                .UseSelection();
            
            descriptor.Field(c => c.AssignCatalogToAgreementAsync(default, default))
                .Name("assignCatalogToAgreement")
                .Authorize(Policies.PRODUCER)
                .Type<NonNullType<AgreementType>>()
                .UseSingleOrDefault()
                .UseSelection();

            descriptor.Field(c => c.RefuseAgreementsAsync(default, default))
                .Name("refuseAgreements")
                .Authorize(Policies.STORE_OR_PRODUCER)
                .Type<NonNullType<ListType<AgreementType>>>()
                .UsePaging<AgreementType>()
                .UseFiltering<AgreementFilterType>()
                .UseSorting<AgreementSortType>()
                .UseSelection();

            descriptor.Field(c => c.CancelAgreementsAsync(default, default))
                .Name("cancelAgreements")
                .Authorize(Policies.STORE_OR_PRODUCER)
                .Type<NonNullType<ListType<AgreementType>>>()
                .UsePaging<AgreementType>()
                .UseFiltering<AgreementFilterType>()
                .UseSorting<AgreementSortType>()
                .UseSelection();

            //JOBS
            descriptor.Field(c => c.PauseJobsAsync(default, default))
                .Name("pauseJobs")
                .Authorize(Policies.REGISTERED)
                .Type<NonNullType<ListType<JobType>>>()
                .UsePaging<JobType>()
                .UseFiltering<JobFilterType>()
                .UseSorting<JobSortType>()
                .UseSelection();

            descriptor.Field(c => c.ResumeJobsAsync(default, default))
                .Name("resumeJobs")
                .Authorize(Policies.REGISTERED)
                .Type<NonNullType<ListType<JobType>>>()
                .UsePaging<JobType>()
                .UseFiltering<JobFilterType>()
                .UseSorting<JobSortType>()
                .UseSelection();

            descriptor.Field(c => c.RetryJobsAsync(default, default))
                .Name("retryJobs")
                .Authorize(Policies.REGISTERED)
                .Type<NonNullType<ListType<JobType>>>()
                .UsePaging<JobType>()
                .UseFiltering<JobFilterType>()
                .UseSorting<JobSortType>()
                .UseSelection();

            descriptor.Field(c => c.CancelJobsAsync(default, default))
                .Name("cancelJobs")
                .Authorize(Policies.REGISTERED)
                .Type<NonNullType<ListType<JobType>>>()
                .UsePaging<JobType>()
                .UseFiltering<JobFilterType>()
                .UseSorting<JobSortType>()
                .UseSelection();

            descriptor.Field(c => c.ArchiveJobsAsync(default, default))
                .Name("archiveJobs")
                .Authorize(Policies.REGISTERED)
                .Type<NonNullType<ListType<JobType>>>()
                .UsePaging<JobType>()
                .UseFiltering<JobFilterType>()
                .UseSorting<JobSortType>()
                .UseSelection();

            //PRODUCTS
            descriptor.Field(c => c.CreateProductAsync(default, default))
                .Name("createProduct")
                .Authorize(Policies.PRODUCER)
                .Type<NonNullType<ProductType>>()
                .UseSingleOrDefault()
                .UseSelection();

            descriptor.Field(c => c.UpdateProductAsync(default, default))
                .Name("updateProduct")
                .Authorize(Policies.PRODUCER)
                .Type<NonNullType<ProductType>>()
                .UseSingleOrDefault()
                .UseSelection();

            descriptor.Field(c => c.UpdateProductPictureAsync(default, default))
                .Name("updateProductPicture")
                .Authorize(Policies.PRODUCER)
                .Type<NonNullType<ProductType>>()
                .UseSingleOrDefault()
                .UseSelection();

            descriptor.Field(c => c.SetProductsAvailabilityAsync(default, default))
                .Name("setProductsAvailability")
                .Authorize(Policies.PRODUCER)
                .Type<NonNullType<ListType<ProductType>>>()
                .UsePaging<ProductType>()
                .UseFiltering<ProductFilterType>()
                .UseSorting<SearchProductSortType>()
                .UseSelection();

            descriptor.Field(c => c.DeleteProductsAsync(default))
                .Name("deleteProducts")
                .Authorize(Policies.PRODUCER)
                .Type<NonNullType<BooleanType>>();

            descriptor.Field(c => c.RateProductAsync(default, default))
                .Name("rateProduct")
                .Authorize(Policies.REGISTERED)
                .Type<NonNullType<ProductType>>()
                .UseSingleOrDefault()
                .UseSelection();

            descriptor.Field(c => c.AddPictureToProductAsync(default))
                .Name("addPictureToProduct")
                .Authorize(Policies.PRODUCER)
                .Type<NonNullType<BooleanType>>();
            
            descriptor.Field(c => c.RemoveProductPicturesAsync(default))
                .Name("removeProductPictures")
                .Authorize(Policies.PRODUCER)
                .Type<NonNullType<BooleanType>>();

            //DELIVERY
            descriptor.Field(c => c.CreateDeliveryModeAsync(default, default))
                .Name("createDeliveryMode")
                .Authorize(Policies.PRODUCER)
                .Type<NonNullType<DeliveryModeType>>()
                .UseSingleOrDefault()
                .UseSelection();

            descriptor.Field(c => c.UpdateDeliveryModeAsync(default, default))
                .Name("updateDeliveryMode")
                .Authorize(Policies.PRODUCER)
                .Type<NonNullType<DeliveryModeType>>()
                .UseSingleOrDefault()
                .UseSelection();

            descriptor.Field(c => c.DeleteDeliveryModeAsync(default))
                .Name("deleteDeliveryMode")
                .Authorize(Policies.PRODUCER)
                .Type<NonNullType<BooleanType>>();

            descriptor.Field(c => c.SetDeliveryModesAvailabilityAsync(default, default))
                .Name("setDeliveryModesAvailability")
                .Authorize(Policies.PRODUCER)
                .Type<NonNullType<ListType<DeliveryModeType>>>()
                .UsePaging<DeliveryModeType>()
                .UseFiltering<DeliveryModeFilterType>()
                .UseSorting<DeliveryModeSortType>()
                .UseSelection();

            //RETURNABLE
            descriptor.Field(c => c.CreateReturnableAsync(default, default))
                .Name("createReturnable")
                .Authorize(Policies.PRODUCER)
                .Type<NonNullType<ReturnableType>>()
                .UseSingleOrDefault()
                .UseSelection();

            descriptor.Field(c => c.UpdateReturnableAsync(default, default))
                .Name("updateReturnable")
                .Authorize(Policies.PRODUCER)
                .Type<NonNullType<ReturnableType>>()
                .UseSingleOrDefault()
                .UseSelection();

            descriptor.Field(c => c.DeleteReturnableAsync(default))
                .Name("deleteReturnable")
                .Authorize(Policies.PRODUCER)
                .Type<NonNullType<BooleanType>>();

            //ORDER
            descriptor.Field(c => c.CreateOrderAsync(default, default))
                .Name("createOrder")
                .Authorize(Policies.ANONYMOUS_OR_CONNECTED)
                .Type<NonNullType<OrderType>>()
                .UseSingleOrDefault();

            descriptor.Field(c => c.UpdateOrderAsync(default, default))
                .Name("updateOrder")
                .Authorize(Policies.ANONYMOUS_OR_CONNECTED)
                .Type<NonNullType<OrderType>>()
                .UseSingleOrDefault();

            descriptor.Field(c => c.CreateWebPayinForOrderAsync(default, default))
                .Name("payOrder")
                .Authorize(Policies.CONSUMER)
                .Type<NonNullType<WebPayinType>>()
                .UseSingleOrDefault()
                .UseSelection();
            
            descriptor.Field(c => c.ResetOrderAsync(default, default))
                .Name("resetOrder")
                .Authorize(Policies.CONSUMER)
                .Type<NonNullType<OrderType>>()
                .UseSingleOrDefault()
                .UseSelection();

            descriptor.Field(c => c.CreateBusinessOrderAsync(default, default))
                .Name("createPurchaseOrders")
                .Authorize(Policies.STORE)
                .Type<NonNullType<ListType<PurchaseOrderType>>>()
                .UsePaging<PurchaseOrderType>()
                .UseFiltering<PurchaseOrderFilterType>()
                .UseSorting<PurchaseOrderSortType>()
                .UseSelection();

            //QUICKORDER
            descriptor.Field(c => c.CreateQuickOrderAsync(default, default))
                .Name("createQuickOrder")
                .Authorize(Policies.STORE)
                .Type<NonNullType<QuickOrderType>>()
                .UseSingleOrDefault()
                .UseSelection();

            descriptor.Field(c => c.SetDefaultQuickOrderAsync(default, default))
                .Name("setDefaultQuickOrder")
                .Authorize(Policies.STORE)
                .Type<NonNullType<QuickOrderType>>()
                .UseSingleOrDefault()
                .UseSelection();

            descriptor.Field(c => c.UpdateQuickOrderAsync(default, default))
                .Name("updateQuickOrder")
                .Authorize(Policies.STORE)
                .Type<NonNullType<QuickOrderType>>()
                .UseSingleOrDefault()
                .UseSelection();

            descriptor.Field(c => c.DeleteQuickOrdersAsync(default))
                .Name("deleteQuickOrders")
                .Authorize(Policies.STORE)
                .Type<NonNullType<BooleanType>>();

            //USER
            descriptor.Field(c => c.UpdateUserPictureAsync(default, default))
                .Name("updateUserPicture")
                .Authorize(Policies.REGISTERED)
                .Type<NonNullType<UserType>>()
                .UseSingleOrDefault()
                .UseSelection();
            
            descriptor.Field(c => c.AddPictureToUserProfileAsync(default))
                .Name("addPictureToProfile")
                .Authorize(Policies.REGISTERED)
                .Type<NonNullType<BooleanType>>();
            
            descriptor.Field(c => c.RemoveUserProfilePicturesAsync(default))
                .Name("removeProfilePictures")
                .Authorize(Policies.REGISTERED)
                .Type<NonNullType<BooleanType>>();

            descriptor.Field(c => c.RemoveUserAsync(default))
                .Name("removeUser")
                .Authorize(Policies.REGISTERED)
                .Type<NonNullType<BooleanType>>();

            //EXPORT
            descriptor.Field(c => c.ExportUserDataAsync(default, default))
                .Name("exportRGPD")
                .Authorize(Policies.REGISTERED)
                .Type<NonNullType<JobType>>()
                .UseSingleOrDefault()
                .UseSelection();

            descriptor.Field(c => c.ExportPickingOrdersAsync(default, default))
                .Name("exportPickingFromOrders")
                .Authorize(Policies.PRODUCER)
                .Type<NonNullType<JobType>>()
                .UseSingleOrDefault()
                .UseSelection();

            descriptor.Field(c => c.ExportPurchaseOrdersAsync(default, default))
                .Name("exportPurchaseOrders")
                .Authorize(Policies.PRODUCER)
                .Type<NonNullType<JobType>>()
                .UseSingleOrDefault()
                .UseSelection();

            descriptor.Field(c => c.ExportTransactionsAsync(default, default))
                .Name("exportTransactions")
                .Authorize(Policies.PRODUCER)
                .Type<NonNullType<JobType>>()
                .UseSingleOrDefault()
                .UseSelection();

            //NOTIFICATIONS
            descriptor.Field(c => c.MarkMyNotificationsAsReadAsync())
                .Name("markUserNotificationsAsRead")
                .Authorize(Policies.REGISTERED)
                .Type<NonNullType<DateTimeType>>();

            descriptor.Field(c => c.MarkNotificationAsReadAsync(default, default))
                .Name("markUserNotificationAsRead")
                .Authorize(Policies.REGISTERED)
                .Type<NonNullType<NotificationType>>()
                .UseSingleOrDefault()
                .UseSelection();

            //PRODUCER
            descriptor.Field(c => c.RegisterProducerAsync(default, default))
                .Name("registerProducer")
                .Authorize(Policies.AUTHENTICATED)
                .Type<NonNullType<ProducerType>>()
                .UseSingleOrDefault()
                .UseSelection();

            descriptor.Field(c => c.UpdateProducerAsync(default, default))
                .Name("updateProducer")
                .Authorize(Policies.PRODUCER)
                .Type<NonNullType<ProducerType>>()
                .UseSingleOrDefault()
                .UseSelection();

            //STORE
            descriptor.Field(c => c.RegisterStoreAsync(default, default))
                .Name("registerStore")
                .Authorize(Policies.AUTHENTICATED)
                .Type<NonNullType<StoreType>>()
                .UseSingleOrDefault()
                .UseSelection();

            descriptor.Field(c => c.UpdateStoreAsync(default, default))
                .Name("updateStore")
                .Authorize(Policies.STORE)
                .Type<NonNullType<StoreType>>()
                .UseSingleOrDefault()
                .UseSelection();

            //CONSUMER
            descriptor.Field(c => c.RegisterConsumerAsync(default, default))
                .Name("registerConsumer")
                .Authorize(Policies.AUTHENTICATED)
                .Type<NonNullType<ConsumerType>>()
                .UseSingleOrDefault()
                .UseSelection();

            descriptor.Field(c => c.UpdateConsumerAsync(default, default))
                .Name("updateConsumer")
                .Authorize(Policies.CONSUMER)
                .Type<NonNullType<ConsumerType>>()
                .UseSingleOrDefault()
                .UseSelection();

            //LEGALS
            descriptor.Field(c => c.CreateBusinessLegalsAsync(default, default))
                .Name("createBusinessLegals")
                .Authorize(Policies.PRODUCER)
                .Type<NonNullType<BusinessLegalType>>()
                .UseSingleOrDefault()
                .UseSelection();

            descriptor.Field(c => c.UpdateBusinessLegalsAsync(default, default))
                .Name("updateBusinessLegals")
                .Authorize(Policies.PRODUCER)
                .Type<NonNullType<BusinessLegalType>>()
                .UseSingleOrDefault()
                .UseSelection();

            descriptor.Field(c => c.CreateConsumerLegalsAsync(default, default))
                .Name("createConsumerLegals")
                .Authorize(Policies.CONSUMER)
                .Type<NonNullType<ConsumerLegalType>>()
                .UseSingleOrDefault()
                .UseSelection();

            descriptor.Field(c => c.UpdateConsumerLegalsAsync(default, default))
                .Name("updateConsumerLegals")
                .Authorize(Policies.CONSUMER)
                .Type<NonNullType<ConsumerLegalType>>()
                .UseSingleOrDefault()
                .UseSelection();
            
            //CLOSINGS
            
            descriptor.Field(c => c.UpdateOrCreateBusinessClosingsAsync(default, default))
                .Name("updateOrCreateBusinessClosings")
                .Authorize(Policies.OWNER)
                .Type<NonNullType<ListType<ClosingType>>>()
                .UsePaging<ClosingType>()
                .UseSelection();
            
            descriptor.Field(c => c.UpdateOrCreateBusinessClosingAsync(default, default))
                .Name("updateOrCreateBusinessClosing")
                .Authorize(Policies.OWNER)
                .Type<NonNullType<ClosingType>>()
                .UseSingleOrDefault()
                .UseSelection();
            
            descriptor.Field(c => c.UpdateOrCreateDeliveryClosingAsync(default, default))
                .Name("updateOrCreateDeliveryClosing")
                .Authorize(Policies.OWNER)
                .Type<NonNullType<ClosingType>>()
                .UseSingleOrDefault()
                .UseSelection();
            
            descriptor.Field(c => c.DeleteBusinessClosingsAsync(default))
                .Name("deleteBusinessClosings")
                .Authorize(Policies.OWNER)
                .Type<BooleanType>();
            
            descriptor.Field(c => c.DeleteDeliveryClosingsAsync(default))
                .Name("deleteDeliveryClosings")
                .Authorize(Policies.OWNER)
                .Type<BooleanType>();
            
            //CATALOGS
            
            descriptor.Field(c => c.CreateCatalogAsync(default, default))
                .Name("createCatalog")
                .Authorize(Policies.OWNER)
                .Type<NonNullType<CatalogType>>()
                .UseSingleOrDefault()
                .UseSelection();
            
            descriptor.Field(c => c.UpdateCatalogAsync(default, default))
                .Name("updateCatalog")
                .Authorize(Policies.OWNER)
                .Type<NonNullType<CatalogType>>()
                .UseSingleOrDefault()
                .UseSelection();
            
            descriptor.Field(c => c.DeleteCatalogsAsync(default))
                .Name("deleteCatalogs")
                .Authorize(Policies.OWNER)
                .Type<BooleanType>();
            
            descriptor.Field(c => c.AddOrUpdateProductsToCatalogAsync(default, default))
                .Name("addOrUpdateProductsToCatalog")
                .Authorize(Policies.OWNER)
                .Type<NonNullType<CatalogType>>()
                .UseSingleOrDefault()
                .UseSelection();
            
            descriptor.Field(c => c.RemoveProductsFromCatalogAsync(default, default))
                .Name("removeProductsFromCatalog")
                .Authorize(Policies.OWNER)
                .Type<NonNullType<CatalogType>>()
                .UseSingleOrDefault()
                .UseSelection();
            
            descriptor.Field(c => c.CloneCatalogAsync(default, default))
                .Name("cloneCatalog")
                .Authorize(Policies.OWNER)
                .Type<NonNullType<CatalogType>>()
                .UseSingleOrDefault()
                .UseSelection();
            
            descriptor.Field(c => c.UpdateAllCatalogPricesAsync(default, default))
                .Name("updateAllCatalogPrices")
                .Authorize(Policies.OWNER)
                .Type<NonNullType<CatalogType>>()
                .UseSingleOrDefault()
                .UseSelection();
            
            descriptor.Field(c => c.UpdateCatalogPricesAsync(default, default))
                .Name("updateCatalogPrices")
                .Authorize(Policies.OWNER)
                .Type<NonNullType<CatalogType>>()
                .UseSingleOrDefault()
                .UseSelection();
            
            descriptor.Field(c => c.SetCatalogAsDefaultAsync(default, default))
                .Name("setCatalogAsDefault")
                .Authorize(Policies.OWNER)
                .Type<NonNullType<CatalogType>>()
                .UseSingleOrDefault()
                .UseSelection();
            
            descriptor.Field(c => c.SetCatalogsAvailabilityAsync(default, default))
                .Name("setCatalogsAvailability")
                .Authorize(Policies.OWNER)
                .Type<NonNullType<ListType<CatalogType>>>()
                .UseSingleOrDefault()
                .UseSelection();
            
            //PreAuhtorization
            
            descriptor.Field(c => c.CreatePreAuthorization(default, default))
                .Name("prepayOrder")
                .Authorize(Policies.CONSUMER)
                .Type<NonNullType<PreAuthorizationType>>()
                .UseSingleOrDefault()
                .UseSelection();

            descriptor.Field(c => c.CreateCardRegistration())
                .Name("createCardRegistration")
                .Authorize(Policies.CONSUMER)
                .Type<NonNullType<CardRegistrationType>>();
        }
    }
}
