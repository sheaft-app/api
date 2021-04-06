using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using HotChocolate;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Interfaces.Queries;
using Sheaft.Application.Models;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;
using Sheaft.Mediatr.Agreement.Commands;
using Sheaft.Mediatr.BusinessClosing.Commands;
using Sheaft.Mediatr.Catalog;
using Sheaft.Mediatr.Consumer.Commands;
using Sheaft.Mediatr.DeliveryClosing.Commands;
using Sheaft.Mediatr.DeliveryMode.Commands;
using Sheaft.Mediatr.Job.Commands;
using Sheaft.Mediatr.Legal.Commands;
using Sheaft.Mediatr.Notification.Commands;
using Sheaft.Mediatr.Order.Commands;
using Sheaft.Mediatr.PickingOrders.Commands;
using Sheaft.Mediatr.Producer.Commands;
using Sheaft.Mediatr.Product.Commands;
using Sheaft.Mediatr.ProfileInformation.Commands;
using Sheaft.Mediatr.PurchaseOrder.Commands;
using Sheaft.Mediatr.QuickOrder.Commands;
using Sheaft.Mediatr.Returnable.Commands;
using Sheaft.Mediatr.Store.Commands;
using Sheaft.Mediatr.Transactions.Commands;
using Sheaft.Mediatr.User.Commands;

namespace Sheaft.GraphQL
{
    public class SheaftMutation
    {
        private readonly ISheaftMediatr _mediator;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private CancellationToken Token => _httpContextAccessor.HttpContext.RequestAborted;

        private RequestUser CurrentUser => _currentUserService.GetCurrentUserInfo().Data;

        public SheaftMutation(
            ISheaftMediatr mediator,
            IMapper mapper,
            ICurrentUserService currentUserService,
            IHttpContextAccessor httpContextAccessor)
        {
            _mediator = mediator;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> GenerateUserSponsoringCodeAsync(ResourceIdDto input)
        {
            return await ExecuteAsync<ResourceIdDto, GenerateUserCodeCommand, string>(input, Token);
        }

        public async Task<IQueryable<JobDto>> ExportPickingOrdersAsync(ExportPickingOrdersDto input,
            [Service] IJobQueries jobQueries)
        {
            var result = await ExecuteAsync<ExportPickingOrdersDto, QueueExportPickingOrderCommand, Guid>(input, Token);
            return jobQueries.GetJob(result, CurrentUser);
        }

        public async Task<IQueryable<JobDto>> ExportPurchaseOrdersAsync(ExportPurchaseOrdersDto input,
            [Service] IJobQueries jobQueries)
        {
            var result =
                await ExecuteAsync<ExportPurchaseOrdersDto, QueueExportPurchaseOrdersCommand, Guid>(input, Token);
            return jobQueries.GetJob(result, CurrentUser);
        }

        public async Task<IQueryable<JobDto>> ExportTransactionsAsync(ExportTransactionsDto input,
            [Service] IJobQueries jobQueries)
        {
            var result = await ExecuteAsync<ExportTransactionsDto, QueueExportTransactionsCommand, Guid>(input, Token);
            return jobQueries.GetJob(result, CurrentUser);
        }

        public async Task<IQueryable<JobDto>> ExportUserDataAsync(ResourceIdDto input, [Service] IJobQueries jobQueries)
        {
            var result = await ExecuteAsync<ResourceIdDto, QueueExportUserDataCommand, Guid>(input, Token);
            return jobQueries.GetJob(result, CurrentUser);
        }

        public async Task<IQueryable<JobDto>> ResumeJobsAsync(ResourceIdsDto input, [Service] IJobQueries jobQueries)
        {
            await ExecuteAsync<ResourceIdsDto, ResumeJobsCommand>(input, Token);
            return jobQueries.GetJobs(CurrentUser).Where(j => input.Ids.Contains(j.Id));
        }

        public async Task<IQueryable<JobDto>> PauseJobsAsync(ResourceIdsDto input, [Service] IJobQueries jobQueries)
        {
            await ExecuteAsync<ResourceIdsDto, PauseJobsCommand>(input, Token);
            return jobQueries.GetJobs(CurrentUser).Where(j => input.Ids.Contains(j.Id));
        }

        public async Task<IQueryable<JobDto>> RetryJobsAsync(ResourceIdsDto input, [Service] IJobQueries jobQueries)
        {
            await ExecuteAsync<ResourceIdsDto, RetryJobsCommand>(input, Token);
            return jobQueries.GetJobs(CurrentUser).Where(j => input.Ids.Contains(j.Id));
        }

        public async Task<IQueryable<JobDto>> CancelJobsAsync(ResourceIdsWithReasonDto input,
            [Service] IJobQueries jobQueries)
        {
            await ExecuteAsync<ResourceIdsWithReasonDto, CancelJobsCommand>(input, Token);
            return jobQueries.GetJobs(CurrentUser).Where(j => input.Ids.Contains(j.Id));
        }

        public async Task<IQueryable<JobDto>> ArchiveJobsAsync(ResourceIdsDto input, [Service] IJobQueries jobQueries)
        {
            await ExecuteAsync<ResourceIdsDto, ArchiveJobsCommand>(input, Token);
            return jobQueries.GetJobs(CurrentUser).Where(j => input.Ids.Contains(j.Id));
        }

        public async Task<IQueryable<AgreementDto>> CreateAgreementAsync(CreateAgreementDto input,
            [Service] IAgreementQueries agreementQueries)
        {
            var result = await ExecuteAsync<CreateAgreementDto, CreateAgreementCommand, Guid>(input, Token);
            return agreementQueries.GetAgreement(result, CurrentUser);
        }

        public async Task<IQueryable<AgreementDto>> AcceptAgreementAsync(AcceptAgreementDto input,
            [Service] IAgreementQueries agreementQueries)
        {
            await ExecuteAsync<AcceptAgreementDto, AcceptAgreementCommand>(input, Token);
            return agreementQueries.GetAgreement(input.Id, CurrentUser);
        }

        public async Task<IQueryable<AgreementDto>> AssignCatalogToAgreementAsync(AssignCatalogToAgreementDto input,
            [Service] IAgreementQueries agreementQueries)
        {
            await ExecuteAsync<AssignCatalogToAgreementDto, AssignCatalogToAgreementCommand>(input, Token);
            return agreementQueries.GetAgreement(input.AgreementId, CurrentUser);
        }

        public async Task<IQueryable<AgreementDto>> CancelAgreementsAsync(ResourceIdsWithReasonDto input,
            [Service] IAgreementQueries agreementQueries)
        {
            await ExecuteAsync<ResourceIdsWithReasonDto, CancelAgreementsCommand>(input, Token);
            return agreementQueries.GetAgreements(CurrentUser).Where(j => input.Ids.Contains(j.Id));
        }

        public async Task<IQueryable<AgreementDto>> RefuseAgreementsAsync(ResourceIdsWithReasonDto input,
            [Service] IAgreementQueries agreementQueries)
        {
            await ExecuteAsync<ResourceIdsWithReasonDto, RefuseAgreementsCommand>(input, Token);
            return agreementQueries.GetAgreements(CurrentUser).Where(j => input.Ids.Contains(j.Id));
        }

        public async Task<DateTimeOffset> MarkMyNotificationsAsReadAsync()
        {
            var input = new MarkMyNotificationAsReadDto(DateTimeOffset.UtcNow);
            await ExecuteAsync<MarkMyNotificationAsReadDto, MarkUserNotificationsAsReadCommand>(input, Token);
            return input.ReadBefore;
        }

        public async Task<IQueryable<NotificationDto>> MarkNotificationAsReadAsync(ResourceIdDto input,
            [Service] INotificationQueries notificationQueries)
        {
            await ExecuteAsync<ResourceIdDto, MarkUserNotificationAsReadCommand>(input, Token);
            return notificationQueries.GetNotification(input.Id, CurrentUser);
        }

        public async Task<IQueryable<OrderDto>> CreateOrderAsync(CreateOrderDto input,
            [Service] IOrderQueries orderQueries)
        {
            var result = await ExecuteAsync<CreateOrderDto, CreateConsumerOrderCommand, Guid>(input, Token);
            return orderQueries.GetOrder(result, CurrentUser);
        }

        public async Task<IQueryable<OrderDto>> UpdateOrderAsync(UpdateOrderDto input,
            [Service] IOrderQueries orderQueries)
        {
            await ExecuteAsync<UpdateOrderDto, UpdateConsumerOrderCommand>(input, Token);
            return orderQueries.GetOrder(input.Id, CurrentUser);
        }

        public async Task<IQueryable<WebPayinDto>> PayOrderAsync(ResourceIdDto input,
            [Service] IPayinQueries payinQueries)
        {
            var result =
                await ExecuteAsync<ResourceIdDto, PayOrderCommand, Guid>(input, Token);
            return payinQueries.GetWebPayinTransaction(result, CurrentUser);
        }

        public async Task<IQueryable<PurchaseOrderDto>> CreateBusinessOrderAsync(CreateOrderDto input,
            [Service] IPurchaseOrderQueries purchaseOrderQueries)
        {
            var result =
                await ExecuteAsync<CreateOrderDto, CreateBusinessOrderCommand, IEnumerable<Guid>>(input, Token);
            return purchaseOrderQueries.GetPurchaseOrders(CurrentUser).Where(j => result.Contains(j.Id));
        }

        public async Task<IQueryable<PurchaseOrderDto>> AcceptPurchaseOrdersAsync(ResourceIdsDto input,
            [Service] IPurchaseOrderQueries purchaseOrderQueries)
        {
            await ExecuteAsync<ResourceIdsDto, AcceptPurchaseOrdersCommand>(input, Token);
            return purchaseOrderQueries.GetPurchaseOrders(CurrentUser).Where(j => input.Ids.Contains(j.Id));
        }

        public async Task<IQueryable<PurchaseOrderDto>> ShipPurchaseOrdersAsync(ResourceIdsDto input,
            [Service] IPurchaseOrderQueries purchaseOrderQueries)
        {
            await ExecuteAsync<ResourceIdsDto, ShipPurchaseOrdersCommand>(input, Token);
            return purchaseOrderQueries.GetPurchaseOrders(CurrentUser).Where(j => input.Ids.Contains(j.Id));
        }

        public async Task<IQueryable<PurchaseOrderDto>> DeliverPurchaseOrdersAsync(ResourceIdsDto input,
            [Service] IPurchaseOrderQueries purchaseOrderQueries)
        {
            await ExecuteAsync<ResourceIdsDto, DeliverPurchaseOrdersCommand>(input, Token);
            return purchaseOrderQueries.GetPurchaseOrders(CurrentUser).Where(j => input.Ids.Contains(j.Id));
        }

        public async Task<IQueryable<PurchaseOrderDto>> ProcessPurchaseOrdersAsync(ResourceIdsDto input,
            [Service] IPurchaseOrderQueries purchaseOrderQueries)
        {
            await ExecuteAsync<ResourceIdsDto, ProcessPurchaseOrdersCommand>(input, Token);
            return purchaseOrderQueries.GetPurchaseOrders(CurrentUser).Where(j => input.Ids.Contains(j.Id));
        }

        public async Task<IQueryable<PurchaseOrderDto>> CompletePurchaseOrdersAsync(ResourceIdsDto input,
            [Service] IPurchaseOrderQueries purchaseOrderQueries)
        {
            await ExecuteAsync<ResourceIdsDto, CompletePurchaseOrdersCommand>(input, Token);
            return purchaseOrderQueries.GetPurchaseOrders(CurrentUser).Where(j => input.Ids.Contains(j.Id));
        }

        public async Task<IQueryable<PurchaseOrderDto>> CancelPurchaseOrdersAsync(ResourceIdsWithReasonDto input,
            [Service] IPurchaseOrderQueries purchaseOrderQueries)
        {
            await ExecuteAsync<ResourceIdsWithReasonDto, CancelPurchaseOrdersCommand>(input, Token);
            return purchaseOrderQueries.GetPurchaseOrders(CurrentUser).Where(j => input.Ids.Contains(j.Id));
        }

        public async Task<IQueryable<PurchaseOrderDto>> RefusePurchaseOrdersAsync(ResourceIdsWithReasonDto input,
            [Service] IPurchaseOrderQueries purchaseOrderQueries)
        {
            await ExecuteAsync<ResourceIdsWithReasonDto, RefusePurchaseOrdersCommand>(input, Token);
            return purchaseOrderQueries.GetPurchaseOrders(CurrentUser).Where(j => input.Ids.Contains(j.Id));
        }

        public async Task<bool> DeletePurchaseOrdersAsync(ResourceIdsDto input)
        {
            return await ExecuteAsync<ResourceIdsDto, DeletePurchaseOrdersCommand>(input, Token);
        }

        public async Task<IQueryable<ProductDto>> CreateProductAsync(CreateProductDto input,
            [Service] IProductQueries productQueries)
        {
            var result = await ExecuteAsync<CreateProductDto, CreateProductCommand, Guid>(input, Token);
            return productQueries.GetProduct(result, CurrentUser);
        }

        public async Task<IQueryable<ProductDto>> UpdateProductAsync(UpdateProductDto input,
            [Service] IProductQueries productQueries)
        {
            await ExecuteAsync<UpdateProductDto, UpdateProductCommand>(input, Token);
            return productQueries.GetProduct(input.Id, CurrentUser);
        }

        public async Task<IQueryable<ProductDto>> RateProductAsync(RateProductDto input,
            [Service] IProductQueries productQueries)
        {
            await ExecuteAsync<RateProductDto, RateProductCommand>(input, Token);
            return productQueries.GetProduct(input.Id, CurrentUser);
        }

        public async Task<IQueryable<ProductDto>> UpdateProductPictureAsync(UpdateResourceIdPictureDto input,
            [Service] IProductQueries productQueries)
        {
            await ExecuteAsync<UpdateResourceIdPictureDto, UpdateProductPreviewCommand, string>(input, Token);
            return productQueries.GetProduct(input.Id, CurrentUser);
        }

        public async Task<IQueryable<ProductDto>> SetProductsAvailabilityAsync(SetResourceIdsAvailabilityDto input,
            [Service] IProductQueries productQueries)
        {
            await ExecuteAsync<SetResourceIdsAvailabilityDto, SetProductsAvailabilityCommand>(input, Token);
            return productQueries.GetProducts(CurrentUser).Where(j => input.Ids.Contains(j.Id));
        }

        public async Task<bool> DeleteProductsAsync(ResourceIdsDto input)
        {
            return await ExecuteAsync<ResourceIdsDto, DeleteProductsCommand>(input, Token);
        }

        public async Task<IQueryable<StoreDto>> RegisterStoreAsync(RegisterStoreDto input,
            [Service] IStoreQueries storeQueries)
        {
            var result = await ExecuteAsync<RegisterStoreDto, RegisterStoreCommand, Guid>(input, Token);
            return storeQueries.GetStore(result, CurrentUser);
        }

        public async Task<IQueryable<StoreDto>> UpdateStoreAsync(UpdateStoreDto input,
            [Service] IStoreQueries storeQueries)
        {
            await ExecuteAsync<UpdateStoreDto, UpdateStoreCommand>(input, Token);
            return storeQueries.GetStore(input.Id, CurrentUser);
        }

        public async Task<IQueryable<ProducerDto>> RegisterProducerAsync(RegisterProducerDto input,
            [Service] IProducerQueries producerQueries)
        {
            var result = await ExecuteAsync<RegisterProducerDto, RegisterProducerCommand, Guid>(input, Token);
            return producerQueries.GetProducer(result, CurrentUser);
        }

        public async Task<IQueryable<ProducerDto>> UpdateProducerAsync(UpdateProducerDto input,
            [Service] IProducerQueries producerQueries)
        {
            await ExecuteAsync<UpdateProducerDto, UpdateProducerCommand>(input, Token);
            return producerQueries.GetProducer(input.Id, CurrentUser);
        }

        public async Task<IQueryable<BusinessLegalDto>> CreateBusinessLegalsAsync(CreateBusinessLegalDto input,
            [Service] ILegalQueries legalQueries)
        {
            var result = await ExecuteAsync<CreateBusinessLegalDto, CreateBusinessLegalCommand, Guid>(input, Token);
            return legalQueries.GetBusinessLegals(result, CurrentUser);
        }

        public async Task<IQueryable<BusinessLegalDto>> UpdateBusinessLegalsAsync(UpdateBusinessLegalDto input,
            [Service] ILegalQueries legalQueries)
        {
            await ExecuteAsync<UpdateBusinessLegalDto, UpdateBusinessLegalCommand>(input, Token);
            return legalQueries.GetBusinessLegals(input.Id, CurrentUser);
        }

        public async Task<IQueryable<ConsumerDto>> RegisterConsumerAsync(RegisterConsumerDto input,
            [Service] IConsumerQueries consumerQueries)
        {
            var result = await ExecuteAsync<RegisterConsumerDto, RegisterConsumerCommand, Guid>(input, Token);
            return consumerQueries.GetConsumer(result, CurrentUser);
        }

        public async Task<IQueryable<ConsumerDto>> UpdateConsumerAsync(UpdateConsumerDto input,
            [Service] IConsumerQueries consumerQueries)
        {
            await ExecuteAsync<UpdateConsumerDto, UpdateConsumerCommand>(input, Token);
            return consumerQueries.GetConsumer(input.Id, CurrentUser);
        }

        public async Task<IQueryable<ConsumerLegalDto>> CreateConsumerLegalsAsync(CreateConsumerLegalDto input,
            [Service] ILegalQueries legalQueries)
        {
            var result = await ExecuteAsync<CreateConsumerLegalDto, CreateConsumerLegalCommand, Guid>(input, Token);
            return legalQueries.GetConsumerLegals(result, CurrentUser);
        }

        public async Task<IQueryable<ConsumerLegalDto>> UpdateConsumerLegalsAsync(UpdateConsumerLegalDto input,
            [Service] ILegalQueries legalQueries)
        {
            await ExecuteAsync<UpdateConsumerLegalDto, UpdateConsumerLegalCommand>(input, Token);
            return legalQueries.GetConsumerLegals(input.Id, CurrentUser);
        }

        public async Task<IQueryable<UserDto>> UpdateUserPictureAsync(UpdateResourceIdPictureDto input,
            [Service] IUserQueries userQueries)
        {
            await ExecuteAsync<UpdateResourceIdPictureDto, UpdateUserPictureCommand, string>(input, Token);
            return userQueries.GetUser(input.Id, CurrentUser);
        }

        public async Task<bool> AddPictureToUserProfileAsync(AddPictureToResourceIdDto input)
        {
            return await ExecuteAsync<AddPictureToResourceIdDto, AddPictureToUserProfileCommand>(input, Token);
        }

        public async Task<bool> RemoveUserProfilePicturesAsync(ResourceIdsDto input)
        {
            return await ExecuteAsync<ResourceIdsDto, RemoveUserProfilePicturesCommand>(input, Token);
        }

        public async Task<bool> AddPictureToProductAsync(AddPictureToResourceIdDto input)
        {
            return await ExecuteAsync<AddPictureToResourceIdDto, AddPictureToProductCommand>(input, Token);
        }

        public async Task<bool> RemoveProductPicturesAsync(ResourceIdsDto input)
        {
            return await ExecuteAsync<ResourceIdsDto, RemoveProductPicturesCommand>(input, Token);
        }

        public async Task<bool> RemoveUserAsync(ResourceIdWithReasonDto input)
        {
            return await ExecuteAsync<ResourceIdWithReasonDto, RemoveUserCommand>(input, Token);
        }

        public async Task<IQueryable<QuickOrderDto>> CreateQuickOrderAsync(CreateQuickOrderDto input,
            [Service] IQuickOrderQueries quickOrderQueries)
        {
            var result = await ExecuteAsync<CreateQuickOrderDto, CreateQuickOrderCommand, Guid>(input, Token);
            return quickOrderQueries.GetQuickOrder(result, CurrentUser);
        }

        public async Task<IQueryable<QuickOrderDto>> SetDefaultQuickOrderAsync(ResourceIdDto input,
            [Service] IQuickOrderQueries quickOrderQueries)
        {
            await ExecuteAsync<ResourceIdDto, SetDefaultQuickOrderCommand>(input, Token);
            return quickOrderQueries.GetQuickOrder(input.Id, CurrentUser);
        }

        public async Task<IQueryable<QuickOrderDto>> UpdateQuickOrderAsync(UpdateQuickOrderDto input,
            [Service] IQuickOrderQueries quickOrderQueries)
        {
            await ExecuteAsync<UpdateQuickOrderDto, UpdateQuickOrderCommand>(input, Token);
            return quickOrderQueries.GetQuickOrder(input.Id, CurrentUser);
        }

        public async Task<IQueryable<QuickOrderDto>> UpdateQuickOrderProductsAsync(
            UpdateResourceIdProductsQuantitiesDto input, [Service] IQuickOrderQueries quickOrderQueries)
        {
            await ExecuteAsync<UpdateResourceIdProductsQuantitiesDto, UpdateQuickOrderProductsCommand>(input, Token);
            return quickOrderQueries.GetQuickOrder(input.Id, CurrentUser);
        }

        public async Task<bool> DeleteQuickOrdersAsync(ResourceIdsWithReasonDto input)
        {
            return await ExecuteAsync<ResourceIdsWithReasonDto, DeleteQuickOrdersCommand>(input, Token);
        }

        public async Task<IQueryable<DeliveryModeDto>> SetDeliveryModesAvailabilityAsync(
            SetResourceIdsAvailabilityDto input, [Service] IDeliveryQueries deliveryModeQueries)
        {
            await ExecuteAsync<SetResourceIdsAvailabilityDto, SetDeliveryModesAvailabilityCommand>(input, Token);
            return deliveryModeQueries.GetDeliveries(CurrentUser).Where(j => input.Ids.Contains(j.Id));
        }

        public async Task<IQueryable<DeliveryModeDto>> CreateDeliveryModeAsync(CreateDeliveryModeDto input,
            [Service] IDeliveryQueries deliveryQueries)
        {
            var result = await ExecuteAsync<CreateDeliveryModeDto, CreateDeliveryModeCommand, Guid>(input, Token);
            return deliveryQueries.GetDelivery(result, CurrentUser);
        }

        public async Task<IQueryable<DeliveryModeDto>> UpdateDeliveryModeAsync(UpdateDeliveryModeDto input,
            [Service] IDeliveryQueries deliveryQueries)
        {
            await ExecuteAsync<UpdateDeliveryModeDto, UpdateDeliveryModeCommand>(input, Token);
            return deliveryQueries.GetDelivery(input.Id, CurrentUser);
        }

        public async Task<bool> DeleteDeliveryModeAsync(ResourceIdDto input)
        {
            return await ExecuteAsync<ResourceIdDto, DeleteDeliveryModeCommand>(input, Token);
        }

        public async Task<IQueryable<ReturnableDto>> CreateReturnableAsync(CreateReturnableDto input,
            [Service] IReturnableQueries returnableQueries)
        {
            var result = await ExecuteAsync<CreateReturnableDto, CreateReturnableCommand, Guid>(input, Token);
            return returnableQueries.GetReturnable(result, CurrentUser);
        }

        public async Task<IQueryable<ReturnableDto>> UpdateReturnableAsync(UpdateReturnableDto input,
            [Service] IReturnableQueries returnableQueries)
        {
            await ExecuteAsync<UpdateReturnableDto, UpdateReturnableCommand>(input, Token);
            return returnableQueries.GetReturnable(input.Id, CurrentUser);
        }

        public async Task<bool> DeleteReturnableAsync(ResourceIdDto input)
        {
            return await ExecuteAsync<ResourceIdDto, DeleteReturnableCommand>(input, Token);
        }

        public async Task<IQueryable<ClosingDto>> UpdateOrCreateBusinessClosingsAsync(
            UpdateOrCreateResourceIdClosingsDto input, [Service] IBusinessClosingQueries closingQueries)
        {
            var result =
                await ExecuteAsync<UpdateOrCreateResourceIdClosingsDto, UpdateOrCreateBusinessClosingsCommand,
                    IEnumerable<Guid>>(input, Token);
            return closingQueries.GetClosings(CurrentUser).Where(c => result.Contains(c.Id));
        }

        public async Task<IQueryable<ClosingDto>> UpdateOrCreateBusinessClosingAsync(
            UpdateOrCreateResourceIdClosingDto input, [Service] IBusinessClosingQueries closingQueries)
        {
            var result =
                await ExecuteAsync<UpdateOrCreateResourceIdClosingDto, UpdateOrCreateBusinessClosingCommand, Guid>(
                    input, Token);
            return closingQueries.GetClosing(result, CurrentUser);
        }

        public async Task<IQueryable<ClosingDto>> UpdateOrCreateDeliveryClosingAsync(
            UpdateOrCreateResourceIdClosingDto input, [Service] IDeliveryClosingQueries closingQueries)
        {
            var result =
                await ExecuteAsync<UpdateOrCreateResourceIdClosingDto, UpdateOrCreateDeliveryClosingCommand, Guid>(
                    input, Token);
            return closingQueries.GetClosing(result, CurrentUser);
        }

        public async Task<bool> DeleteBusinessClosingsAsync(ResourceIdsDto input)
        {
            return await ExecuteAsync<ResourceIdsDto, DeleteBusinessClosingsCommand>(input, Token);
        }

        public async Task<bool> DeleteDeliveryClosingsAsync(ResourceIdsDto input)
        {
            return await ExecuteAsync<ResourceIdsDto, DeleteDeliveryClosingsCommand>(input, Token);
        }

        public async Task<IQueryable<CatalogDto>> CreateCatalogAsync(CreateCatalogDto input,
            [Service] ICatalogQueries catalogQueries)
        {
            var result = await ExecuteAsync<CreateCatalogDto, CreateCatalogCommand, Guid>(input, Token);
            return catalogQueries.GetCatalog(result, CurrentUser);
        }

        public async Task<IQueryable<CatalogDto>> UpdateCatalogAsync(UpdateCatalogDto input,
            [Service] ICatalogQueries catalogQueries)
        {
            await ExecuteAsync<UpdateCatalogDto, UpdateCatalogCommand>(input, Token);
            return catalogQueries.GetCatalog(input.Id, CurrentUser);
        }

        public async Task<bool> DeleteCatalogsAsync(ResourceIdsDto input)
        {
            return await ExecuteAsync<ResourceIdsDto, DeleteCatalogsCommand>(input, Token);
        }

        public async Task<IQueryable<CatalogDto>> AddProductsToCatalogAsync(AddProductsToCatalogDto input,
            [Service] ICatalogQueries catalogQueries)
        {
            await ExecuteAsync<AddProductsToCatalogDto, AddProductsToCatalogCommand>(input, Token);
            return catalogQueries.GetCatalog(input.Id, CurrentUser);
        }

        public async Task<IQueryable<CatalogDto>> RemoveProductsFromCatalogAsync(RemoveProductsFromCatalogDto input,
            [Service] ICatalogQueries catalogQueries)
        {
            await ExecuteAsync<RemoveProductsFromCatalogDto, RemoveProductsFromCatalogCommand>(input, Token);
            return catalogQueries.GetCatalog(input.Id, CurrentUser);
        }

        public async Task<IQueryable<CatalogDto>> CloneCatalogAsync(CloneCatalogDto input,
            [Service] ICatalogQueries catalogQueries)
        {
            var result = await ExecuteAsync<CloneCatalogDto, CloneCatalogCommand, Guid>(input, Token);
            return catalogQueries.GetCatalog(result, CurrentUser);
        }

        public async Task<IQueryable<CatalogDto>> UpdateAllCatalogPricesAsync(UpdateAllCatalogPricesDto input,
            [Service] ICatalogQueries catalogQueries)
        {
            await ExecuteAsync<UpdateAllCatalogPricesDto, UpdateAllCatalogPricesCommand>(input, Token);
            return catalogQueries.GetCatalog(input.Id, CurrentUser);
        }

        public async Task<IQueryable<CatalogDto>> UpdateCatalogPricesAsync(UpdateCatalogPricesDto input,
            [Service] ICatalogQueries catalogQueries)
        {
            await ExecuteAsync<UpdateCatalogPricesDto, UpdateCatalogPricesCommand>(input, Token);
            return catalogQueries.GetCatalog(input.Id, CurrentUser);
        }

        public async Task<IQueryable<CatalogDto>> SetCatalogAsDefaultAsync(ResourceIdDto input,
            [Service] ICatalogQueries catalogQueries)
        {
            await ExecuteAsync<ResourceIdDto, SetCatalogAsDefaultCommand>(input, Token);
            return catalogQueries.GetCatalog(input.Id, CurrentUser);
        }

        public async Task<IQueryable<CatalogDto>> SetCatalogsAvailabilityAsync(SetResourceIdsAvailabilityDto input,
            [Service] ICatalogQueries catalogQueries)
        {
            await ExecuteAsync<SetResourceIdsAvailabilityDto, SetCatalogsAvailabilityCommand>(input, Token);
            return catalogQueries.GetCatalogs(CurrentUser).Where(c => input.Ids.Contains(c.Id));
        }

        private async Task<bool> ExecuteAsync<T, TU>(T input, CancellationToken token,
            [CallerMemberName] string memberName = null) where TU : ICommand
        {
            SetLogTransaction(input, memberName);

            var command = _mapper.Map(input, (TU) Activator.CreateInstance(typeof(TU), CurrentUser));
            var result = await _mediator.Process(command, token);
            if (result.Succeeded)
                return true;

            if (result.Exception != null)
                throw result.Exception;

            throw SheaftException.Unexpected(result.Message);
        }

        private async Task<TX> ExecuteAsync<T, TU, TX>(T input, CancellationToken token,
            [CallerMemberName] string memberName = null) where TU : ICommand<TX>
        {
            SetLogTransaction(input, memberName);

            var command = _mapper.Map(input, (TU) Activator.CreateInstance(typeof(TU), CurrentUser));
            var result = await _mediator.Process(command, token);
            if (result.Succeeded)
                return result.Data;

            if (result.Exception != null)
                throw result.Exception;

            throw SheaftException.Unexpected(result.Message);
        }

        private void SetLogTransaction(object input, string name)
        {
            NewRelic.Api.Agent.NewRelic.SetTransactionName("GraphQL", name);

            var currentTransaction = NewRelic.Api.Agent.NewRelic.GetAgent().CurrentTransaction;
            currentTransaction.AddCustomAttribute("RequestId", _httpContextAccessor.HttpContext.TraceIdentifier);
            currentTransaction.AddCustomAttribute("UserIdentifier", CurrentUser.Id.ToString("N"));
            currentTransaction.AddCustomAttribute("IsAuthenticated", CurrentUser.IsAuthenticated.ToString());
            currentTransaction.AddCustomAttribute("Roles", string.Join(";", CurrentUser.Roles));
            currentTransaction.AddCustomAttribute("GraphQL", name);

            if (input != null)
                currentTransaction.AddCustomAttribute("Input", JsonConvert.SerializeObject(input));
        }
    }
}