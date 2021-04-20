using HotChocolate.Types;
using HotChocolate.Types.Relay;
using Sheaft.Application.Security;
using Sheaft.GraphQL.Filters;
using Sheaft.GraphQL.Sorts;
using Sheaft.GraphQL.Types.Inputs;
using Sheaft.GraphQL.Types.Outputs;

namespace Sheaft.GraphQL.Types
{
    public class SheaftMutationType : ObjectType<SheaftMutation>
    {
        protected override void Configure(IObjectTypeDescriptor<SheaftMutation> descriptor)
        {
            descriptor.Field(c => c.GenerateUserSponsoringCodeAsync(default))
                .Argument("input", c => c.Type<NonNullType<GenerateUserCodeInputType>>())
                .Name("generateUserSponsoringCode")
                .Authorize(Policies.REGISTERED)
                .Type<NonNullType<StringType>>();

            //PURCHASE ORDERS
            descriptor.Field(c => c.AcceptPurchaseOrdersAsync(default, default))
                .Argument("input", c => c.Type<NonNullType<AcceptPurchaseOrdersInputType>>())
                .Name("acceptPurchaseOrders")
                .Authorize(Policies.PRODUCER)
                .Type<NonNullType<ListType<PurchaseOrderType>>>()
                .UsePaging<PurchaseOrderType>()
                .UseFiltering<PurchaseOrderFilterType>()
                .UseSorting<PurchaseOrderSortType>()
                .UseSelection();

            descriptor.Field(c => c.CompletePurchaseOrdersAsync(default, default))
                .Argument("input", c => c.Type<NonNullType<CompletePurchaseOrdersInputType>>())
                .Name("completePurchaseOrders")
                .Authorize(Policies.PRODUCER)
                .Type<NonNullType<ListType<PurchaseOrderType>>>()
                .UsePaging<PurchaseOrderType>()
                .UseFiltering<PurchaseOrderFilterType>()
                .UseSorting<PurchaseOrderSortType>()
                .UseSelection();

            descriptor.Field(c => c.DeliverPurchaseOrdersAsync(default, default))
                .Argument("input", c => c.Type<NonNullType<DeliverPurchaseOrdersInputType>>())
                .Name("deliverPurchaseOrders")
                .Authorize(Policies.PRODUCER)
                .Type<NonNullType<ListType<PurchaseOrderType>>>()
                .UsePaging<PurchaseOrderType>()
                .UseFiltering<PurchaseOrderFilterType>()
                .UseSorting<PurchaseOrderSortType>()
                .UseSelection();

            descriptor.Field(c => c.ProcessPurchaseOrdersAsync(default, default))
                .Argument("input", c => c.Type<NonNullType<ProcessPurchaseOrdersInputType>>())
                .Name("processPurchaseOrders")
                .Authorize(Policies.PRODUCER)
                .Type<NonNullType<ListType<PurchaseOrderType>>>()
                .UsePaging<PurchaseOrderType>()
                .UseFiltering<PurchaseOrderFilterType>()
                .UseSorting<PurchaseOrderSortType>()
                .UseSelection();

            descriptor.Field(c => c.ShipPurchaseOrdersAsync(default, default))
                .Argument("input", c => c.Type<NonNullType<ShipPurchaseOrdersInputType>>())
                .Name("shipPurchaseOrders")
                .Authorize(Policies.PRODUCER)
                .Type<NonNullType<ListType<PurchaseOrderType>>>()
                .UsePaging<PurchaseOrderType>()
                .UseFiltering<PurchaseOrderFilterType>()
                .UseSorting<PurchaseOrderSortType>()
                .UseSelection();

            descriptor.Field(c => c.RefusePurchaseOrdersAsync(default, default))
                .Argument("input", c => c.Type<NonNullType<RefusePurchaseOrdersInputType>>())
                .Name("refusePurchaseOrders")
                .Authorize(Policies.PRODUCER)
                .Type<NonNullType<ListType<PurchaseOrderType>>>()
                .UsePaging<PurchaseOrderType>()
                .UseFiltering<PurchaseOrderFilterType>()
                .UseSorting<PurchaseOrderSortType>()
                .UseSelection();

            descriptor.Field(c => c.CancelPurchaseOrdersAsync(default, default))
                .Argument("input", c => c.Type<NonNullType<CancelPurchaseOrdersInputType>>())
                .Name("cancelPurchaseOrders")
                .Authorize(Policies.PRODUCER)
                .Type<NonNullType<ListType<PurchaseOrderType>>>()
                .UsePaging<PurchaseOrderType>()
                .UseFiltering<PurchaseOrderFilterType>()
                .UseSorting<PurchaseOrderSortType>()
                .UseSelection();
            
            descriptor.Field(c => c.WithdrawnPurchaseOrdersAsync(default, default))
                .Argument("input", c => c.Type<NonNullType<WithdrawnPurchaseOrdersInputType>>())
                .Name("withdrawnPurchaseOrders")
                .Authorize(Policies.STORE_OR_CONSUMER)
                .Type<NonNullType<ListType<PurchaseOrderType>>>()
                .UsePaging<PurchaseOrderType>()
                .UseFiltering<PurchaseOrderFilterType>()
                .UseSorting<PurchaseOrderSortType>()
                .UseSelection();

            descriptor.Field(c => c.DeletePurchaseOrdersAsync(default))
                .Argument("input", c => c.Type<NonNullType<DeletePurchaseOrdersInputType>>())
                .Name("deletePurchaseOrders")
                .Type<NonNullType<BooleanType>>()
                .Authorize(Policies.REGISTERED);

            //AGREEMENTS
            descriptor.Field(c => c.CreateAgreementAsync(default, default))
                .Argument("input", c => c.Type<NonNullType<CreateAgreementInputType>>())
                .Name("createAgreement")
                .Authorize(Policies.STORE_OR_PRODUCER)
                .Type<NonNullType<AgreementType>>()
                .UseSingleOrDefault()
                .UseSelection();

            descriptor.Field(c => c.AcceptAgreementAsync(default, default))
                .Argument("input", c => c.Type<NonNullType<AcceptAgreementInputType>>())
                .Name("acceptAgreement")
                .Authorize(Policies.STORE_OR_PRODUCER)
                .Type<NonNullType<AgreementType>>()
                .UseSingleOrDefault()
                .UseSelection();
            
            descriptor.Field(c => c.AssignCatalogToAgreementAsync(default, default))
                .Argument("input", c => c.Type<NonNullType<AssignCatalogToAgreementInputType>>())
                .Name("assignCatalogToAgreement")
                .Authorize(Policies.PRODUCER)
                .Type<NonNullType<AgreementType>>()
                .UseSingleOrDefault()
                .UseSelection();

            descriptor.Field(c => c.RefuseAgreementsAsync(default, default))
                .Argument("input", c => c.Type<NonNullType<RefuseAgreementsInputType>>())
                .Name("refuseAgreements")
                .Authorize(Policies.STORE_OR_PRODUCER)
                .Type<NonNullType<ListType<AgreementType>>>()
                .UsePaging<AgreementType>()
                .UseFiltering<AgreementFilterType>()
                .UseSorting<AgreementSortType>()
                .UseSelection();

            descriptor.Field(c => c.CancelAgreementsAsync(default, default))
                .Argument("input", c => c.Type<NonNullType<CancelAgreementsInputType>>())
                .Name("cancelAgreements")
                .Authorize(Policies.STORE_OR_PRODUCER)
                .Type<NonNullType<ListType<AgreementType>>>()
                .UsePaging<AgreementType>()
                .UseFiltering<AgreementFilterType>()
                .UseSorting<AgreementSortType>()
                .UseSelection();

            //JOBS
            descriptor.Field(c => c.PauseJobsAsync(default, default))
                .Argument("input", c => c.Type<NonNullType<PauseJobsInputType>>())
                .Name("pauseJobs")
                .Authorize(Policies.REGISTERED)
                .Type<NonNullType<ListType<JobType>>>()
                .UsePaging<JobType>()
                .UseFiltering<JobFilterType>()
                .UseSorting<JobSortType>()
                .UseSelection();

            descriptor.Field(c => c.ResumeJobsAsync(default, default))
                .Argument("input", c => c.Type<NonNullType<ResumeJobsInputType>>())
                .Name("resumeJobs")
                .Authorize(Policies.REGISTERED)
                .Type<NonNullType<ListType<JobType>>>()
                .UsePaging<JobType>()
                .UseFiltering<JobFilterType>()
                .UseSorting<JobSortType>()
                .UseSelection();

            descriptor.Field(c => c.RetryJobsAsync(default, default))
                .Argument("input", c => c.Type<NonNullType<RetryJobsInputType>>())
                .Name("retryJobs")
                .Authorize(Policies.REGISTERED)
                .Type<NonNullType<ListType<JobType>>>()
                .UsePaging<JobType>()
                .UseFiltering<JobFilterType>()
                .UseSorting<JobSortType>()
                .UseSelection();

            descriptor.Field(c => c.CancelJobsAsync(default, default))
                .Argument("input", c => c.Type<NonNullType<CancelJobsInputType>>())
                .Name("cancelJobs")
                .Authorize(Policies.REGISTERED)
                .Type<NonNullType<ListType<JobType>>>()
                .UsePaging<JobType>()
                .UseFiltering<JobFilterType>()
                .UseSorting<JobSortType>()
                .UseSelection();

            descriptor.Field(c => c.ArchiveJobsAsync(default, default))
                .Argument("input", c => c.Type<NonNullType<ArchiveJobsInputType>>())
                .Name("archiveJobs")
                .Authorize(Policies.REGISTERED)
                .Type<NonNullType<ListType<JobType>>>()
                .UsePaging<JobType>()
                .UseFiltering<JobFilterType>()
                .UseSorting<JobSortType>()
                .UseSelection();

            //PRODUCTS
            descriptor.Field(c => c.CreateProductAsync(default, default))
                .Argument("input", c => c.Type<NonNullType<CreateProductInputType>>())
                .Name("createProduct")
                .Authorize(Policies.PRODUCER)
                .Type<NonNullType<ProductType>>()
                .UseSingleOrDefault()
                .UseSelection();

            descriptor.Field(c => c.UpdateProductAsync(default, default))
                .Argument("input", c => c.Type<NonNullType<UpdateProductInputType>>())
                .Name("updateProduct")
                .Authorize(Policies.PRODUCER)
                .Type<NonNullType<ProductType>>()
                .UseSingleOrDefault()
                .UseSelection();

            descriptor.Field(c => c.UpdateProductPictureAsync(default, default))
                .Argument("input", c => c.Type<NonNullType<UpdateProductPictureInputType>>())
                .Name("updateProductPicture")
                .Authorize(Policies.PRODUCER)
                .Type<NonNullType<ProductType>>()
                .UseSingleOrDefault()
                .UseSelection();

            descriptor.Field(c => c.SetProductsAvailabilityAsync(default, default))
                .Argument("input", c => c.Type<NonNullType<SetProductsAvailabilityInputType>>())
                .Name("setProductsAvailability")
                .Authorize(Policies.PRODUCER)
                .Type<NonNullType<ListType<ProductType>>>()
                .UsePaging<ProductType>()
                .UseFiltering<ProductFilterType>()
                .UseSorting<SearchProductSortType>()
                .UseSelection();

            descriptor.Field(c => c.DeleteProductsAsync(default))
                .Argument("input", c => c.Type<NonNullType<DeleteProductsInputType>>())
                .Name("deleteProducts")
                .Authorize(Policies.PRODUCER)
                .Type<NonNullType<BooleanType>>();

            descriptor.Field(c => c.RateProductAsync(default, default))
                .Argument("input", c => c.Type<NonNullType<RateProductInputType>>())
                .Name("rateProduct")
                .Authorize(Policies.REGISTERED)
                .Type<NonNullType<ProductType>>()
                .UseSingleOrDefault()
                .UseSelection();

            descriptor.Field(c => c.AddPictureToProductAsync(default))
                .Argument("input", c => c.Type<NonNullType<AddPictureToProductInputType>>())
                .Name("addPictureToProduct")
                .Authorize(Policies.PRODUCER)
                .Type<NonNullType<BooleanType>>();
            
            descriptor.Field(c => c.RemoveProductPicturesAsync(default))
                .Argument("input", c => c.Type<NonNullType<RemoveProductPicturesInputType>>())
                .Name("removeProductPictures")
                .Authorize(Policies.PRODUCER)
                .Type<NonNullType<BooleanType>>();

            //DELIVERY
            descriptor.Field(c => c.CreateDeliveryModeAsync(default, default))
                .Argument("input", c => c.Type<NonNullType<CreateDeliveryModeInputType>>())
                .Name("createDeliveryMode")
                .Authorize(Policies.PRODUCER)
                .Type<NonNullType<DeliveryModeType>>()
                .UseSingleOrDefault()
                .UseSelection();

            descriptor.Field(c => c.UpdateDeliveryModeAsync(default, default))
                .Argument("input", c => c.Type<NonNullType<UpdateDeliveryModeInputType>>())
                .Name("updateDeliveryMode")
                .Authorize(Policies.PRODUCER)
                .Type<NonNullType<DeliveryModeType>>()
                .UseSingleOrDefault()
                .UseSelection();

            descriptor.Field(c => c.DeleteDeliveryModeAsync(default))
                .Argument("input", c => c.Type<NonNullType<DeleteDeliveryModeInputType>>())
                .Name("deleteDeliveryMode")
                .Authorize(Policies.PRODUCER)
                .Type<NonNullType<BooleanType>>();

            descriptor.Field(c => c.SetDeliveryModesAvailabilityAsync(default, default))
                .Argument("input", c => c.Type<NonNullType<SetDeliveryModesAvailabilityInputType>>())
                .Name("setDeliveryModesAvailability")
                .Authorize(Policies.PRODUCER)
                .Type<NonNullType<ListType<DeliveryModeType>>>()
                .UsePaging<DeliveryModeType>()
                .UseFiltering<DeliveryModeFilterType>()
                .UseSorting<DeliveryModeSortType>()
                .UseSelection();

            //RETURNABLE
            descriptor.Field(c => c.CreateReturnableAsync(default, default))
                .Argument("input", c => c.Type<NonNullType<CreateReturnableInputType>>())
                .Name("createReturnable")
                .Authorize(Policies.PRODUCER)
                .Type<NonNullType<ReturnableType>>()
                .UseSingleOrDefault()
                .UseSelection();

            descriptor.Field(c => c.UpdateReturnableAsync(default, default))
                .Argument("input", c => c.Type<NonNullType<UpdateReturnableInputType>>())
                .Authorize(Policies.PRODUCER)
                .Type<NonNullType<ReturnableType>>()
                .UseSingleOrDefault()
                .UseSelection();

            descriptor.Field(c => c.DeleteReturnableAsync(default))
                .Argument("input", c => c.Type<NonNullType<DeleteReturnableInputType>>())
                .Name("deleteReturnable")
                .Authorize(Policies.PRODUCER)
                .Type<NonNullType<BooleanType>>();

            //ORDER
            descriptor.Field(c => c.CreateOrderAsync(default, default))
                .Argument("input", c => c.Type<NonNullType<CreateConsumerOrderInputType>>())
                .Name("createOrder")
                .Authorize(Policies.ANONYMOUS_OR_CONNECTED)
                .Type<NonNullType<OrderType>>()
                .UseSingleOrDefault();

            descriptor.Field(c => c.UpdateOrderAsync(default, default))
                .Argument("input", c => c.Type<NonNullType<UpdateConsumerOrderInputType>>())
                .Name("updateOrder")
                .Authorize(Policies.ANONYMOUS_OR_CONNECTED)
                .Type<NonNullType<OrderType>>()
                .UseSingleOrDefault();

            descriptor.Field(c => c.CreateWebPayinForOrderAsync(default, default))
                .Argument("input", c => c.Type<NonNullType<CreateWebPayinForOrderInputType>>())
                .Name("payOrder")
                .Authorize(Policies.CONSUMER)
                .Type<NonNullType<WebPayinType>>()
                .UseSingleOrDefault()
                .UseSelection();
            
            descriptor.Field(c => c.ResetOrderAsync(default, default))
                .Argument("input", c => c.Type<NonNullType<ResetOrderInputType>>())
                .Name("resetOrder")
                .Authorize(Policies.CONSUMER)
                .Type<NonNullType<OrderType>>()
                .UseSingleOrDefault()
                .UseSelection();

            descriptor.Field(c => c.CreateBusinessOrderAsync(default, default))
                .Argument("input", c => c.Type<NonNullType<CreateBusinessOrderInputType>>())
                .Name("createPurchaseOrders")
                .Authorize(Policies.STORE)
                .Type<NonNullType<ListType<PurchaseOrderType>>>()
                .UsePaging<PurchaseOrderType>()
                .UseFiltering<PurchaseOrderFilterType>()
                .UseSorting<PurchaseOrderSortType>()
                .UseSelection();

            //QUICKORDER
            descriptor.Field(c => c.CreateQuickOrderAsync(default, default))
                .Argument("input", c => c.Type<NonNullType<CreateQuickOrderInputType>>())
                .Name("createQuickOrder")
                .Authorize(Policies.STORE)
                .Type<NonNullType<QuickOrderType>>()
                .UseSingleOrDefault()
                .UseSelection();

            descriptor.Field(c => c.SetDefaultQuickOrderAsync(default, default))
                .Argument("input", c => c.Type<NonNullType<SetQuickOrderAsDefaultInputType>>())
                .Name("setDefaultQuickOrder")
                .Authorize(Policies.STORE)
                .Type<NonNullType<QuickOrderType>>()
                .UseSingleOrDefault()
                .UseSelection();

            descriptor.Field(c => c.UpdateQuickOrderAsync(default, default))
                .Argument("input", c => c.Type<NonNullType<UpdateQuickOrderInputType>>())
                .Name("updateQuickOrder")
                .Authorize(Policies.STORE)
                .Type<NonNullType<QuickOrderType>>()
                .UseSingleOrDefault()
                .UseSelection();

            descriptor.Field(c => c.DeleteQuickOrdersAsync(default))
                .Argument("input", c => c.Type<NonNullType<DeleteQuickOrdersInputType>>())
                .Name("deleteQuickOrders")
                .Authorize(Policies.STORE)
                .Type<NonNullType<BooleanType>>();

            //USER
            descriptor.Field(c => c.UpdateUserPictureAsync(default, default))
                .Argument("input", c => c.Type<NonNullType<UpdateUserPictureInputType>>())
                .Name("updateUserPicture")
                .Authorize(Policies.REGISTERED)
                .Type<NonNullType<UserType>>()
                .UseSingleOrDefault()
                .UseSelection();
            
            descriptor.Field(c => c.AddPictureToUserAsync(default))
                .Argument("input", c => c.Type<NonNullType<AddPictureToUserInputType>>())
                .Name("addPictureToUser")
                .Authorize(Policies.REGISTERED)
                .Type<NonNullType<BooleanType>>();
            
            descriptor.Field(c => c.RemoveUserPicturesAsync(default))
                .Argument("input", c => c.Type<NonNullType<RemoveUserPicturesInputType>>())
                .Name("removeUserPictures")
                .Authorize(Policies.REGISTERED)
                .Type<NonNullType<BooleanType>>();

            descriptor.Field(c => c.RemoveUserAsync(default))
                .Argument("input", c => c.Type<NonNullType<RemoveUserInputType>>())
                .Name("removeUser")
                .Authorize(Policies.REGISTERED)
                .Type<NonNullType<BooleanType>>();

            //EXPORT
            descriptor.Field(c => c.ExportUserDataAsync(default, default))
                .Argument("input", c => c.Type<NonNullType<QueueExportUserDataInputType>>())
                .Name("exportRGPD")
                .Authorize(Policies.REGISTERED)
                .Type<NonNullType<JobType>>()
                .UseSingleOrDefault()
                .UseSelection();

            descriptor.Field(c => c.ExportPickingOrdersAsync(default, default))
                .Argument("input", c => c.Type<NonNullType<QueueExportPickingOrderInputType>>())
                .Name("exportPickingFromOrders")
                .Authorize(Policies.PRODUCER)
                .Type<NonNullType<JobType>>()
                .UseSingleOrDefault()
                .UseSelection();

            descriptor.Field(c => c.ExportPurchaseOrdersAsync(default, default))
                .Argument("input", c => c.Type<NonNullType<QueueExportPurchaseOrdersInputType>>())
                .Name("exportPurchaseOrders")
                .Authorize(Policies.PRODUCER)
                .Type<NonNullType<JobType>>()
                .UseSingleOrDefault()
                .UseSelection();

            descriptor.Field(c => c.ExportTransactionsAsync(default, default))
                .Argument("input", c => c.Type<NonNullType<QueueExportTransactionsInputType>>())
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
                .Argument("input", c => c.Type<NonNullType<MarkUserNotificationAsReadInputType>>())
                .Name("markUserNotificationAsRead")
                .Authorize(Policies.REGISTERED)
                .Type<NonNullType<NotificationType>>()
                .UseSingleOrDefault()
                .UseSelection();

            //PRODUCER
            descriptor.Field(c => c.RegisterProducerAsync(default, default))
                .Argument("input", c => c.Type<NonNullType<RegisterProducerInputType>>())
                .Name("registerProducer")
                .Authorize(Policies.AUTHENTICATED)
                .Type<NonNullType<ProducerType>>()
                .UseSingleOrDefault()
                .UseSelection();

            descriptor.Field(c => c.UpdateProducerAsync(default, default))
                .Argument("input", c => c.Type<NonNullType<UpdateProducerInputType>>())
                .Name("updateProducer")
                .Authorize(Policies.PRODUCER)
                .Type<NonNullType<ProducerType>>()
                .UseSingleOrDefault()
                .UseSelection();

            //STORE
            descriptor.Field(c => c.RegisterStoreAsync(default, default))
                .Argument("input", c => c.Type<NonNullType<RegisterStoreInputType>>())
                .Name("registerStore")
                .Authorize(Policies.AUTHENTICATED)
                .Type<NonNullType<StoreType>>()
                .UseSingleOrDefault()
                .UseSelection();

            descriptor.Field(c => c.UpdateStoreAsync(default, default))
                .Argument("input", c => c.Type<NonNullType<UpdateStoreInputType>>())
                .Name("updateStore")
                .Authorize(Policies.STORE)
                .Type<NonNullType<StoreType>>()
                .UseSingleOrDefault()
                .UseSelection();

            //CONSUMER
            descriptor.Field(c => c.RegisterConsumerAsync(default, default))
                .Argument("input", c => c.Type<NonNullType<RegisterConsumerInputType>>())
                .Name("registerConsumer")
                .Authorize(Policies.AUTHENTICATED)
                .Type<NonNullType<ConsumerType>>()
                .UseSingleOrDefault()
                .UseSelection();

            descriptor.Field(c => c.UpdateConsumerAsync(default, default))
                .Argument("input", c => c.Type<NonNullType<UpdateConsumerInputType>>())
                .Name("updateConsumer")
                .Authorize(Policies.CONSUMER)
                .Type<NonNullType<ConsumerType>>()
                .UseSingleOrDefault()
                .UseSelection();

            //LEGALS
            descriptor.Field(c => c.CreateBusinessLegalsAsync(default, default))
                .Argument("input", c => c.Type<NonNullType<CreateBusinessLegalInputType>>())
                .Name("createBusinessLegals")
                .Authorize(Policies.PRODUCER)
                .Type<NonNullType<BusinessLegalType>>()
                .UseSingleOrDefault()
                .UseSelection();

            descriptor.Field(c => c.UpdateBusinessLegalsAsync(default, default))
                .Argument("input", c => c.Type<NonNullType<UpdateBusinessLegalsInputType>>())
                .Name("updateBusinessLegals")
                .Authorize(Policies.PRODUCER)
                .Type<NonNullType<BusinessLegalType>>()
                .UseSingleOrDefault()
                .UseSelection();

            descriptor.Field(c => c.CreateConsumerLegalsAsync(default, default))
                .Argument("input", c => c.Type<NonNullType<CreateConsumerLegalsInputType>>())
                .Name("createConsumerLegals")
                .Authorize(Policies.CONSUMER)
                .Type<NonNullType<ConsumerLegalType>>()
                .UseSingleOrDefault()
                .UseSelection();

            descriptor.Field(c => c.UpdateConsumerLegalsAsync(default, default))
                .Argument("input", c => c.Type<NonNullType<UpdateConsumerLegalsInputType>>())
                .Name("updateConsumerLegals")
                .Authorize(Policies.CONSUMER)
                .Type<NonNullType<ConsumerLegalType>>()
                .UseSingleOrDefault()
                .UseSelection();
            
            //CLOSINGS
            
            descriptor.Field(c => c.UpdateOrCreateBusinessClosingsAsync(default, default))
                .Argument("input", c => c.Type<NonNullType<UpdateOrCreateBusinessClosingsInputType>>())
                .Name("updateOrCreateBusinessClosings")
                .Authorize(Policies.OWNER)
                .Type<NonNullType<ListType<ClosingType>>>()
                .UsePaging<ClosingType>()
                .UseSelection();
            
            descriptor.Field(c => c.UpdateOrCreateBusinessClosingAsync(default, default))
                .Argument("input", c => c.Type<NonNullType<UpdateOrCreateBusinessClosingInputType>>())
                .Name("updateOrCreateBusinessClosing")
                .Authorize(Policies.OWNER)
                .Type<NonNullType<ClosingType>>()
                .UseSingleOrDefault()
                .UseSelection();
            
            descriptor.Field(c => c.UpdateOrCreateDeliveryClosingAsync(default, default))
                .Argument("input", c => c.Type<NonNullType<UpdateOrCreateDeliveryClosingInputType>>())
                .Name("updateOrCreateDeliveryClosing")
                .Authorize(Policies.OWNER)
                .Type<NonNullType<ClosingType>>()
                .UseSingleOrDefault()
                .UseSelection();
            
            descriptor.Field(c => c.UpdateOrCreateDeliveryClosingsAsync(default, default))
                .Argument("input", c => c.Type<NonNullType<UpdateOrCreateDeliveryClosingsInputType>>())
                .Name("updateOrCreateDeliveryClosings")
                .Authorize(Policies.OWNER)
                .Type<NonNullType<ListType<ClosingType>>>()
                .UsePaging<ClosingType>()
                .UseSelection();
            
            descriptor.Field(c => c.DeleteBusinessClosingsAsync(default))
                .Argument("input", c => c.Type<NonNullType<DeleteBusinessClosingsInputType>>())
                .Name("deleteBusinessClosings")
                .Authorize(Policies.OWNER)
                .Type<BooleanType>();
            
            descriptor.Field(c => c.DeleteDeliveryClosingsAsync(default))
                .Argument("input", c => c.Type<NonNullType<DeleteDeliveryClosingsInputType>>())
                .Name("deleteDeliveryClosings")
                .Authorize(Policies.OWNER)
                .Type<BooleanType>();
            
            //CATALOGS
            
            descriptor.Field(c => c.CreateCatalogAsync(default, default))
                .Argument("input", c => c.Type<NonNullType<CreateCatalogInputType>>())
                .Name("createCatalog")
                .Authorize(Policies.OWNER)
                .Type<NonNullType<CatalogType>>()
                .UseSingleOrDefault()
                .UseSelection();
            
            descriptor.Field(c => c.UpdateCatalogAsync(default, default))
                .Argument("input", c => c.Type<NonNullType<UpdateCatalogInputType>>())
                .Name("updateCatalog")
                .Authorize(Policies.OWNER)
                .Type<NonNullType<CatalogType>>()
                .UseSingleOrDefault()
                .UseSelection();
            
            descriptor.Field(c => c.DeleteCatalogsAsync(default))
                .Argument("input", c => c.Type<NonNullType<DeleteCatalogsInputType>>())
                .Name("deleteCatalogs")
                .Authorize(Policies.OWNER)
                .Type<BooleanType>();
            
            descriptor.Field(c => c.AddOrUpdateProductsToCatalogAsync(default, default))
                .Argument("input", c => c.Type<NonNullType<AddOrUpdateProductsToCatalogInputType>>())
                .Name("addOrUpdateProductsToCatalog")
                .Authorize(Policies.OWNER)
                .Type<NonNullType<CatalogType>>()
                .UseSingleOrDefault()
                .UseSelection();
            
            descriptor.Field(c => c.RemoveProductsFromCatalogAsync(default, default))
                .Argument("input", c => c.Type<NonNullType<RemoveProductsFromCatalogInputType>>())
                .Name("removeProductsFromCatalog")
                .Authorize(Policies.OWNER)
                .Type<NonNullType<CatalogType>>()
                .UseSingleOrDefault()
                .UseSelection();
            
            descriptor.Field(c => c.CloneCatalogAsync(default, default))
                .Argument("input", c => c.Type<NonNullType<CloneCatalogInputType>>())
                .Name("cloneCatalog")
                .Authorize(Policies.OWNER)
                .Type<NonNullType<CatalogType>>()
                .UseSingleOrDefault()
                .UseSelection();
            
            descriptor.Field(c => c.UpdateAllCatalogPricesAsync(default, default))
                .Argument("input", c => c.Type<NonNullType<UpdateAllCatalogPricesInputType>>())
                .Name("updateAllCatalogPrices")
                .Authorize(Policies.OWNER)
                .Type<NonNullType<CatalogType>>()
                .UseSingleOrDefault()
                .UseSelection();
            
            descriptor.Field(c => c.UpdateCatalogPricesAsync(default, default))
                .Argument("input", c => c.Type<NonNullType<UpdateCatalogPricesInputType>>())
                .Name("updateCatalogPrices")
                .Authorize(Policies.OWNER)
                .Type<NonNullType<CatalogType>>()
                .UseSingleOrDefault()
                .UseSelection();
            
            descriptor.Field(c => c.SetCatalogAsDefaultAsync(default, default))
                .Argument("input", c => c.Type<NonNullType<SetCatalogAsDefaultInputType>>())
                .Name("setCatalogAsDefault")
                .Authorize(Policies.OWNER)
                .Type<NonNullType<CatalogType>>()
                .UseSingleOrDefault()
                .UseSelection();
            
            descriptor.Field(c => c.SetCatalogsAvailabilityAsync(default, default))
                .Argument("input", c => c.Type<NonNullType<SetCatalogsAvailabilityInputType>>())
                .Name("setCatalogsAvailability")
                .Authorize(Policies.OWNER)
                .Type<NonNullType<ListType<CatalogType>>>()
                .UseSingleOrDefault()
                .UseSelection();
            
            //PreAuhtorization
            
            descriptor.Field(c => c.CreatePreAuthorization(default, default))
                .Argument("input", c => c.Type<NonNullType<CreatePreAuthorizationInputType>>())
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
