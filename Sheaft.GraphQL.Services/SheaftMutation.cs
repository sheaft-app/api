using MediatR;
using Sheaft.Application.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Sheaft.Application.Queries;
using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Core.Extensions;
using HotChocolate;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Sheaft.Exceptions;
using Sheaft.Core;
using Sheaft.Application.Interop;

namespace Sheaft.GraphQL.Services
{
    public class SheaftMutation
    {
        private readonly ISheaftMediatr _mediator;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<SheaftMutation> _logger;
        private CancellationToken Token => _httpContextAccessor.HttpContext.RequestAborted;
        private RequestUser CurrentUser
        {
            get
            {
                if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
                    return _httpContextAccessor.HttpContext.User.ToIdentityUser(_httpContextAccessor.HttpContext.TraceIdentifier);
                else
                    return new RequestUser(_httpContextAccessor.HttpContext.TraceIdentifier);
            }
        }

        public SheaftMutation(
            ISheaftMediatr mediator,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            ILogger<SheaftMutation> logger)
        {
            _mediator = mediator;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<string> GenerateUserSponsoringCodeAsync(IdInput input)
        {
            SetLogTransaction("GraphQL", nameof(GenerateUserSponsoringCodeAsync));
            return await ExecuteCommandAsync<GenerateUserCodeCommand, string>(_mapper.Map(input, new GenerateUserCodeCommand(CurrentUser)), Token);
        }

        public async Task<IQueryable<JobDto>> ExportPickingOrdersAsync(ExportPickingOrdersInput input, [Service] IJobQueries jobQueries)
        {
            SetLogTransaction("GraphQL", nameof(ExportPickingOrdersAsync));
            var result = await ExecuteCommandAsync<QueueExportPickingOrderCommand, Guid>(_mapper.Map(input, new QueueExportPickingOrderCommand(CurrentUser)), Token);
            return jobQueries.GetJob(result, CurrentUser);
        }

        public async Task<IQueryable<JobDto>> ExportUserDataAsync(IdInput input, [Service] IJobQueries jobQueries)
        {
            SetLogTransaction("GraphQL", nameof(ExportUserDataAsync));
            var result = await ExecuteCommandAsync<QueueExportUserDataCommand, Guid>(_mapper.Map(input, new QueueExportUserDataCommand(CurrentUser)), Token);
            return jobQueries.GetJob(result, CurrentUser);
        }

        public async Task<IQueryable<JobDto>> ResumeJobsAsync(IdsInput input, [Service] IJobQueries jobQueries)
        {
            SetLogTransaction("GraphQL", nameof(ResumeJobsAsync));
            var result = await ExecuteCommandAsync<ResumeJobsCommand, bool>(_mapper.Map(input, new ResumeJobsCommand(CurrentUser)), Token);
            return jobQueries.GetJobs(CurrentUser).Where(j => input.Ids.Contains(j.Id));
        }

        public async Task<IQueryable<JobDto>> PauseJobsAsync(IdsInput input, [Service] IJobQueries jobQueries)
        {
            SetLogTransaction("GraphQL", nameof(PauseJobsAsync));
            var result = await ExecuteCommandAsync<PauseJobsCommand, bool>(_mapper.Map(input, new PauseJobsCommand(CurrentUser)), Token);
            return jobQueries.GetJobs(CurrentUser).Where(j => input.Ids.Contains(j.Id));
        }

        public async Task<IQueryable<JobDto>> RetryJobsAsync(IdsInput input, [Service] IJobQueries jobQueries)
        {
            SetLogTransaction("GraphQL", nameof(RetryJobsAsync));
            var result = await ExecuteCommandAsync<RetryJobsCommand, bool>(_mapper.Map(input, new RetryJobsCommand(CurrentUser)), Token);
            return jobQueries.GetJobs(CurrentUser).Where(j => input.Ids.Contains(j.Id));
        }

        public async Task<IQueryable<JobDto>> CancelJobsAsync(IdsWithReasonInput input, [Service] IJobQueries jobQueries)
        {
            SetLogTransaction("GraphQL", nameof(CancelJobsAsync));
            var result = await ExecuteCommandAsync<CancelJobsCommand, bool>(_mapper.Map(input, new CancelJobsCommand(CurrentUser)), Token);
            return jobQueries.GetJobs(CurrentUser).Where(j => input.Ids.Contains(j.Id));
        }

        public async Task<IQueryable<JobDto>> ArchiveJobsAsync(IdsInput input, [Service] IJobQueries jobQueries)
        {
            SetLogTransaction("GraphQL", nameof(ArchiveJobsAsync));
            var result = await ExecuteCommandAsync<ArchiveJobsCommand, bool>(_mapper.Map(input, new ArchiveJobsCommand(CurrentUser)), Token);
            return jobQueries.GetJobs(CurrentUser).Where(j => input.Ids.Contains(j.Id));
        }

        public async Task<IQueryable<AgreementDto>> CreateAgreementAsync(CreateAgreementInput input, [Service] IAgreementQueries agreementQueries)
        {
            SetLogTransaction("GraphQL", nameof(CreateAgreementAsync));
            var result = await ExecuteCommandAsync<CreateAgreementCommand, Guid>(_mapper.Map(input, new CreateAgreementCommand(CurrentUser)), Token);
            return agreementQueries.GetAgreement(result, CurrentUser);
        }

        public async Task<IQueryable<AgreementDto>> AcceptAgreementAsync(IdTimeSlotGroupInput input, [Service] IAgreementQueries agreementQueries)
        {
            SetLogTransaction("GraphQL", nameof(AcceptAgreementAsync));
            await ExecuteCommandAsync<AcceptAgreementCommand, bool>(_mapper.Map(input, new AcceptAgreementCommand(CurrentUser)), Token);
            return agreementQueries.GetAgreement(input.Id, CurrentUser);
        }

        public async Task<IQueryable<AgreementDto>> CancelAgreementsAsync(IdsWithReasonInput input, [Service] IAgreementQueries agreementQueries)
        {
            SetLogTransaction("GraphQL", nameof(CancelAgreementsAsync));
            await ExecuteCommandAsync<CancelAgreementsCommand, bool>(_mapper.Map(input, new CancelAgreementsCommand(CurrentUser)), Token);
            return agreementQueries.GetAgreements(CurrentUser).Where(j => input.Ids.Contains(j.Id));
        }

        public async Task<IQueryable<AgreementDto>> RefuseAgreementsAsync(IdsWithReasonInput input, [Service] IAgreementQueries agreementQueries)
        {
            SetLogTransaction("GraphQL", nameof(RefuseAgreementsAsync));
            await ExecuteCommandAsync<RefuseAgreementsCommand, bool>(_mapper.Map(input, new RefuseAgreementsCommand(CurrentUser)), Token);
            return agreementQueries.GetAgreements(CurrentUser).Where(j => input.Ids.Contains(j.Id));
        }

        public async Task<DateTimeOffset> MarkMyNotificationsAsReadAsync()
        {
            SetLogTransaction("GraphQL", nameof(MarkMyNotificationsAsReadAsync));
            var input = new MarkUserNotificationsAsReadCommand(CurrentUser) { ReadBefore = DateTimeOffset.UtcNow };
            await ExecuteCommandAsync<MarkUserNotificationsAsReadCommand, bool>(input, Token);
            return input.ReadBefore;
        }

        public async Task<IQueryable<NotificationDto>> MarkNotificationAsReadAsync(IdInput input, [Service] INotificationQueries notificationQueries)
        {
            SetLogTransaction("GraphQL", nameof(MarkNotificationAsReadAsync));
            await ExecuteCommandAsync<MarkUserNotificationAsReadCommand, bool>(_mapper.Map(input, new MarkUserNotificationAsReadCommand(CurrentUser)), Token);
            return notificationQueries.GetNotification(input.Id, CurrentUser);
        }

        public async Task<IQueryable<OrderDto>> CreateOrderAsync(CreateOrderInput input, [Service] IOrderQueries orderQueries)
        {
            SetLogTransaction("GraphQL", nameof(CreateOrderAsync));
            var result = await ExecuteCommandAsync<CreateConsumerOrderCommand, Guid>(_mapper.Map(input, new CreateConsumerOrderCommand(CurrentUser)), Token);
            return orderQueries.GetOrder(result, CurrentUser);
        }

        public async Task<IQueryable<OrderDto>> UpdateOrderAsync(UpdateOrderInput input, [Service] IOrderQueries orderQueries)
        {
            SetLogTransaction("GraphQL", nameof(UpdateOrderAsync));
            await ExecuteCommandAsync<UpdateConsumerOrderCommand, bool>(_mapper.Map(input, new UpdateConsumerOrderCommand(CurrentUser)), Token);
            return orderQueries.GetOrder(input.Id, CurrentUser);
        }

        public async Task<IQueryable<WebPayinDto>> PayOrderAsync(IdInput input, [Service] ITransactionQueries transactionQueries)
        {
            SetLogTransaction("GraphQL", nameof(PayOrderAsync));
            var result = await ExecuteCommandAsync<PayOrderCommand, Guid>(_mapper.Map(input, new PayOrderCommand(CurrentUser)), Token);
            return transactionQueries.GetWebPayinTransaction(result, CurrentUser);
        }

        public async Task<IQueryable<PurchaseOrderDto>> CreateBusinessOrderAsync(CreateOrderInput input, [Service] IPurchaseOrderQueries purchaseOrderQueries)
        {
            SetLogTransaction("GraphQL", nameof(CreateBusinessOrderAsync));
            var result = await ExecuteCommandAsync<CreateBusinessOrderCommand, IEnumerable<Guid>>(_mapper.Map(input, new CreateBusinessOrderCommand(CurrentUser)), Token);
            return purchaseOrderQueries.GetPurchaseOrders(CurrentUser).Where(j => result.Contains(j.Id));
        }

        public async Task<IQueryable<DocumentDto>> CreateDocumentAsync(CreateDocumentInput input, [Service] IDocumentQueries documentQueries)
        {
            SetLogTransaction("GraphQL", nameof(CreateDocumentAsync));
            var result = await ExecuteCommandAsync<CreateDocumentCommand, Guid>(_mapper.Map(input, new CreateDocumentCommand(CurrentUser)), Token);
            return documentQueries.GetDocument(result, CurrentUser);
        }

        public async Task<bool> RemoveDocumentAsync(IdInput input)
        {
            SetLogTransaction("GraphQL", nameof(RemoveDocumentAsync));
            return await ExecuteCommandAsync<DeleteDocumentCommand, bool>(_mapper.Map(input, new DeleteDocumentCommand(CurrentUser)), Token);
        }

        public async Task<IQueryable<PurchaseOrderDto>> AcceptPurchaseOrdersAsync(IdsInput input, [Service] IPurchaseOrderQueries purchaseOrderQueries)
        {
            SetLogTransaction("GraphQL", nameof(AcceptPurchaseOrdersAsync));
            await ExecuteCommandAsync<AcceptPurchaseOrdersCommand, bool>(_mapper.Map(input, new AcceptPurchaseOrdersCommand(CurrentUser)), Token);
            return purchaseOrderQueries.GetPurchaseOrders(CurrentUser).Where(j => input.Ids.Contains(j.Id));
        }

        public async Task<IQueryable<PurchaseOrderDto>> ShipPurchaseOrdersAsync(IdsInput input, [Service] IPurchaseOrderQueries purchaseOrderQueries)
        {
            SetLogTransaction("GraphQL", nameof(ShipPurchaseOrdersAsync));
            await ExecuteCommandAsync<ShipPurchaseOrdersCommand, bool>(_mapper.Map(input, new ShipPurchaseOrdersCommand(CurrentUser)), Token);
            return purchaseOrderQueries.GetPurchaseOrders(CurrentUser).Where(j => input.Ids.Contains(j.Id));
        }

        public async Task<IQueryable<PurchaseOrderDto>> DeliverPurchaseOrdersAsync(IdsInput input, [Service] IPurchaseOrderQueries purchaseOrderQueries)
        {
            SetLogTransaction("GraphQL", nameof(DeliverPurchaseOrdersAsync));
            await ExecuteCommandAsync<DeliverPurchaseOrdersCommand, bool>(_mapper.Map(input, new DeliverPurchaseOrdersCommand(CurrentUser)), Token);
            return purchaseOrderQueries.GetPurchaseOrders(CurrentUser).Where(j => input.Ids.Contains(j.Id));
        }

        public async Task<IQueryable<PurchaseOrderDto>> ProcessPurchaseOrdersAsync(IdsInput input, [Service] IPurchaseOrderQueries purchaseOrderQueries)
        {
            SetLogTransaction("GraphQL", nameof(ProcessPurchaseOrdersAsync));
            await ExecuteCommandAsync<ProcessPurchaseOrdersCommand, bool>(_mapper.Map(input, new ProcessPurchaseOrdersCommand(CurrentUser)), Token);
            return purchaseOrderQueries.GetPurchaseOrders(CurrentUser).Where(j => input.Ids.Contains(j.Id));
        }

        public async Task<IQueryable<PurchaseOrderDto>> CompletePurchaseOrdersAsync(IdsInput input, [Service] IPurchaseOrderQueries purchaseOrderQueries)
        {
            SetLogTransaction("GraphQL", nameof(CompletePurchaseOrdersAsync));
            await ExecuteCommandAsync<CompletePurchaseOrdersCommand, bool>(_mapper.Map(input, new CompletePurchaseOrdersCommand(CurrentUser)), Token);
            return purchaseOrderQueries.GetPurchaseOrders(CurrentUser).Where(j => input.Ids.Contains(j.Id));
        }

        public async Task<IQueryable<PurchaseOrderDto>> CancelPurchaseOrdersAsync(IdsWithReasonInput input, [Service] IPurchaseOrderQueries purchaseOrderQueries)
        {
            SetLogTransaction("GraphQL", nameof(CancelPurchaseOrdersAsync));
            await ExecuteCommandAsync<CancelPurchaseOrdersCommand, bool>(_mapper.Map(input, new CancelPurchaseOrdersCommand(CurrentUser)), Token);
            return purchaseOrderQueries.GetPurchaseOrders(CurrentUser).Where(j => input.Ids.Contains(j.Id));
        }

        public async Task<IQueryable<PurchaseOrderDto>> RefusePurchaseOrdersAsync(IdsWithReasonInput input, [Service] IPurchaseOrderQueries purchaseOrderQueries)
        {
            SetLogTransaction("GraphQL", nameof(RefusePurchaseOrdersAsync));
            await ExecuteCommandAsync<RefusePurchaseOrdersCommand, bool>(_mapper.Map(input, new RefusePurchaseOrdersCommand(CurrentUser)), Token);
            return purchaseOrderQueries.GetPurchaseOrders(CurrentUser).Where(j => input.Ids.Contains(j.Id));
        }

        public async Task<bool> DeletePurchaseOrdersAsync(IdsInput input)
        {
            SetLogTransaction("GraphQL", nameof(DeletePurchaseOrdersAsync));
            return await ExecuteCommandAsync<DeletePurchaseOrdersCommand, bool>(_mapper.Map(input, new DeletePurchaseOrdersCommand(CurrentUser)), Token);
        }

        public async Task<IQueryable<ProductDto>> CreateProductAsync(CreateProductInput input, [Service] IProductQueries productQueries)
        {
            SetLogTransaction("GraphQL", nameof(CreateProductAsync));
            var result = await ExecuteCommandAsync<CreateProductCommand, Guid>(_mapper.Map(input, new CreateProductCommand(CurrentUser)), Token);
            return productQueries.GetProduct(result, CurrentUser);
        }

        public async Task<IQueryable<ProductDto>> UpdateProductAsync(UpdateProductInput input, [Service] IProductQueries productQueries)
        {
            SetLogTransaction("GraphQL", nameof(UpdateProductAsync));
            await ExecuteCommandAsync<UpdateProductCommand, bool>(_mapper.Map(input, new UpdateProductCommand(CurrentUser)), Token);
            return productQueries.GetProduct(input.Id, CurrentUser);
        }

        public async Task<IQueryable<ProductDto>> RateProductAsync(RateProductInput input, [Service] IProductQueries productQueries)
        {
            SetLogTransaction("GraphQL", nameof(RateProductAsync));
            await ExecuteCommandAsync<RateProductCommand, bool>(_mapper.Map(input, new RateProductCommand(CurrentUser)), Token);
            return productQueries.GetProduct(input.Id, CurrentUser);
        }

        public async Task<IQueryable<ProductDto>> UpdateProductPictureAsync(UpdatePictureInput input, [Service] IProductQueries productQueries)
        {
            SetLogTransaction("GraphQL", nameof(UpdateProductPictureAsync));
            await ExecuteCommandAsync<UpdateProductPictureCommand, string>(_mapper.Map(input, new UpdateProductPictureCommand(CurrentUser)), Token);
            return productQueries.GetProduct(input.Id, CurrentUser);
        }

        public async Task<IQueryable<ProductDto>> SetProductsAvailabilityAsync(SetProductsAvailabilityInput input, [Service] IProductQueries productQueries)
        {
            SetLogTransaction("GraphQL", nameof(SetProductsAvailabilityAsync));
            await ExecuteCommandAsync<SetProductsAvailabilityCommand, bool>(_mapper.Map(input, new SetProductsAvailabilityCommand(CurrentUser)), Token);
            return productQueries.GetProducts(CurrentUser).Where(j => input.Ids.Contains(j.Id));
        }

        public async Task<bool> DeleteProductsAsync(IdsInput input)
        {
            SetLogTransaction("GraphQL", nameof(DeleteProductsAsync));
            return await ExecuteCommandAsync<DeleteProductsCommand, bool>(_mapper.Map(input, new DeleteProductsCommand(CurrentUser)), Token);
        }

        public async Task<IQueryable<StoreDto>> RegisterStoreAsync(RegisterStoreInput input, [Service] IBusinessQueries businessQueries)
        {
            SetLogTransaction("GraphQL", nameof(RegisterStoreAsync));
            var result = await ExecuteCommandAsync<RegisterStoreCommand, Guid>(_mapper.Map(input, new RegisterStoreCommand(CurrentUser)), Token);
            return businessQueries.GetStore(result, CurrentUser);
        }

        public async Task<IQueryable<StoreDto>> UpdateStoreAsync(UpdateStoreInput input, [Service] IBusinessQueries businessQueries)
        {
            SetLogTransaction("GraphQL", nameof(UpdateStoreAsync));
            await ExecuteCommandAsync<UpdateStoreCommand, bool>(_mapper.Map(input, new UpdateStoreCommand(CurrentUser)), Token);
            return businessQueries.GetStore(input.Id, CurrentUser);
        }

        public async Task<IQueryable<ProducerDto>> RegisterProducerAsync(RegisterProducerInput input, [Service] IBusinessQueries businessQueries)
        {
            SetLogTransaction("GraphQL", nameof(RegisterProducerAsync));
            var result = await ExecuteCommandAsync<RegisterProducerCommand, Guid>(_mapper.Map(input, new RegisterProducerCommand(CurrentUser)), Token);
            return businessQueries.GetProducer(result, CurrentUser);
        }

        public async Task<IQueryable<ProducerDto>> UpdateProducerAsync(UpdateProducerInput input, [Service] IBusinessQueries businessQueries)
        {
            SetLogTransaction("GraphQL", nameof(UpdateProducerAsync));
            await ExecuteCommandAsync<UpdateProducerCommand, bool>(_mapper.Map(input, new UpdateProducerCommand(CurrentUser)), Token);
            return businessQueries.GetProducer(input.Id, CurrentUser);
        }

        public async Task<IQueryable<BusinessLegalDto>> CreateBusinessLegalsAsync(CreateBusinessLegalInput input, [Service] ILegalQueries legalQueries)
        {
            SetLogTransaction("GraphQL", nameof(CreateBusinessLegalsAsync));
            var result = await ExecuteCommandAsync<CreateBusinessLegalCommand, Guid>(_mapper.Map(input, new CreateBusinessLegalCommand(CurrentUser)), Token);
            return legalQueries.GetBusinessLegals(result, CurrentUser);
        }

        public async Task<IQueryable<BusinessLegalDto>> UpdateBusinessLegalsAsync(UpdateBusinessLegalInput input, [Service] ILegalQueries legalQueries)
        {
            SetLogTransaction("GraphQL", nameof(UpdateBusinessLegalsAsync));
            await ExecuteCommandAsync<UpdateBusinessLegalCommand, bool>(_mapper.Map(input, new UpdateBusinessLegalCommand(CurrentUser)), Token);
            return legalQueries.GetBusinessLegals(input.Id, CurrentUser);
        }

        public async Task<bool> RemoveUboAsync(IdInput input)
        {
            SetLogTransaction("GraphQL", nameof(RemoveUboAsync));
            return await ExecuteCommandAsync<DeleteUboCommand, bool>(_mapper.Map(input, new DeleteUboCommand(CurrentUser)), Token);
        }

        public async Task<IQueryable<ConsumerDto>> RegisterConsumerAsync(RegisterConsumerInput input, [Service] IConsumerQueries consumerQueries)
        {
            SetLogTransaction("GraphQL", nameof(RegisterConsumerAsync));
            var result = await ExecuteCommandAsync<RegisterConsumerCommand, Guid>(_mapper.Map(input, new RegisterConsumerCommand(CurrentUser)), Token);
            return consumerQueries.GetConsumer(result, CurrentUser);
        }

        public async Task<IQueryable<ConsumerDto>> UpdateConsumerAsync(UpdateConsumerInput input, [Service] IConsumerQueries consumerQueries)
        {
            SetLogTransaction("GraphQL", nameof(UpdateConsumerAsync));
            await ExecuteCommandAsync<UpdateConsumerCommand, bool>(_mapper.Map(input, new UpdateConsumerCommand(CurrentUser)), Token);
            return consumerQueries.GetConsumer(input.Id, CurrentUser);
        }

        public async Task<IQueryable<ConsumerLegalDto>> CreateConsumerLegalsAsync(CreateConsumerLegalInput input, [Service] ILegalQueries legalQueries)
        {
            SetLogTransaction("GraphQL", nameof(CreateConsumerLegalsAsync));
            var result = await ExecuteCommandAsync<CreateConsumerLegalCommand, Guid>(_mapper.Map(input, new CreateConsumerLegalCommand(CurrentUser)), Token);
            return legalQueries.GetConsumerLegals(result, CurrentUser);
        }

        public async Task<IQueryable<ConsumerLegalDto>> UpdateConsumerLegalsAsync(UpdateConsumerLegalInput input, [Service] ILegalQueries legalQueries)
        {
            SetLogTransaction("GraphQL", nameof(UpdateConsumerLegalsAsync));
            await ExecuteCommandAsync<UpdateConsumerLegalCommand, bool>(_mapper.Map(input, new UpdateConsumerLegalCommand(CurrentUser)), Token);
            return legalQueries.GetConsumerLegals(input.Id, CurrentUser);
        }

        public async Task<IQueryable<UserProfileDto>> UpdateUserPictureAsync(UpdatePictureInput input, [Service] IUserQueries userQueries)
        {
            SetLogTransaction("GraphQL", nameof(UpdateUserPictureAsync));
            await ExecuteCommandAsync<UpdateUserPictureCommand, string>(_mapper.Map(input, new UpdateUserPictureCommand(CurrentUser)), Token);
            return userQueries.GetUserProfile(input.Id, CurrentUser);
        }

        public async Task<bool> RemoveUserAsync(IdWithReasonInput input)
        {
            SetLogTransaction("GraphQL", nameof(RemoveUserAsync));
            return await ExecuteCommandAsync<RemoveUserCommand, bool>(_mapper.Map(input, new RemoveUserCommand(CurrentUser)), Token);
        }

        public async Task<IQueryable<QuickOrderDto>> CreateQuickOrderAsync(CreateQuickOrderInput input, [Service] IQuickOrderQueries quickOrderQueries)
        {
            SetLogTransaction("GraphQL", nameof(CreateQuickOrderAsync));
            var result = await ExecuteCommandAsync<CreateQuickOrderCommand, Guid>(_mapper.Map(input, new CreateQuickOrderCommand(CurrentUser)), Token);
            return quickOrderQueries.GetQuickOrder(result, CurrentUser);
        }

        public async Task<IQueryable<QuickOrderDto>> SetDefaultQuickOrderAsync(IdInput input, [Service] IQuickOrderQueries quickOrderQueries)
        {
            SetLogTransaction("GraphQL", nameof(SetDefaultQuickOrderAsync));
            await ExecuteCommandAsync<SetDefaultQuickOrderCommand, bool>(_mapper.Map(input, new SetDefaultQuickOrderCommand(CurrentUser)), Token);
            return quickOrderQueries.GetQuickOrder(input.Id, CurrentUser);
        }

        public async Task<IQueryable<QuickOrderDto>> UpdateQuickOrderAsync(UpdateQuickOrderInput input, [Service] IQuickOrderQueries quickOrderQueries)
        {
            SetLogTransaction("GraphQL", nameof(UpdateQuickOrderAsync));
            await ExecuteCommandAsync<UpdateQuickOrderCommand, bool>(_mapper.Map(input, new UpdateQuickOrderCommand(CurrentUser)), Token);
            return quickOrderQueries.GetQuickOrder(input.Id, CurrentUser);
        }

        public async Task<IQueryable<QuickOrderDto>> UpdateQuickOrderProductsAsync(UpdateIdProductsQuantitiesInput input, [Service] IQuickOrderQueries quickOrderQueries)
        {
            SetLogTransaction("GraphQL", nameof(UpdateQuickOrderProductsAsync));
            await ExecuteCommandAsync<UpdateQuickOrderProductsCommand, bool>(_mapper.Map(input, new UpdateQuickOrderProductsCommand(CurrentUser)), Token);
            return quickOrderQueries.GetQuickOrder(input.Id, CurrentUser);
        }

        public async Task<bool> DeleteQuickOrdersAsync(IdsWithReasonInput input)
        {
            SetLogTransaction("GraphQL", nameof(DeleteQuickOrdersAsync));
            return await ExecuteCommandAsync<DeleteQuickOrdersCommand, bool>(_mapper.Map(input, new DeleteQuickOrdersCommand(CurrentUser)), Token);
        }

        public async Task<IQueryable<DeliveryModeDto>> CreateDeliveryModeAsync(CreateDeliveryModeInput input, [Service] IDeliveryQueries deliveryQueries)
        {
            SetLogTransaction("GraphQL", nameof(CreateDeliveryModeAsync));
            var result = await ExecuteCommandAsync<CreateDeliveryModeCommand, Guid>(_mapper.Map(input, new CreateDeliveryModeCommand(CurrentUser)), Token);
            return deliveryQueries.GetDelivery(result, CurrentUser);
        }

        public async Task<IQueryable<DeliveryModeDto>> UpdateDeliveryModeAsync(UpdateDeliveryModeInput input, [Service] IDeliveryQueries deliveryQueries)
        {
            SetLogTransaction("GraphQL", nameof(UpdateDeliveryModeAsync));
            await ExecuteCommandAsync<UpdateDeliveryModeCommand, bool>(_mapper.Map(input, new UpdateDeliveryModeCommand(CurrentUser)), Token);
            return deliveryQueries.GetDelivery(input.Id, CurrentUser);
        }

        public async Task<bool> DeleteDeliveryModeAsync(IdInput input)
        {
            SetLogTransaction("GraphQL", nameof(DeleteDeliveryModeAsync));
            return await ExecuteCommandAsync<DeleteDeliveryModeCommand, bool>(_mapper.Map(input, new DeleteDeliveryModeCommand(CurrentUser)), Token);
        }

        public async Task<IQueryable<ReturnableDto>> CreateReturnableAsync(CreateReturnableInput input, [Service] IReturnableQueries returnableQueries)
        {
            SetLogTransaction("GraphQL", nameof(CreateReturnableAsync));
            var result = await ExecuteCommandAsync<CreateReturnableCommand, Guid>(_mapper.Map(input, new CreateReturnableCommand(CurrentUser)), Token);
            return returnableQueries.GetReturnable(result, CurrentUser);
        }

        public async Task<IQueryable<ReturnableDto>> UpdateReturnableAsync(UpdateReturnableInput input, [Service] IReturnableQueries returnableQueries)
        {
            SetLogTransaction("GraphQL", nameof(UpdateReturnableAsync));
            await ExecuteCommandAsync<UpdateReturnableCommand, bool>(_mapper.Map(input, new UpdateReturnableCommand(CurrentUser)), Token);
            return returnableQueries.GetReturnable(input.Id, CurrentUser);
        }

        public async Task<bool> DeleteReturnableAsync(IdInput input)
        {
            SetLogTransaction("GraphQL", nameof(DeleteReturnableAsync));
            return await ExecuteCommandAsync<DeleteReturnableCommand, bool>(_mapper.Map(input, new DeleteReturnableCommand(CurrentUser)), Token);
        }

        private async Task<T> ExecuteCommandAsync<U, T>(U input, CancellationToken token) where U : ICommand<T>
        {
            var command = typeof(U).Name;

            using (var scope = _logger.BeginScope(new Dictionary<string, object>
            {
                ["UserIdentifier"] = CurrentUser.Id.ToString("N"),
                ["Roles"] = string.Join(';', CurrentUser.Roles),
                ["IsAuthenticated"] = CurrentUser.IsAuthenticated.ToString(),
                ["GraphQL"] = command,
            }))
            {
                _logger.LogInformation($"Executing mutation {command}");
                var result = await _mediator.Process(input, token);

                var currentTransaction = NewRelic.Api.Agent.NewRelic.GetAgent().CurrentTransaction;
                currentTransaction.AddCustomAttribute("CommandSucceeded", result.Success);

                if (result.Success)
                {
                    _logger.LogDebug($"Mutation {command} succeeded");
                    return result.Data;
                }
                else
                    _logger.LogDebug($"Mutation {command} failed");

                throw new UnexpectedException(result.Message);
            }
        }

        private void SetLogTransaction(string category, string name)
        {
            NewRelic.Api.Agent.NewRelic.SetTransactionName(category, name);

            var currentTransaction = NewRelic.Api.Agent.NewRelic.GetAgent().CurrentTransaction;
            currentTransaction.AddCustomAttribute("RequestId", _httpContextAccessor.HttpContext.TraceIdentifier);
            currentTransaction.AddCustomAttribute("UserIdentifier", CurrentUser.Id.ToString("N"));
            currentTransaction.AddCustomAttribute("IsAuthenticated", CurrentUser.IsAuthenticated.ToString());
            currentTransaction.AddCustomAttribute("Roles", string.Join(";", CurrentUser.Roles));
            currentTransaction.AddCustomAttribute("GraphQL", name);
        }
    }
}
