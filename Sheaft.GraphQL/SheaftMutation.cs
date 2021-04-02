using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using HotChocolate;
using Microsoft.AspNetCore.Http;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Interfaces.Queries;
using Sheaft.Application.Models;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;
using Sheaft.Mediatr.Agreement.Commands;
using Sheaft.Mediatr.BusinessClosing.Commands;
using Sheaft.Mediatr.Consumer.Commands;
using Sheaft.Mediatr.DeliveryClosing.Commands;
using Sheaft.Mediatr.DeliveryMode.Commands;
using Sheaft.Mediatr.Document.Commands;
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
            SetLogTransaction(nameof(GenerateUserSponsoringCodeAsync));
            return await ExecuteCommandAsync<GenerateUserCodeCommand, string>(
                _mapper.Map(input, new GenerateUserCodeCommand(CurrentUser)), Token);
        }

        public async Task<IQueryable<JobDto>> ExportPickingOrdersAsync(ExportPickingOrdersDto input,
            [Service] IJobQueries jobQueries)
        {
            SetLogTransaction(nameof(ExportPickingOrdersAsync));
            var result =
                await ExecuteCommandAsync<QueueExportPickingOrderCommand, Guid>(
                    _mapper.Map(input, new QueueExportPickingOrderCommand(CurrentUser) {ProducerId = CurrentUser.Id}),
                    Token);
            return jobQueries.GetJob(result, CurrentUser);
        }

        public async Task<IQueryable<JobDto>> ExportPurchaseOrdersAsync(ExportPurchaseOrdersDto input,
            [Service] IJobQueries jobQueries)
        {
            SetLogTransaction(nameof(ExportPurchaseOrdersAsync));
            var result =
                await ExecuteCommandAsync<QueueExportPurchaseOrdersCommand, Guid>(
                    _mapper.Map(input, new QueueExportPurchaseOrdersCommand(CurrentUser) {UserId = CurrentUser.Id}),
                    Token);
            return jobQueries.GetJob(result, CurrentUser);
        }

        public async Task<IQueryable<JobDto>> ExportTransactionsAsync(ExportTransactionsDto input,
            [Service] IJobQueries jobQueries)
        {
            SetLogTransaction(nameof(ExportTransactionsAsync));
            var result =
                await ExecuteCommandAsync<QueueExportTransactionsCommand, Guid>(
                    _mapper.Map(input, new QueueExportTransactionsCommand(CurrentUser) {UserId = CurrentUser.Id}),
                    Token);
            return jobQueries.GetJob(result, CurrentUser);
        }

        public async Task<IQueryable<JobDto>> ExportUserDataAsync(ResourceIdDto input, [Service] IJobQueries jobQueries)
        {
            SetLogTransaction(nameof(ExportUserDataAsync));
            var result =
                await ExecuteCommandAsync<QueueExportUserDataCommand, Guid>(
                    _mapper.Map(input, new QueueExportUserDataCommand(CurrentUser)), Token);
            return jobQueries.GetJob(result, CurrentUser);
        }

        public async Task<IQueryable<JobDto>> ResumeJobsAsync(ResourceIdsDto input, [Service] IJobQueries jobQueries)
        {
            SetLogTransaction(nameof(ResumeJobsAsync));
            await ExecuteCommandAsync(_mapper.Map(input, new ResumeJobsCommand(CurrentUser)), Token);
            return jobQueries.GetJobs(CurrentUser).Where(j => input.Ids.Contains(j.Id));
        }

        public async Task<IQueryable<JobDto>> PauseJobsAsync(ResourceIdsDto input, [Service] IJobQueries jobQueries)
        {
            SetLogTransaction(nameof(PauseJobsAsync));
            await ExecuteCommandAsync(_mapper.Map(input, new PauseJobsCommand(CurrentUser)), Token);
            return jobQueries.GetJobs(CurrentUser).Where(j => input.Ids.Contains(j.Id));
        }

        public async Task<IQueryable<JobDto>> RetryJobsAsync(ResourceIdsDto input, [Service] IJobQueries jobQueries)
        {
            SetLogTransaction(nameof(RetryJobsAsync));
            await ExecuteCommandAsync(_mapper.Map(input, new RetryJobsCommand(CurrentUser)), Token);
            return jobQueries.GetJobs(CurrentUser).Where(j => input.Ids.Contains(j.Id));
        }

        public async Task<IQueryable<JobDto>> CancelJobsAsync(ResourceIdsWithReasonDto input,
            [Service] IJobQueries jobQueries)
        {
            SetLogTransaction(nameof(CancelJobsAsync));
            await ExecuteCommandAsync(_mapper.Map(input, new CancelJobsCommand(CurrentUser)), Token);
            return jobQueries.GetJobs(CurrentUser).Where(j => input.Ids.Contains(j.Id));
        }

        public async Task<IQueryable<JobDto>> ArchiveJobsAsync(ResourceIdsDto input, [Service] IJobQueries jobQueries)
        {
            SetLogTransaction(nameof(ArchiveJobsAsync));
            await ExecuteCommandAsync(_mapper.Map(input, new ArchiveJobsCommand(CurrentUser)), Token);
            return jobQueries.GetJobs(CurrentUser).Where(j => input.Ids.Contains(j.Id));
        }

        public async Task<IQueryable<AgreementDto>> CreateAgreementAsync(CreateAgreementDto input,
            [Service] IAgreementQueries agreementQueries)
        {
            SetLogTransaction(nameof(CreateAgreementAsync));
            var result =
                await ExecuteCommandAsync<CreateAgreementCommand, Guid>(
                    _mapper.Map(input, new CreateAgreementCommand(CurrentUser)), Token);
            return agreementQueries.GetAgreement(result, CurrentUser);
        }

        public async Task<IQueryable<AgreementDto>> AcceptAgreementAsync(ResourceIdTimeSlotsDto input,
            [Service] IAgreementQueries agreementQueries)
        {
            SetLogTransaction(nameof(AcceptAgreementAsync));
            await ExecuteCommandAsync(_mapper.Map(input, new AcceptAgreementCommand(CurrentUser)), Token);
            return agreementQueries.GetAgreement(input.Id, CurrentUser);
        }

        public async Task<IQueryable<AgreementDto>> CancelAgreementsAsync(ResourceIdsWithReasonDto input,
            [Service] IAgreementQueries agreementQueries)
        {
            SetLogTransaction(nameof(CancelAgreementsAsync));
            await ExecuteCommandAsync(_mapper.Map(input, new CancelAgreementsCommand(CurrentUser)), Token);
            return agreementQueries.GetAgreements(CurrentUser).Where(j => input.Ids.Contains(j.Id));
        }

        public async Task<IQueryable<AgreementDto>> RefuseAgreementsAsync(ResourceIdsWithReasonDto input,
            [Service] IAgreementQueries agreementQueries)
        {
            SetLogTransaction(nameof(RefuseAgreementsAsync));
            await ExecuteCommandAsync(_mapper.Map(input, new RefuseAgreementsCommand(CurrentUser)), Token);
            return agreementQueries.GetAgreements(CurrentUser).Where(j => input.Ids.Contains(j.Id));
        }

        public async Task<DateTimeOffset> MarkMyNotificationsAsReadAsync()
        {
            SetLogTransaction(nameof(MarkMyNotificationsAsReadAsync));
            var input = new MarkUserNotificationsAsReadCommand(CurrentUser)
                {UserId = CurrentUser.Id, ReadBefore = DateTimeOffset.UtcNow};
            await ExecuteCommandAsync(input, Token);
            return input.ReadBefore;
        }

        public async Task<IQueryable<NotificationDto>> MarkNotificationAsReadAsync(ResourceIdDto input,
            [Service] INotificationQueries notificationQueries)
        {
            SetLogTransaction(nameof(MarkNotificationAsReadAsync));
            await ExecuteCommandAsync(_mapper.Map(input, new MarkUserNotificationAsReadCommand(CurrentUser)), Token);
            return notificationQueries.GetNotification(input.Id, CurrentUser);
        }

        public async Task<IQueryable<OrderDto>> CreateOrderAsync(CreateOrderDto input,
            [Service] IOrderQueries orderQueries)
        {
            SetLogTransaction(nameof(CreateOrderAsync));
            var result =
                await ExecuteCommandAsync<CreateConsumerOrderCommand, Guid>(
                    _mapper.Map(input,
                        new CreateConsumerOrderCommand(CurrentUser)
                            {UserId = CurrentUser.Id}), Token);
            return orderQueries.GetOrder(result, CurrentUser);
        }

        public async Task<IQueryable<OrderDto>> UpdateOrderAsync(UpdateOrderDto input,
            [Service] IOrderQueries orderQueries)
        {
            SetLogTransaction(nameof(UpdateOrderAsync));
            await ExecuteCommandAsync(_mapper.Map(input, new UpdateConsumerOrderCommand(CurrentUser){ UserId = CurrentUser.IsAuthenticated ? CurrentUser.Id : (Guid?)null }), Token);
            return orderQueries.GetOrder(input.Id, CurrentUser);
        }

        public async Task<IQueryable<WebPayinDto>> PayOrderAsync(ResourceIdDto input, [Service] IPayinQueries payinQueries)
        {
            SetLogTransaction(nameof(PayOrderAsync));
            var result =
                await ExecuteCommandAsync<PayOrderCommand, Guid>(_mapper.Map(input, new PayOrderCommand(CurrentUser)),
                    Token);
            return payinQueries.GetWebPayinTransaction(result, CurrentUser);
        }

        public async Task<IQueryable<PurchaseOrderDto>> CreateBusinessOrderAsync(CreateOrderDto input,
            [Service] IPurchaseOrderQueries purchaseOrderQueries)
        {
            SetLogTransaction(nameof(CreateBusinessOrderAsync));
            var result =
                await ExecuteCommandAsync<CreateBusinessOrderCommand, IEnumerable<Guid>>(
                    _mapper.Map(input, new CreateBusinessOrderCommand(CurrentUser) {UserId = CurrentUser.Id}), Token);
            return purchaseOrderQueries.GetPurchaseOrders(CurrentUser).Where(j => result.Contains(j.Id));
        }

        public async Task<IQueryable<DocumentDto>> CreateDocumentAsync(CreateDocumentDto input,
            [Service] IDocumentQueries documentQueries)
        {
            SetLogTransaction(nameof(CreateDocumentAsync));
            var result =
                await ExecuteCommandAsync<CreateDocumentCommand, Guid>(
                    _mapper.Map(input, new CreateDocumentCommand(CurrentUser)), Token);
            return documentQueries.GetDocument(result, CurrentUser);
        }

        public async Task<bool> RemoveDocumentAsync(ResourceIdDto input)
        {
            SetLogTransaction(nameof(RemoveDocumentAsync));
            return await ExecuteCommandAsync(_mapper.Map(input, new DeleteDocumentCommand(CurrentUser)), Token);
        }

        public async Task<IQueryable<PurchaseOrderDto>> AcceptPurchaseOrdersAsync(ResourceIdsDto input,
            [Service] IPurchaseOrderQueries purchaseOrderQueries)
        {
            SetLogTransaction(nameof(AcceptPurchaseOrdersAsync));
            await ExecuteCommandAsync(_mapper.Map(input, new AcceptPurchaseOrdersCommand(CurrentUser)), Token);
            return purchaseOrderQueries.GetPurchaseOrders(CurrentUser).Where(j => input.Ids.Contains(j.Id));
        }

        public async Task<IQueryable<PurchaseOrderDto>> ShipPurchaseOrdersAsync(ResourceIdsDto input,
            [Service] IPurchaseOrderQueries purchaseOrderQueries)
        {
            SetLogTransaction(nameof(ShipPurchaseOrdersAsync));
            await ExecuteCommandAsync(_mapper.Map(input, new ShipPurchaseOrdersCommand(CurrentUser)), Token);
            return purchaseOrderQueries.GetPurchaseOrders(CurrentUser).Where(j => input.Ids.Contains(j.Id));
        }

        public async Task<IQueryable<PurchaseOrderDto>> DeliverPurchaseOrdersAsync(ResourceIdsDto input,
            [Service] IPurchaseOrderQueries purchaseOrderQueries)
        {
            SetLogTransaction(nameof(DeliverPurchaseOrdersAsync));
            await ExecuteCommandAsync(_mapper.Map(input, new DeliverPurchaseOrdersCommand(CurrentUser)), Token);
            return purchaseOrderQueries.GetPurchaseOrders(CurrentUser).Where(j => input.Ids.Contains(j.Id));
        }

        public async Task<IQueryable<PurchaseOrderDto>> ProcessPurchaseOrdersAsync(ResourceIdsDto input,
            [Service] IPurchaseOrderQueries purchaseOrderQueries)
        {
            SetLogTransaction(nameof(ProcessPurchaseOrdersAsync));
            await ExecuteCommandAsync(_mapper.Map(input, new ProcessPurchaseOrdersCommand(CurrentUser)), Token);
            return purchaseOrderQueries.GetPurchaseOrders(CurrentUser).Where(j => input.Ids.Contains(j.Id));
        }

        public async Task<IQueryable<PurchaseOrderDto>> CompletePurchaseOrdersAsync(ResourceIdsDto input,
            [Service] IPurchaseOrderQueries purchaseOrderQueries)
        {
            SetLogTransaction(nameof(CompletePurchaseOrdersAsync));
            await ExecuteCommandAsync(_mapper.Map(input, new CompletePurchaseOrdersCommand(CurrentUser)), Token);
            return purchaseOrderQueries.GetPurchaseOrders(CurrentUser).Where(j => input.Ids.Contains(j.Id));
        }

        public async Task<IQueryable<PurchaseOrderDto>> CancelPurchaseOrdersAsync(ResourceIdsWithReasonDto input,
            [Service] IPurchaseOrderQueries purchaseOrderQueries)
        {
            SetLogTransaction(nameof(CancelPurchaseOrdersAsync));
            await ExecuteCommandAsync(_mapper.Map(input, new CancelPurchaseOrdersCommand(CurrentUser)), Token);
            return purchaseOrderQueries.GetPurchaseOrders(CurrentUser).Where(j => input.Ids.Contains(j.Id));
        }

        public async Task<IQueryable<PurchaseOrderDto>> RefusePurchaseOrdersAsync(ResourceIdsWithReasonDto input,
            [Service] IPurchaseOrderQueries purchaseOrderQueries)
        {
            SetLogTransaction(nameof(RefusePurchaseOrdersAsync));
            await ExecuteCommandAsync(_mapper.Map(input, new RefusePurchaseOrdersCommand(CurrentUser)), Token);
            return purchaseOrderQueries.GetPurchaseOrders(CurrentUser).Where(j => input.Ids.Contains(j.Id));
        }

        public async Task<bool> DeletePurchaseOrdersAsync(ResourceIdsDto input)
        {
            SetLogTransaction(nameof(DeletePurchaseOrdersAsync));
            return await ExecuteCommandAsync(_mapper.Map(input, new DeletePurchaseOrdersCommand(CurrentUser)), Token);
        }

        public async Task<IQueryable<ProductDto>> CreateProductAsync(CreateProductDto input,
            [Service] IProductQueries productQueries)
        {
            SetLogTransaction(nameof(CreateProductAsync));
            var result =
                await ExecuteCommandAsync<CreateProductCommand, Guid>(
                    _mapper.Map(input, new CreateProductCommand(CurrentUser) {ProducerId = CurrentUser.Id}), Token);
            return productQueries.GetProduct(result, CurrentUser);
        }

        public async Task<IQueryable<ProductDto>> UpdateProductAsync(UpdateProductDto input,
            [Service] IProductQueries productQueries)
        {
            SetLogTransaction(nameof(UpdateProductAsync));
            await ExecuteCommandAsync(_mapper.Map(input, new UpdateProductCommand(CurrentUser)), Token);
            return productQueries.GetProduct(input.Id, CurrentUser);
        }

        public async Task<IQueryable<ProductDto>> RateProductAsync(RateProductDto input,
            [Service] IProductQueries productQueries)
        {
            SetLogTransaction(nameof(RateProductAsync));
            await ExecuteCommandAsync(_mapper.Map(input, new RateProductCommand(CurrentUser) {UserId = CurrentUser.Id}),
                Token);
            return productQueries.GetProduct(input.Id, CurrentUser);
        }

        public async Task<IQueryable<ProductDto>> UpdateProductPictureAsync(UpdateResourceIdPictureDto input,
            [Service] IProductQueries productQueries)
        {
            SetLogTransaction(nameof(UpdateProductPictureAsync));
            await ExecuteCommandAsync<UpdateProductPreviewCommand, string>(
                _mapper.Map(input, new UpdateProductPreviewCommand(CurrentUser)), Token);
            return productQueries.GetProduct(input.Id, CurrentUser);
        }

        public async Task<IQueryable<ProductDto>> SetProductsAvailabilityAsync(SetResourceIdsAvailabilityDto input,
            [Service] IProductQueries productQueries)
        {
            SetLogTransaction(nameof(SetProductsAvailabilityAsync));
            await ExecuteCommandAsync(_mapper.Map(input, new SetProductsAvailabilityCommand(CurrentUser)), Token);
            return productQueries.GetProducts(CurrentUser).Where(j => input.Ids.Contains(j.Id));
        }

        public async Task<IQueryable<ProductDto>> SetProductsSearchabilityAsync(SetResourceIdsVisibilityDto input,
            [Service] IProductQueries productQueries)
        {
            SetLogTransaction(nameof(SetProductsSearchabilityAsync));
            await ExecuteCommandAsync(_mapper.Map(input, new SetProductsSearchabilityCommand(CurrentUser)), Token);
            return productQueries.GetProducts(CurrentUser).Where(j => input.Ids.Contains(j.Id));
        }

        public async Task<bool> DeleteProductsAsync(ResourceIdsDto input)
        {
            SetLogTransaction(nameof(DeleteProductsAsync));
            return await ExecuteCommandAsync(_mapper.Map(input, new DeleteProductsCommand(CurrentUser)), Token);
        }

        public async Task<IQueryable<StoreDto>> RegisterStoreAsync(RegisterStoreDto input,
            [Service] IStoreQueries storeQueries)
        {
            SetLogTransaction(nameof(RegisterStoreAsync));
            var result =
                await ExecuteCommandAsync<RegisterStoreCommand, Guid>(
                    _mapper.Map(input, new RegisterStoreCommand(CurrentUser) {StoreId = CurrentUser.Id}), Token);
            return storeQueries.GetStore(result, CurrentUser);
        }

        public async Task<IQueryable<StoreDto>> UpdateStoreAsync(UpdateStoreDto input,
            [Service] IStoreQueries storeQueries)
        {
            SetLogTransaction(nameof(UpdateStoreAsync));
            await ExecuteCommandAsync(_mapper.Map(input, new UpdateStoreCommand(CurrentUser)), Token);
            return storeQueries.GetStore(input.Id, CurrentUser);
        }

        public async Task<IQueryable<ProducerDto>> RegisterProducerAsync(RegisterProducerDto input,
            [Service] IProducerQueries producerQueries)
        {
            SetLogTransaction(nameof(RegisterProducerAsync));
            var result =
                await ExecuteCommandAsync<RegisterProducerCommand, Guid>(
                    _mapper.Map(input, new RegisterProducerCommand(CurrentUser) {ProducerId = CurrentUser.Id}), Token);
            return producerQueries.GetProducer(result, CurrentUser);
        }

        public async Task<IQueryable<ProducerDto>> UpdateProducerAsync(UpdateProducerDto input,
            [Service] IProducerQueries producerQueries)
        {
            SetLogTransaction(nameof(UpdateProducerAsync));
            await ExecuteCommandAsync(_mapper.Map(input, new UpdateProducerCommand(CurrentUser)), Token);
            return producerQueries.GetProducer(input.Id, CurrentUser);
        }

        public async Task<IQueryable<BusinessLegalDto>> CreateBusinessLegalsAsync(CreateBusinessLegalDto input,
            [Service] ILegalQueries legalQueries)
        {
            SetLogTransaction(nameof(CreateBusinessLegalsAsync));
            var result =
                await ExecuteCommandAsync<CreateBusinessLegalCommand, Guid>(
                    _mapper.Map(input, new CreateBusinessLegalCommand(CurrentUser)), Token);
            return legalQueries.GetBusinessLegals(result, CurrentUser);
        }

        public async Task<IQueryable<BusinessLegalDto>> UpdateBusinessLegalsAsync(UpdateBusinessLegalDto input,
            [Service] ILegalQueries legalQueries)
        {
            SetLogTransaction(nameof(UpdateBusinessLegalsAsync));
            await ExecuteCommandAsync(_mapper.Map(input, new UpdateBusinessLegalCommand(CurrentUser)), Token);
            return legalQueries.GetBusinessLegals(input.Id, CurrentUser);
        }

        public async Task<IQueryable<ConsumerDto>> RegisterConsumerAsync(RegisterConsumerDto input,
            [Service] IConsumerQueries consumerQueries)
        {
            SetLogTransaction(nameof(RegisterConsumerAsync));
            var result =
                await ExecuteCommandAsync<RegisterConsumerCommand, Guid>(
                    _mapper.Map(input, new RegisterConsumerCommand(CurrentUser) {ConsumerId = CurrentUser.Id}), Token);
            return consumerQueries.GetConsumer(result, CurrentUser);
        }

        public async Task<IQueryable<ConsumerDto>> UpdateConsumerAsync(UpdateConsumerDto input,
            [Service] IConsumerQueries consumerQueries)
        {
            SetLogTransaction(nameof(UpdateConsumerAsync));
            await ExecuteCommandAsync(_mapper.Map(input, new UpdateConsumerCommand(CurrentUser)), Token);
            return consumerQueries.GetConsumer(input.Id, CurrentUser);
        }

        public async Task<IQueryable<ConsumerLegalDto>> CreateConsumerLegalsAsync(CreateConsumerLegalDto input,
            [Service] ILegalQueries legalQueries)
        {
            SetLogTransaction(nameof(CreateConsumerLegalsAsync));
            var result =
                await ExecuteCommandAsync<CreateConsumerLegalCommand, Guid>(
                    _mapper.Map(input, new CreateConsumerLegalCommand(CurrentUser)), Token);
            return legalQueries.GetConsumerLegals(result, CurrentUser);
        }

        public async Task<IQueryable<ConsumerLegalDto>> UpdateConsumerLegalsAsync(UpdateConsumerLegalDto input,
            [Service] ILegalQueries legalQueries)
        {
            SetLogTransaction(nameof(UpdateConsumerLegalsAsync));
            await ExecuteCommandAsync(_mapper.Map(input, new UpdateConsumerLegalCommand(CurrentUser)), Token);
            return legalQueries.GetConsumerLegals(input.Id, CurrentUser);
        }

        public async Task<IQueryable<UserDto>> UpdateUserPictureAsync(UpdateResourceIdPictureDto input,
            [Service] IUserQueries userQueries)
        {
            SetLogTransaction(nameof(UpdateUserPictureAsync));
            await ExecuteCommandAsync<UpdateUserPictureCommand, string>(
                _mapper.Map(input, new UpdateUserPictureCommand(CurrentUser)), Token);
            return userQueries.GetUser(input.Id, CurrentUser);
        }

        public async Task<bool> AddPictureToUserProfileAsync(AddPictureToResourceIdDto input)
        {
            SetLogTransaction(nameof(AddPictureToUserProfileAsync));
            return await ExecuteCommandAsync(_mapper.Map(input, new AddPictureToUserProfileCommand(CurrentUser)),
                Token);
        }

        public async Task<bool> RemoveUserProfilePicturesAsync(ResourceIdsDto input)
        {
            SetLogTransaction(nameof(RemoveUserProfilePicturesAsync));
            return await ExecuteCommandAsync(
                _mapper.Map(input, new RemoveUserProfilePicturesCommand(CurrentUser) {UserId = CurrentUser.Id}), Token);
        }

        public async Task<bool> AddPictureToProductAsync(AddPictureToResourceIdDto input)
        {
            SetLogTransaction(nameof(AddPictureToProductAsync));
            return await ExecuteCommandAsync(_mapper.Map(input, new AddPictureToProductCommand(CurrentUser)), Token);
        }

        public async Task<bool> RemoveProductPicturesAsync(ResourceIdsDto input)
        {
            SetLogTransaction(nameof(RemoveProductPicturesAsync));
            return await ExecuteCommandAsync(_mapper.Map(input, new RemoveProductPicturesCommand(CurrentUser)), Token);
        }

        public async Task<bool> RemoveUserAsync(ResourceIdWithReasonDto input)
        {
            SetLogTransaction(nameof(RemoveUserAsync));
            return await ExecuteCommandAsync(_mapper.Map(input, new RemoveUserCommand(CurrentUser)), Token);
        }

        public async Task<IQueryable<QuickOrderDto>> CreateQuickOrderAsync(CreateQuickOrderDto input,
            [Service] IQuickOrderQueries quickOrderQueries)
        {
            SetLogTransaction(nameof(CreateQuickOrderAsync));
            var result =
                await ExecuteCommandAsync<CreateQuickOrderCommand, Guid>(
                    _mapper.Map(input, new CreateQuickOrderCommand(CurrentUser) {UserId = CurrentUser.Id}), Token);
            return quickOrderQueries.GetQuickOrder(result, CurrentUser);
        }

        public async Task<IQueryable<QuickOrderDto>> SetDefaultQuickOrderAsync(ResourceIdDto input,
            [Service] IQuickOrderQueries quickOrderQueries)
        {
            SetLogTransaction(nameof(SetDefaultQuickOrderAsync));
            await ExecuteCommandAsync(
                _mapper.Map(input, new SetDefaultQuickOrderCommand(CurrentUser) {UserId = CurrentUser.Id}), Token);
            return quickOrderQueries.GetQuickOrder(input.Id, CurrentUser);
        }

        public async Task<IQueryable<QuickOrderDto>> UpdateQuickOrderAsync(UpdateQuickOrderDto input,
            [Service] IQuickOrderQueries quickOrderQueries)
        {
            SetLogTransaction(nameof(UpdateQuickOrderAsync));
            await ExecuteCommandAsync(_mapper.Map(input, new UpdateQuickOrderCommand(CurrentUser)), Token);
            return quickOrderQueries.GetQuickOrder(input.Id, CurrentUser);
        }

        public async Task<IQueryable<QuickOrderDto>> UpdateQuickOrderProductsAsync(
            UpdateResourceIdProductsQuantitiesDto input, [Service] IQuickOrderQueries quickOrderQueries)
        {
            SetLogTransaction(nameof(UpdateQuickOrderProductsAsync));
            await ExecuteCommandAsync(_mapper.Map(input, new UpdateQuickOrderProductsCommand(CurrentUser)), Token);
            return quickOrderQueries.GetQuickOrder(input.Id, CurrentUser);
        }

        public async Task<bool> DeleteQuickOrdersAsync(ResourceIdsWithReasonDto input)
        {
            SetLogTransaction(nameof(DeleteQuickOrdersAsync));
            return await ExecuteCommandAsync(_mapper.Map(input, new DeleteQuickOrdersCommand(CurrentUser)), Token);
        }

        public async Task<IQueryable<DeliveryModeDto>> SetDeliveryModesAvailabilityAsync(
            SetResourceIdsAvailabilityDto input, [Service] IDeliveryQueries deliveryModeQueries)
        {
            SetLogTransaction(nameof(SetDeliveryModesAvailabilityAsync));
            await ExecuteCommandAsync(_mapper.Map(input, new SetDeliveryModesAvailabilityCommand(CurrentUser)), Token);
            return deliveryModeQueries.GetDeliveries(CurrentUser).Where(j => input.Ids.Contains(j.Id));
        }

        public async Task<IQueryable<DeliveryModeDto>> CreateDeliveryModeAsync(CreateDeliveryModeDto input,
            [Service] IDeliveryQueries deliveryQueries)
        {
            SetLogTransaction(nameof(CreateDeliveryModeAsync));
            var result =
                await ExecuteCommandAsync<CreateDeliveryModeCommand, Guid>(
                    _mapper.Map(input, new CreateDeliveryModeCommand(CurrentUser) {ProducerId = CurrentUser.Id}),
                    Token);
            return deliveryQueries.GetDelivery(result, CurrentUser);
        }

        public async Task<IQueryable<DeliveryModeDto>> UpdateDeliveryModeAsync(UpdateDeliveryModeDto input,
            [Service] IDeliveryQueries deliveryQueries)
        {
            SetLogTransaction(nameof(UpdateDeliveryModeAsync));
            await ExecuteCommandAsync(_mapper.Map(input, new UpdateDeliveryModeCommand(CurrentUser)), Token);
            return deliveryQueries.GetDelivery(input.Id, CurrentUser);
        }

        public async Task<bool> DeleteDeliveryModeAsync(ResourceIdDto input)
        {
            SetLogTransaction(nameof(DeleteDeliveryModeAsync));
            return await ExecuteCommandAsync(_mapper.Map(input, new DeleteDeliveryModeCommand(CurrentUser)), Token);
        }

        public async Task<IQueryable<ReturnableDto>> CreateReturnableAsync(CreateReturnableDto input,
            [Service] IReturnableQueries returnableQueries)
        {
            SetLogTransaction(nameof(CreateReturnableAsync));
            var result =
                await ExecuteCommandAsync<CreateReturnableCommand, Guid>(
                    _mapper.Map(input, new CreateReturnableCommand(CurrentUser) {UserId = CurrentUser.Id}), Token);
            return returnableQueries.GetReturnable(result, CurrentUser);
        }

        public async Task<IQueryable<ReturnableDto>> UpdateReturnableAsync(UpdateReturnableDto input,
            [Service] IReturnableQueries returnableQueries)
        {
            SetLogTransaction(nameof(UpdateReturnableAsync));
            await ExecuteCommandAsync(_mapper.Map(input, new UpdateReturnableCommand(CurrentUser)), Token);
            return returnableQueries.GetReturnable(input.Id, CurrentUser);
        }

        public async Task<bool> DeleteReturnableAsync(ResourceIdDto input)
        {
            SetLogTransaction(nameof(DeleteReturnableAsync));
            return await ExecuteCommandAsync(_mapper.Map(input, new DeleteReturnableCommand(CurrentUser)), Token);
        }
        
        //Closings
        public async Task<IQueryable<ClosingDto>> UpdateOrCreateBusinessClosingsAsync(UpdateOrCreateResourceIdClosingsDto input, [Service] IBusinessClosingQueries closingQueries)
        {
            SetLogTransaction(nameof(UpdateOrCreateBusinessClosingsAsync));
            var result = await ExecuteCommandAsync<UpdateOrCreateBusinessClosingsCommand, IEnumerable<Guid>>(_mapper.Map(input, new UpdateOrCreateBusinessClosingsCommand(CurrentUser)), Token);
            return closingQueries.GetClosings(CurrentUser).Where(c => result.Contains(c.Id));
        }
        public async Task<IQueryable<ClosingDto>> UpdateOrCreateBusinessClosingAsync(UpdateOrCreateResourceIdClosingDto input, [Service] IBusinessClosingQueries closingQueries)
        {
            SetLogTransaction(nameof(UpdateOrCreateBusinessClosingAsync));
            var result = await ExecuteCommandAsync<UpdateOrCreateBusinessClosingCommand, Guid>(_mapper.Map(input, new UpdateOrCreateBusinessClosingCommand(CurrentUser)), Token);
            return closingQueries.GetClosing(result, CurrentUser);
        }
        public async Task<IQueryable<ClosingDto>> UpdateOrCreateDeliveryClosingAsync(UpdateOrCreateResourceIdClosingDto input, [Service] IDeliveryClosingQueries closingQueries)
        {
            SetLogTransaction(nameof(UpdateOrCreateDeliveryClosingAsync));
            var result = await ExecuteCommandAsync<UpdateOrCreateDeliveryClosingCommand, Guid>(_mapper.Map(input, new UpdateOrCreateDeliveryClosingCommand(CurrentUser)), Token);
            return closingQueries.GetClosing(result, CurrentUser);
        }
        public async Task<bool> DeleteBusinessClosingsAsync(ResourceIdsDto input)
        {
            SetLogTransaction(nameof(DeleteBusinessClosingsAsync));
            return await ExecuteCommandAsync(_mapper.Map(input, new DeleteBusinessClosingsCommand(CurrentUser)), Token);
        }
        public async Task<bool> DeleteDeliveryClosingsAsync(ResourceIdsDto input)
        {
            SetLogTransaction(nameof(DeleteDeliveryClosingsAsync));
            return await ExecuteCommandAsync(_mapper.Map(input, new DeleteDeliveryClosingsCommand(CurrentUser)), Token);
        }

        private async Task<T> ExecuteCommandAsync<TU, T>(TU input, CancellationToken token) where TU : ICommand<T>
        {
            var result = await _mediator.Process(input, token);
            if (result.Succeeded)
                return result.Data;

            if (result.Exception != null)
                throw result.Exception;

            throw SheaftException.Unexpected(result.Message);
        }

        private async Task<bool> ExecuteCommandAsync<TU>(TU input, CancellationToken token) where TU : ICommand
        {
            var result = await _mediator.Process(input, token);
            if (result.Succeeded)
                return true;

            if (result.Exception != null)
                throw result.Exception;

            throw SheaftException.Unexpected(result.Message);
        }

        private void SetLogTransaction(string name)
        {
            NewRelic.Api.Agent.NewRelic.SetTransactionName("GraphQL", name);

            var currentTransaction = NewRelic.Api.Agent.NewRelic.GetAgent().CurrentTransaction;
            currentTransaction.AddCustomAttribute("RequestId", _httpContextAccessor.HttpContext.TraceIdentifier);
            currentTransaction.AddCustomAttribute("UserIdentifier", CurrentUser.Id.ToString("N"));
            currentTransaction.AddCustomAttribute("IsAuthenticated", CurrentUser.IsAuthenticated.ToString());
            currentTransaction.AddCustomAttribute("Roles", string.Join(";", CurrentUser.Roles));
            currentTransaction.AddCustomAttribute("GraphQL", name);
        }
    }
}
