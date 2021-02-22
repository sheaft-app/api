using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using HotChocolate;
using HotChocolate.Execution;
using Microsoft.AspNetCore.Http;
using Sheaft.Application.Agreement.Commands;
using Sheaft.Application.BusinessClosing.Commands;
using Sheaft.Application.Common.Extensions;
using Sheaft.Application.Common.Interfaces.Queries;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Models;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Application.Common.Models.Inputs;
using Sheaft.Application.Consumer.Commands;
using Sheaft.Application.DeliveryClosing.Commands;
using Sheaft.Application.DeliveryMode.Commands;
using Sheaft.Application.Document.Commands;
using Sheaft.Application.Job.Commands;
using Sheaft.Application.Legal.Commands;
using Sheaft.Application.Notification.Commands;
using Sheaft.Application.Order.Commands;
using Sheaft.Application.PickingOrders.Commands;
using Sheaft.Application.Picture.Commands;
using Sheaft.Application.Producer.Commands;
using Sheaft.Application.Product.Commands;
using Sheaft.Application.ProductClosing.Commands;
using Sheaft.Application.PurchaseOrder.Commands;
using Sheaft.Application.QuickOrder.Commands;
using Sheaft.Application.Returnable.Commands;
using Sheaft.Application.Store.Commands;
using Sheaft.Application.User.Commands;
using Sheaft.Domain;
using Sheaft.Domain.Exceptions;

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

        public async Task<string> GenerateUserSponsoringCodeAsync(IdInput input)
        {
            SetLogTransaction(nameof(GenerateUserSponsoringCodeAsync));
            return await ExecuteCommandAsync<GenerateUserCodeCommand, string>(
                _mapper.Map(input, new GenerateUserCodeCommand(CurrentUser)), Token);
        }

        public async Task<IQueryable<JobDto>> ExportPickingOrdersAsync(ExportPickingOrdersInput input,
            [Service] IJobQueries jobQueries)
        {
            SetLogTransaction(nameof(ExportPickingOrdersAsync));
            var result =
                await ExecuteCommandAsync<QueueExportPickingOrderCommand, Guid>(
                    _mapper.Map(input, new QueueExportPickingOrderCommand(CurrentUser) {ProducerId = CurrentUser.Id}), Token);
            return jobQueries.GetJob(result, CurrentUser);
        }

        public async Task<IQueryable<JobDto>> ExportUserDataAsync(IdInput input, [Service] IJobQueries jobQueries)
        {
            SetLogTransaction(nameof(ExportUserDataAsync));
            var result =
                await ExecuteCommandAsync<QueueExportUserDataCommand, Guid>(
                    _mapper.Map(input, new QueueExportUserDataCommand(CurrentUser)), Token);
            return jobQueries.GetJob(result, CurrentUser);
        }

        public async Task<IQueryable<JobDto>> ResumeJobsAsync(IdsInput input, [Service] IJobQueries jobQueries)
        {
            SetLogTransaction(nameof(ResumeJobsAsync));
            await ExecuteCommandAsync(_mapper.Map(input, new ResumeJobsCommand(CurrentUser)), Token);
            return jobQueries.GetJobs(CurrentUser).Where(j => input.Ids.Contains(j.Id));
        }

        public async Task<IQueryable<JobDto>> PauseJobsAsync(IdsInput input, [Service] IJobQueries jobQueries)
        {
            SetLogTransaction(nameof(PauseJobsAsync));
            await ExecuteCommandAsync(_mapper.Map(input, new PauseJobsCommand(CurrentUser)),Token);
            return jobQueries.GetJobs(CurrentUser).Where(j => input.Ids.Contains(j.Id));
        }

        public async Task<IQueryable<JobDto>> RetryJobsAsync(IdsInput input, [Service] IJobQueries jobQueries)
        {
            SetLogTransaction(nameof(RetryJobsAsync));
                await ExecuteCommandAsync(_mapper.Map(input, new RetryJobsCommand(CurrentUser)), Token);
            return jobQueries.GetJobs(CurrentUser).Where(j => input.Ids.Contains(j.Id));
        }

        public async Task<IQueryable<JobDto>> CancelJobsAsync(IdsWithReasonInput input,
            [Service] IJobQueries jobQueries)
        {
            SetLogTransaction(nameof(CancelJobsAsync));
                await ExecuteCommandAsync(_mapper.Map(input, new CancelJobsCommand(CurrentUser)), Token);
            return jobQueries.GetJobs(CurrentUser).Where(j => input.Ids.Contains(j.Id));
        }

        public async Task<IQueryable<JobDto>> ArchiveJobsAsync(IdsInput input, [Service] IJobQueries jobQueries)
        {
            SetLogTransaction(nameof(ArchiveJobsAsync));
                await ExecuteCommandAsync(_mapper.Map(input, new ArchiveJobsCommand(CurrentUser)), Token);
            return jobQueries.GetJobs(CurrentUser).Where(j => input.Ids.Contains(j.Id));
        }

        public async Task<IQueryable<AgreementDto>> CreateAgreementAsync(CreateAgreementInput input,
            [Service] IAgreementQueries agreementQueries)
        {
            SetLogTransaction(nameof(CreateAgreementAsync));
            var result =
                await ExecuteCommandAsync<CreateAgreementCommand, Guid>(
                    _mapper.Map(input, new CreateAgreementCommand(CurrentUser)), Token);
            return agreementQueries.GetAgreement(result, CurrentUser);
        }

        public async Task<IQueryable<AgreementDto>> AcceptAgreementAsync(IdTimeSlotGroupInput input,
            [Service] IAgreementQueries agreementQueries)
        {
            SetLogTransaction(nameof(AcceptAgreementAsync));
            await ExecuteCommandAsync(_mapper.Map(input, new AcceptAgreementCommand(CurrentUser)), Token);
            return agreementQueries.GetAgreement(input.Id, CurrentUser);
        }

        public async Task<IQueryable<AgreementDto>> CancelAgreementsAsync(IdsWithReasonInput input,
            [Service] IAgreementQueries agreementQueries)
        {
            SetLogTransaction(nameof(CancelAgreementsAsync));
            await ExecuteCommandAsync(_mapper.Map(input, new CancelAgreementsCommand(CurrentUser)), Token);
            return agreementQueries.GetAgreements(CurrentUser).Where(j => input.Ids.Contains(j.Id));
        }

        public async Task<IQueryable<AgreementDto>> RefuseAgreementsAsync(IdsWithReasonInput input,
            [Service] IAgreementQueries agreementQueries)
        {
            SetLogTransaction(nameof(RefuseAgreementsAsync));
            await ExecuteCommandAsync(_mapper.Map(input, new RefuseAgreementsCommand(CurrentUser)), Token);
            return agreementQueries.GetAgreements(CurrentUser).Where(j => input.Ids.Contains(j.Id));
        }

        public async Task<DateTimeOffset> MarkMyNotificationsAsReadAsync()
        {
            SetLogTransaction(nameof(MarkMyNotificationsAsReadAsync));
            var input = new MarkUserNotificationsAsReadCommand(CurrentUser) {UserId = CurrentUser.Id, ReadBefore = DateTimeOffset.UtcNow};
            await ExecuteCommandAsync(input, Token);
            return input.ReadBefore;
        }

        public async Task<IQueryable<NotificationDto>> MarkNotificationAsReadAsync(IdInput input,
            [Service] INotificationQueries notificationQueries)
        {
            SetLogTransaction(nameof(MarkNotificationAsReadAsync));
            await ExecuteCommandAsync(_mapper.Map(input, new MarkUserNotificationAsReadCommand(CurrentUser)), Token);
            return notificationQueries.GetNotification(input.Id, CurrentUser);
        }

        public async Task<IQueryable<OrderDto>> CreateOrderAsync(CreateOrderInput input,
            [Service] IOrderQueries orderQueries)
        {
            SetLogTransaction(nameof(CreateOrderAsync));
            var result =
                await ExecuteCommandAsync<CreateConsumerOrderCommand, Guid>(
                    _mapper.Map(input, new CreateConsumerOrderCommand(CurrentUser){UserId = CurrentUser.Id}), Token);
            return orderQueries.GetOrder(result, CurrentUser);
        }

        public async Task<IQueryable<OrderDto>> UpdateOrderAsync(UpdateOrderInput input,
            [Service] IOrderQueries orderQueries)
        {
            SetLogTransaction(nameof(UpdateOrderAsync));
            await ExecuteCommandAsync(_mapper.Map(input, new UpdateConsumerOrderCommand(CurrentUser)), Token);
            return orderQueries.GetOrder(input.Id, CurrentUser);
        }

        public async Task<IQueryable<WebPayinDto>> PayOrderAsync(IdInput input, [Service] IPayinQueries payinQueries)
        {
            SetLogTransaction(nameof(PayOrderAsync));
            var result =
                await ExecuteCommandAsync<PayOrderCommand, Guid>(_mapper.Map(input, new PayOrderCommand(CurrentUser)),
                    Token);
            return payinQueries.GetWebPayinTransaction(result, CurrentUser);
        }

        public async Task<IQueryable<PurchaseOrderDto>> CreateBusinessOrderAsync(CreateOrderInput input,
            [Service] IPurchaseOrderQueries purchaseOrderQueries)
        {
            SetLogTransaction(nameof(CreateBusinessOrderAsync));
            var result =
                await ExecuteCommandAsync<CreateBusinessOrderCommand, IEnumerable<Guid>>(
                    _mapper.Map(input, new CreateBusinessOrderCommand(CurrentUser) {UserId = CurrentUser.Id}), Token);
            return purchaseOrderQueries.GetPurchaseOrders(CurrentUser).Where(j => result.Contains(j.Id));
        }

        public async Task<IQueryable<DocumentDto>> CreateDocumentAsync(CreateDocumentInput input,
            [Service] IDocumentQueries documentQueries)
        {
            SetLogTransaction(nameof(CreateDocumentAsync));
            var result =
                await ExecuteCommandAsync<CreateDocumentCommand, Guid>(
                    _mapper.Map(input, new CreateDocumentCommand(CurrentUser)), Token);
            return documentQueries.GetDocument(result, CurrentUser);
        }

        public async Task<bool> RemoveDocumentAsync(IdInput input)
        {
            SetLogTransaction(nameof(RemoveDocumentAsync));
            return await ExecuteCommandAsync(_mapper.Map(input, new DeleteDocumentCommand(CurrentUser)), Token);
        }

        public async Task<IQueryable<PurchaseOrderDto>> AcceptPurchaseOrdersAsync(IdsInput input,
            [Service] IPurchaseOrderQueries purchaseOrderQueries)
        {
            SetLogTransaction(nameof(AcceptPurchaseOrdersAsync));
            await ExecuteCommandAsync(_mapper.Map(input, new AcceptPurchaseOrdersCommand(CurrentUser)), Token);
            return purchaseOrderQueries.GetPurchaseOrders(CurrentUser).Where(j => input.Ids.Contains(j.Id));
        }

        public async Task<IQueryable<PurchaseOrderDto>> ShipPurchaseOrdersAsync(IdsInput input,
            [Service] IPurchaseOrderQueries purchaseOrderQueries)
        {
            SetLogTransaction(nameof(ShipPurchaseOrdersAsync));
            await ExecuteCommandAsync(_mapper.Map(input, new ShipPurchaseOrdersCommand(CurrentUser)), Token);
            return purchaseOrderQueries.GetPurchaseOrders(CurrentUser).Where(j => input.Ids.Contains(j.Id));
        }

        public async Task<IQueryable<PurchaseOrderDto>> DeliverPurchaseOrdersAsync(IdsInput input,
            [Service] IPurchaseOrderQueries purchaseOrderQueries)
        {
            SetLogTransaction(nameof(DeliverPurchaseOrdersAsync));
            await ExecuteCommandAsync(_mapper.Map(input, new DeliverPurchaseOrdersCommand(CurrentUser)), Token);
            return purchaseOrderQueries.GetPurchaseOrders(CurrentUser).Where(j => input.Ids.Contains(j.Id));
        }

        public async Task<IQueryable<PurchaseOrderDto>> ProcessPurchaseOrdersAsync(IdsInput input,
            [Service] IPurchaseOrderQueries purchaseOrderQueries)
        {
            SetLogTransaction(nameof(ProcessPurchaseOrdersAsync));
            await ExecuteCommandAsync(_mapper.Map(input, new ProcessPurchaseOrdersCommand(CurrentUser)), Token);
            return purchaseOrderQueries.GetPurchaseOrders(CurrentUser).Where(j => input.Ids.Contains(j.Id));
        }

        public async Task<IQueryable<PurchaseOrderDto>> CompletePurchaseOrdersAsync(IdsInput input,
            [Service] IPurchaseOrderQueries purchaseOrderQueries)
        {
            SetLogTransaction(nameof(CompletePurchaseOrdersAsync));
            await ExecuteCommandAsync(_mapper.Map(input, new CompletePurchaseOrdersCommand(CurrentUser)), Token);
            return purchaseOrderQueries.GetPurchaseOrders(CurrentUser).Where(j => input.Ids.Contains(j.Id));
        }

        public async Task<IQueryable<PurchaseOrderDto>> CancelPurchaseOrdersAsync(IdsWithReasonInput input,
            [Service] IPurchaseOrderQueries purchaseOrderQueries)
        {
            SetLogTransaction(nameof(CancelPurchaseOrdersAsync));
            await ExecuteCommandAsync(_mapper.Map(input, new CancelPurchaseOrdersCommand(CurrentUser)), Token);
            return purchaseOrderQueries.GetPurchaseOrders(CurrentUser).Where(j => input.Ids.Contains(j.Id));
        }

        public async Task<IQueryable<PurchaseOrderDto>> RefusePurchaseOrdersAsync(IdsWithReasonInput input,
            [Service] IPurchaseOrderQueries purchaseOrderQueries)
        {
            SetLogTransaction(nameof(RefusePurchaseOrdersAsync));
            await ExecuteCommandAsync(_mapper.Map(input, new RefusePurchaseOrdersCommand(CurrentUser)), Token);
            return purchaseOrderQueries.GetPurchaseOrders(CurrentUser).Where(j => input.Ids.Contains(j.Id));
        }

        public async Task<bool> DeletePurchaseOrdersAsync(IdsInput input)
        {
            SetLogTransaction(nameof(DeletePurchaseOrdersAsync));
            return await ExecuteCommandAsync(_mapper.Map(input, new DeletePurchaseOrdersCommand(CurrentUser)), Token);
        }

        public async Task<IQueryable<ProductDto>> CreateProductAsync(CreateProductInput input,
            [Service] IProductQueries productQueries)
        {
            SetLogTransaction(nameof(CreateProductAsync));
            var result =
                await ExecuteCommandAsync<CreateProductCommand, Guid>(
                    _mapper.Map(input, new CreateProductCommand(CurrentUser) {ProducerId = CurrentUser.Id}), Token);
            return productQueries.GetProduct(result, CurrentUser);
        }

        public async Task<IQueryable<ProductDto>> UpdateProductAsync(UpdateProductInput input,
            [Service] IProductQueries productQueries)
        {
            SetLogTransaction(nameof(UpdateProductAsync));
            await ExecuteCommandAsync(_mapper.Map(input, new UpdateProductCommand(CurrentUser)), Token);
            return productQueries.GetProduct(input.Id, CurrentUser);
        }

        public async Task<IQueryable<ProductDto>> RateProductAsync(RateProductInput input,
            [Service] IProductQueries productQueries)
        {
            SetLogTransaction(nameof(RateProductAsync));
            await ExecuteCommandAsync(_mapper.Map(input, new RateProductCommand(CurrentUser){UserId = CurrentUser.Id}), Token);
            return productQueries.GetProduct(input.Id, CurrentUser);
        }

        public async Task<IQueryable<ProductDto>> UpdateProductPictureAsync(UpdatePictureInput input,
            [Service] IProductQueries productQueries)
        {
            SetLogTransaction(nameof(UpdateProductPictureAsync));
            await ExecuteCommandAsync<UpdateProductPreviewCommand, string>(
                _mapper.Map(input, new UpdateProductPreviewCommand(CurrentUser)), Token);
            return productQueries.GetProduct(input.Id, CurrentUser);
        }

        public async Task<IQueryable<ProductDto>> SetProductsAvailabilityAsync(SetProductsAvailabilityInput input,
            [Service] IProductQueries productQueries)
        {
            SetLogTransaction(nameof(SetProductsAvailabilityAsync));
            await ExecuteCommandAsync(_mapper.Map(input, new SetProductsAvailabilityCommand(CurrentUser)), Token);
            return productQueries.GetProducts(CurrentUser).Where(j => input.Ids.Contains(j.Id));
        }

        public async Task<IQueryable<ProductDto>> SetProductsSearchabilityAsync(SetProductsSearchabilityInput input,
            [Service] IProductQueries productQueries)
        {
            SetLogTransaction(nameof(SetProductsSearchabilityAsync));
            await ExecuteCommandAsync(_mapper.Map(input, new SetProductsSearchabilityCommand(CurrentUser)), Token);
            return productQueries.GetProducts(CurrentUser).Where(j => input.Ids.Contains(j.Id));
        }

        public async Task<bool> DeleteProductsAsync(IdsInput input)
        {
            SetLogTransaction(nameof(DeleteProductsAsync));
            return await ExecuteCommandAsync(_mapper.Map(input, new DeleteProductsCommand(CurrentUser)), Token);
        }

        public async Task<IQueryable<StoreDto>> RegisterStoreAsync(RegisterStoreInput input,
            [Service] IStoreQueries storeQueries)
        {
            SetLogTransaction(nameof(RegisterStoreAsync));
            var result =
                await ExecuteCommandAsync<RegisterStoreCommand, Guid>(
                    _mapper.Map(input, new RegisterStoreCommand(CurrentUser){StoreId = CurrentUser.Id}), Token);
            return storeQueries.GetStore(result, CurrentUser);
        }

        public async Task<IQueryable<StoreDto>> UpdateStoreAsync(UpdateStoreInput input,
            [Service] IStoreQueries storeQueries)
        {
            SetLogTransaction(nameof(UpdateStoreAsync));
            await ExecuteCommandAsync(_mapper.Map(input, new UpdateStoreCommand(CurrentUser)), Token);
            return storeQueries.GetStore(input.Id, CurrentUser);
        }

        public async Task<IQueryable<ProducerDto>> RegisterProducerAsync(RegisterProducerInput input,
            [Service] IProducerQueries producerQueries)
        {
            SetLogTransaction(nameof(RegisterProducerAsync));
            var result =
                await ExecuteCommandAsync<RegisterProducerCommand, Guid>(
                    _mapper.Map(input, new RegisterProducerCommand(CurrentUser){ProducerId =  CurrentUser.Id}), Token);
            return producerQueries.GetProducer<ProducerDto>(result, CurrentUser);
        }

        public async Task<IQueryable<ProducerDto>> UpdateProducerAsync(UpdateProducerInput input,
            [Service] IProducerQueries producerQueries)
        {
            SetLogTransaction(nameof(UpdateProducerAsync));
            await ExecuteCommandAsync(_mapper.Map(input, new UpdateProducerCommand(CurrentUser)), Token);
            return producerQueries.GetProducer<ProducerDto>(input.Id, CurrentUser);
        }

        public async Task<IQueryable<BusinessLegalDto>> CreateBusinessLegalsAsync(CreateBusinessLegalInput input,
            [Service] ILegalQueries legalQueries)
        {
            SetLogTransaction(nameof(CreateBusinessLegalsAsync));
            var result =
                await ExecuteCommandAsync<CreateBusinessLegalCommand, Guid>(
                    _mapper.Map(input, new CreateBusinessLegalCommand(CurrentUser)), Token);
            return legalQueries.GetBusinessLegals(result, CurrentUser);
        }

        public async Task<IQueryable<BusinessLegalDto>> UpdateBusinessLegalsAsync(UpdateBusinessLegalInput input,
            [Service] ILegalQueries legalQueries)
        {
            SetLogTransaction(nameof(UpdateBusinessLegalsAsync));
            await ExecuteCommandAsync(_mapper.Map(input, new UpdateBusinessLegalCommand(CurrentUser)), Token);
            return legalQueries.GetBusinessLegals(input.Id, CurrentUser);
        }

        public async Task<IQueryable<ConsumerDto>> RegisterConsumerAsync(RegisterConsumerInput input,
            [Service] IConsumerQueries consumerQueries)
        {
            SetLogTransaction(nameof(RegisterConsumerAsync));
            var result =
                await ExecuteCommandAsync<RegisterConsumerCommand, Guid>(
                    _mapper.Map(input, new RegisterConsumerCommand(CurrentUser) { ConsumerId = CurrentUser.Id}), Token);
            return consumerQueries.GetConsumer(result, CurrentUser);
        }

        public async Task<IQueryable<ConsumerDto>> UpdateConsumerAsync(UpdateConsumerInput input,
            [Service] IConsumerQueries consumerQueries)
        {
            SetLogTransaction(nameof(UpdateConsumerAsync));
            await ExecuteCommandAsync(_mapper.Map(input, new UpdateConsumerCommand(CurrentUser)), Token);
            return consumerQueries.GetConsumer(input.Id, CurrentUser);
        }

        public async Task<IQueryable<ConsumerLegalDto>> CreateConsumerLegalsAsync(CreateConsumerLegalInput input,
            [Service] ILegalQueries legalQueries)
        {
            SetLogTransaction(nameof(CreateConsumerLegalsAsync));
            var result =
                await ExecuteCommandAsync<CreateConsumerLegalCommand, Guid>(
                    _mapper.Map(input, new CreateConsumerLegalCommand(CurrentUser)), Token);
            return legalQueries.GetConsumerLegals(result, CurrentUser);
        }

        public async Task<IQueryable<ConsumerLegalDto>> UpdateConsumerLegalsAsync(UpdateConsumerLegalInput input,
            [Service] ILegalQueries legalQueries)
        {
            SetLogTransaction(nameof(UpdateConsumerLegalsAsync));
            await ExecuteCommandAsync(_mapper.Map(input, new UpdateConsumerLegalCommand(CurrentUser)), Token);
            return legalQueries.GetConsumerLegals(input.Id, CurrentUser);
        }

        public async Task<IQueryable<UserProfileDto>> UpdateUserPictureAsync(UpdatePictureInput input,
            [Service] IUserQueries userQueries)
        {
            SetLogTransaction(nameof(UpdateUserPictureAsync));
            await ExecuteCommandAsync<UpdateUserPictureCommand, string>(
                _mapper.Map(input, new UpdateUserPictureCommand(CurrentUser)), Token);
            return userQueries.GetUserProfile(input.Id, CurrentUser);
        }

        public async Task<IQueryable<ProfileInformationDto>> UpdateUserProfileAsync(UpdateUserProfileInput input,
            [Service] IUserQueries userQueries)
        {
            SetLogTransaction(nameof(UpdateUserProfileAsync));
            await ExecuteCommandAsync(
                _mapper.Map(input, new UpdateUserProfileCommand(CurrentUser)), Token);
            return userQueries.GetUserProfileInformation(input.Id, CurrentUser);
        }

        public async Task<bool> AddPictureToUserProfileAsync(AddPictureToUserProfileInput input)
        {
            SetLogTransaction(nameof(AddPictureToUserProfileAsync));
            return await ExecuteCommandAsync(_mapper.Map(input, new AddPictureToUserProfileCommand(CurrentUser)), Token);
        }

        public async Task<bool> RemoveUserProfilePictureAsync(IdInput input)
        {
            SetLogTransaction(nameof(RemoveUserProfilePictureAsync));
            return await ExecuteCommandAsync(_mapper.Map(input, new RemoveUserProfilePictureCommand(CurrentUser){UserId = CurrentUser.Id}), Token);
        }

        public async Task<bool> RemoveUserProfilePicturesAsync(IdsInput input)
        {
            SetLogTransaction(nameof(RemoveUserProfilePicturesAsync));
            return await ExecuteCommandAsync(_mapper.Map(input, new RemoveUserProfilePicturesCommand(CurrentUser){UserId = CurrentUser.Id}), Token);
        }

        public async Task<bool> RemoveUserAsync(IdWithReasonInput input)
        {
            SetLogTransaction(nameof(RemoveUserAsync));
            return await ExecuteCommandAsync(_mapper.Map(input, new RemoveUserCommand(CurrentUser)), Token);
        }

        public async Task<IQueryable<QuickOrderDto>> CreateQuickOrderAsync(CreateQuickOrderInput input,
            [Service] IQuickOrderQueries quickOrderQueries)
        {
            SetLogTransaction(nameof(CreateQuickOrderAsync));
            var result =
                await ExecuteCommandAsync<CreateQuickOrderCommand, Guid>(
                    _mapper.Map(input, new CreateQuickOrderCommand(CurrentUser){UserId = CurrentUser.Id}), Token);
            return quickOrderQueries.GetQuickOrder(result, CurrentUser);
        }

        public async Task<IQueryable<QuickOrderDto>> SetDefaultQuickOrderAsync(IdInput input,
            [Service] IQuickOrderQueries quickOrderQueries)
        {
            SetLogTransaction(nameof(SetDefaultQuickOrderAsync));
            await ExecuteCommandAsync(_mapper.Map(input, new SetDefaultQuickOrderCommand(CurrentUser){UserId = CurrentUser.Id}), Token);
            return quickOrderQueries.GetQuickOrder(input.Id, CurrentUser);
        }

        public async Task<IQueryable<QuickOrderDto>> UpdateQuickOrderAsync(UpdateQuickOrderInput input,
            [Service] IQuickOrderQueries quickOrderQueries)
        {
            SetLogTransaction(nameof(UpdateQuickOrderAsync));
            await ExecuteCommandAsync(_mapper.Map(input, new UpdateQuickOrderCommand(CurrentUser)), Token);
            return quickOrderQueries.GetQuickOrder(input.Id, CurrentUser);
        }

        public async Task<IQueryable<QuickOrderDto>> UpdateQuickOrderProductsAsync(
            UpdateIdProductsQuantitiesInput input, [Service] IQuickOrderQueries quickOrderQueries)
        {
            SetLogTransaction(nameof(UpdateQuickOrderProductsAsync));
            await ExecuteCommandAsync(_mapper.Map(input, new UpdateQuickOrderProductsCommand(CurrentUser)), Token);
            return quickOrderQueries.GetQuickOrder(input.Id, CurrentUser);
        }

        public async Task<bool> DeleteQuickOrdersAsync(IdsWithReasonInput input)
        {
            SetLogTransaction(nameof(DeleteQuickOrdersAsync));
            return await ExecuteCommandAsync(_mapper.Map(input, new DeleteQuickOrdersCommand(CurrentUser)), Token);
        }

        public async Task<IQueryable<DeliveryModeDto>> SetDeliveryModesAvailabilityAsync(
            SetDeliveryModesAvailabilityInput input, [Service] IDeliveryQueries deliveryModeQueries)
        {
            SetLogTransaction(nameof(SetProductsAvailabilityAsync));
            await ExecuteCommandAsync(_mapper.Map(input, new SetDeliveryModesAvailabilityCommand(CurrentUser)), Token);
            return deliveryModeQueries.GetDeliveries(CurrentUser).Where(j => input.Ids.Contains(j.Id));
        }

        public async Task<IQueryable<DeliveryModeDto>> CreateDeliveryModeAsync(CreateDeliveryModeInput input,
            [Service] IDeliveryQueries deliveryQueries)
        {
            SetLogTransaction(nameof(CreateDeliveryModeAsync));
            var result =
                await ExecuteCommandAsync<CreateDeliveryModeCommand, Guid>(
                    _mapper.Map(input, new CreateDeliveryModeCommand(CurrentUser) { ProducerId = CurrentUser.Id }), Token);
            return deliveryQueries.GetDelivery(result, CurrentUser);
        }

        public async Task<IQueryable<DeliveryModeDto>> UpdateDeliveryModeAsync(UpdateDeliveryModeInput input,
            [Service] IDeliveryQueries deliveryQueries)
        {
            SetLogTransaction(nameof(UpdateDeliveryModeAsync));
            await ExecuteCommandAsync(_mapper.Map(input, new UpdateDeliveryModeCommand(CurrentUser)), Token);
            return deliveryQueries.GetDelivery(input.Id, CurrentUser);
        }

        public async Task<bool> DeleteDeliveryModeAsync(IdInput input)
        {
            SetLogTransaction(nameof(DeleteDeliveryModeAsync));
            return await ExecuteCommandAsync(_mapper.Map(input, new DeleteDeliveryModeCommand(CurrentUser)), Token);
        }

        public async Task<IQueryable<ReturnableDto>> CreateReturnableAsync(CreateReturnableInput input,
            [Service] IReturnableQueries returnableQueries)
        {
            SetLogTransaction(nameof(CreateReturnableAsync));
            var result =
                await ExecuteCommandAsync<CreateReturnableCommand, Guid>(
                    _mapper.Map(input, new CreateReturnableCommand(CurrentUser){UserId = CurrentUser.Id}), Token);
            return returnableQueries.GetReturnable(result, CurrentUser);
        }

        public async Task<IQueryable<ReturnableDto>> UpdateReturnableAsync(UpdateReturnableInput input,
            [Service] IReturnableQueries returnableQueries)
        {
            SetLogTransaction(nameof(UpdateReturnableAsync));
            await ExecuteCommandAsync(_mapper.Map(input, new UpdateReturnableCommand(CurrentUser)), Token);
            return returnableQueries.GetReturnable(input.Id, CurrentUser);
        }

        public async Task<bool> DeleteReturnableAsync(IdInput input)
        {
            SetLogTransaction(nameof(DeleteReturnableAsync));
            return await ExecuteCommandAsync(_mapper.Map(input, new DeleteReturnableCommand(CurrentUser)), Token);
        }
        
        //Closings
        public async Task<IQueryable<ClosingDto>> CreateBusinessClosingsAsync(CreateBusinessClosingsInput input, [Service] IBusinessClosingQueries closingQueries)
        {
            SetLogTransaction(nameof(CreateBusinessClosingsAsync));
            var result = await ExecuteCommandAsync<CreateBusinessClosingsCommand, List<Guid>>(_mapper.Map(input, new CreateBusinessClosingsCommand(CurrentUser)), Token);
            return closingQueries.GetClosings(CurrentUser).Where(j => result.Contains(j.Id));
        }
        public async Task<IQueryable<ClosingDto>> CreateDeliveryClosingsAsync(CreateDeliveryClosingsInput input, [Service] IDeliveryClosingQueries closingQueries)
        {
            SetLogTransaction(nameof(CreateDeliveryClosingsAsync));
            var result = await ExecuteCommandAsync<CreateDeliveryClosingsCommand, List<Guid>>(_mapper.Map(input, new CreateDeliveryClosingsCommand(CurrentUser)), Token);
            return closingQueries.GetClosings(CurrentUser).Where(j => result.Contains(j.Id));
        }
        public async Task<IQueryable<ClosingDto>> CreateProductClosingsAsync(CreateProductClosingsInput input, [Service] IProductClosingQueries closingQueries)
        {
            SetLogTransaction(nameof(CreateProductClosingsAsync));
            var result = await ExecuteCommandAsync<CreateProductClosingsCommand, List<Guid>>(_mapper.Map(input, new CreateProductClosingsCommand(CurrentUser)), Token);
            return closingQueries.GetClosings(CurrentUser).Where(j => result.Contains(j.Id));
        }
        public async Task<IQueryable<ClosingDto>> UpdateBusinessClosingAsync(UpdateClosingInput input, [Service] IBusinessClosingQueries closingQueries)
        {
            SetLogTransaction(nameof(UpdateBusinessClosingAsync));
            await ExecuteCommandAsync(_mapper.Map(input, new UpdateBusinessClosingCommand(CurrentUser)), Token);
            return closingQueries.GetClosing(input.Id, CurrentUser);
        }
        public async Task<IQueryable<ClosingDto>> UpdateDeliveryClosingAsync(UpdateClosingInput input, [Service] IDeliveryClosingQueries closingQueries)
        {
            SetLogTransaction(nameof(UpdateDeliveryClosingAsync));
            await ExecuteCommandAsync(_mapper.Map(input, new UpdateDeliveryClosingCommand(CurrentUser)), Token);
            return closingQueries.GetClosing(input.Id, CurrentUser);
        }
        public async Task<IQueryable<ClosingDto>> UpdateProductClosingAsync(UpdateClosingInput input, [Service] IProductClosingQueries closingQueries)
        {
            SetLogTransaction(nameof(UpdateProductClosingAsync));
            await ExecuteCommandAsync(_mapper.Map(input, new UpdateProductClosingCommand(CurrentUser)), Token);
            return closingQueries.GetClosing(input.Id, CurrentUser);
        }
        public async Task<bool> DeleteBusinessClosingsAsync(IdsInput input)
        {
            SetLogTransaction(nameof(DeleteBusinessClosingsAsync));
            return await ExecuteCommandAsync(_mapper.Map(input, new DeleteBusinessClosingsCommand(CurrentUser)), Token);
        }
        public async Task<bool> DeleteDeliveryClosingsAsync(IdsInput input)
        {
            SetLogTransaction(nameof(DeleteDeliveryClosingsAsync));
            return await ExecuteCommandAsync(_mapper.Map(input, new DeleteDeliveryClosingsCommand(CurrentUser)), Token);
        }
        public async Task<bool> DeleteProductClosingsAsync(IdsInput input)
        {
            SetLogTransaction(nameof(UpdateProductClosingAsync));
            return await ExecuteCommandAsync(_mapper.Map(input, new DeleteProductClosingsCommand(CurrentUser)), Token);
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