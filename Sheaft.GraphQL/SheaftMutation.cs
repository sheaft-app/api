using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
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
using Sheaft.Mediatr.Card.Commands;
using Sheaft.Mediatr.Catalog.Commands;
using Sheaft.Mediatr.Consumer.Commands;
using Sheaft.Mediatr.DeliveryClosing.Commands;
using Sheaft.Mediatr.DeliveryMode.Commands;
using Sheaft.Mediatr.Job.Commands;
using Sheaft.Mediatr.Legal.Commands;
using Sheaft.Mediatr.Notification.Commands;
using Sheaft.Mediatr.Order.Commands;
using Sheaft.Mediatr.PickingOrders.Commands;
using Sheaft.Mediatr.PreAuthorization.Commands;
using Sheaft.Mediatr.Producer.Commands;
using Sheaft.Mediatr.Product.Commands;
using Sheaft.Mediatr.ProfileInformation.Commands;
using Sheaft.Mediatr.PurchaseOrder.Commands;
using Sheaft.Mediatr.QuickOrder.Commands;
using Sheaft.Mediatr.Returnable.Commands;
using Sheaft.Mediatr.Store.Commands;
using Sheaft.Mediatr.Transactions.Commands;
using Sheaft.Mediatr.User.Commands;
using Sheaft.Mediatr.WebPayin.Commands;

namespace Sheaft.GraphQL
{
    public class SheaftMutation
    {
        private readonly ISheaftMediatr _mediator;
        private readonly ICurrentUserService _currentUserService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private CancellationToken Token => _httpContextAccessor.HttpContext.RequestAborted;

        private RequestUser CurrentUser => _currentUserService.GetCurrentUserInfo().Data;

        public SheaftMutation(
            ISheaftMediatr mediator,
            ICurrentUserService currentUserService,
            IHttpContextAccessor httpContextAccessor)
        {
            _mediator = mediator;
            _currentUserService = currentUserService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> GenerateUserSponsoringCodeAsync(GenerateUserCodeCommand input)
        {
            return await ExecuteAsync<GenerateUserCodeCommand, string>(input, Token);
        }

        public async Task<IQueryable<JobDto>> ExportPickingOrdersAsync(QueueExportPickingOrderCommand input,
            [Service] IJobQueries jobQueries)
        {
            var result = await ExecuteAsync<QueueExportPickingOrderCommand, Guid>(input, Token);
            return jobQueries.GetJob(result, CurrentUser);
        }

        public async Task<IQueryable<JobDto>> ExportPurchaseOrdersAsync(QueueExportPurchaseOrdersCommand input,
            [Service] IJobQueries jobQueries)
        {
            var result =
                await ExecuteAsync<QueueExportPurchaseOrdersCommand, Guid>(input, Token);
            return jobQueries.GetJob(result, CurrentUser);
        }

        public async Task<IQueryable<JobDto>> ExportTransactionsAsync(QueueExportTransactionsCommand input,
            [Service] IJobQueries jobQueries)
        {
            var result = await ExecuteAsync<QueueExportTransactionsCommand, Guid>(input, Token);
            return jobQueries.GetJob(result, CurrentUser);
        }

        public async Task<IQueryable<JobDto>> ExportUserDataAsync(QueueExportUserDataCommand input, [Service] IJobQueries jobQueries)
        {
            var result = await ExecuteAsync<QueueExportUserDataCommand, Guid>(input, Token);
            return jobQueries.GetJob(result, CurrentUser);
        }

        public async Task<IQueryable<JobDto>> ResumeJobsAsync(ResumeJobsCommand input, [Service] IJobQueries jobQueries)
        {
            await ExecuteAsync(input, Token);
            return jobQueries.GetJobs(CurrentUser).Where(j => input.JobIds.Contains(j.Id));
        }

        public async Task<IQueryable<JobDto>> PauseJobsAsync(PauseJobsCommand input, [Service] IJobQueries jobQueries)
        {
            await ExecuteAsync(input, Token);
            return jobQueries.GetJobs(CurrentUser).Where(j => input.JobIds.Contains(j.Id));
        }

        public async Task<IQueryable<JobDto>> RetryJobsAsync(RetryJobsCommand input, [Service] IJobQueries jobQueries)
        {
            await ExecuteAsync(input, Token);
            return jobQueries.GetJobs(CurrentUser).Where(j => input.JobIds.Contains(j.Id));
        }

        public async Task<IQueryable<JobDto>> CancelJobsAsync(CancelJobsCommand input,
            [Service] IJobQueries jobQueries)
        {
            await ExecuteAsync(input, Token);
            return jobQueries.GetJobs(CurrentUser).Where(j => input.JobIds.Contains(j.Id));
        }

        public async Task<IQueryable<JobDto>> ArchiveJobsAsync(ArchiveJobsCommand input, [Service] IJobQueries jobQueries)
        {
            await ExecuteAsync(input, Token);
            return jobQueries.GetJobs(CurrentUser).Where(j => input.JobIds.Contains(j.Id));
        }

        public async Task<IQueryable<AgreementDto>> CreateAgreementAsync(CreateAgreementCommand input,
            [Service] IAgreementQueries agreementQueries)
        {
            var result = await ExecuteAsync<CreateAgreementCommand, Guid>(input, Token);
            return agreementQueries.GetAgreement(result, CurrentUser);
        }

        public async Task<IQueryable<AgreementDto>> AcceptAgreementAsync(AcceptAgreementsCommand input,
            [Service] IAgreementQueries agreementQueries)
        {
            await ExecuteAsync(input, Token);
            return agreementQueries.GetAgreements(CurrentUser).Where(j => input.AgreementIds.Contains(j.Id));
        }

        public async Task<IQueryable<AgreementDto>> AssignCatalogToAgreementAsync(AssignCatalogToAgreementCommand input,
            [Service] IAgreementQueries agreementQueries)
        {
            await ExecuteAsync(input, Token);
            return agreementQueries.GetAgreement(input.AgreementId, CurrentUser);
        }

        public async Task<IQueryable<AgreementDto>> CancelAgreementsAsync(CancelAgreementsCommand input,
            [Service] IAgreementQueries agreementQueries)
        {
            await ExecuteAsync(input, Token);
            return agreementQueries.GetAgreements(CurrentUser).Where(j => input.AgreementIds.Contains(j.Id));
        }

        public async Task<IQueryable<AgreementDto>> RefuseAgreementsAsync(RefuseAgreementsCommand input,
            [Service] IAgreementQueries agreementQueries)
        {
            await ExecuteAsync(input, Token);
            return agreementQueries.GetAgreements(CurrentUser).Where(j => input.AgreementIds.Contains(j.Id));
        }

        public async Task<DateTimeOffset> MarkMyNotificationsAsReadAsync()
        {
            var input = new MarkUserNotificationsAsReadCommand(CurrentUser) {ReadBefore = DateTimeOffset.UtcNow};
            await ExecuteAsync(input, Token);
            return input.ReadBefore;
        }

        public async Task<IQueryable<NotificationDto>> MarkNotificationAsReadAsync(MarkUserNotificationAsReadCommand input,
            [Service] INotificationQueries notificationQueries)
        {
            await ExecuteAsync(input, Token);
            return notificationQueries.GetNotification(input.NotificationId, CurrentUser);
        }

        public async Task<IQueryable<OrderDto>> CreateOrderAsync(CreateConsumerOrderCommand input,
            [Service] IOrderQueries orderQueries)
        {
            var result = await ExecuteAsync<CreateConsumerOrderCommand, Guid>(input, Token);
            return orderQueries.GetOrder(result, CurrentUser);
        }

        public async Task<IQueryable<OrderDto>> UpdateOrderAsync(UpdateConsumerOrderCommand input,
            [Service] IOrderQueries orderQueries)
        {
            await ExecuteAsync(input, Token);
            return orderQueries.GetOrder(input.OrderId, CurrentUser);
        }

        public async Task<IQueryable<WebPayinDto>> CreateWebPayinForOrderAsync(CreateWebPayinForOrderCommand input,
            [Service] IPayinQueries payinQueries)
        {
            var result =
                await ExecuteAsync<CreateWebPayinForOrderCommand, Guid>(input, Token);
            return payinQueries.GetWebPayin(result, CurrentUser);
        }

        public async Task<IQueryable<OrderDto>> ResetOrderAsync(ResetOrderCommand input,
            [Service] IOrderQueries orderQueries)
        {
            await ExecuteAsync(input, Token);
            return orderQueries.GetOrder(input.OrderId, CurrentUser);
        }

        public async Task<IQueryable<PurchaseOrderDto>> CreateBusinessOrderAsync(CreateBusinessOrderCommand input,
            [Service] IPurchaseOrderQueries purchaseOrderQueries)
        {
            var result =
                await ExecuteAsync<CreateBusinessOrderCommand, IEnumerable<Guid>>(input, Token);
            return purchaseOrderQueries.GetPurchaseOrders(CurrentUser).Where(j => result.Contains(j.Id));
        }

        public async Task<IQueryable<PurchaseOrderDto>> AcceptPurchaseOrdersAsync(AcceptPurchaseOrdersCommand input,
            [Service] IPurchaseOrderQueries purchaseOrderQueries)
        {
            await ExecuteAsync(input, Token);
            return purchaseOrderQueries.GetPurchaseOrders(CurrentUser).Where(j => input.PurchaseOrderIds.Contains(j.Id));
        }

        public async Task<IQueryable<PurchaseOrderDto>> ShipPurchaseOrdersAsync(ShipPurchaseOrdersCommand input,
            [Service] IPurchaseOrderQueries purchaseOrderQueries)
        {
            await ExecuteAsync(input, Token);
            return purchaseOrderQueries.GetPurchaseOrders(CurrentUser).Where(j => input.PurchaseOrderIds.Contains(j.Id));
        }

        public async Task<IQueryable<PurchaseOrderDto>> DeliverPurchaseOrdersAsync(DeliverPurchaseOrdersCommand input,
            [Service] IPurchaseOrderQueries purchaseOrderQueries)
        {
            await ExecuteAsync(input, Token);
            return purchaseOrderQueries.GetPurchaseOrders(CurrentUser).Where(j => input.PurchaseOrderIds.Contains(j.Id));
        }

        public async Task<IQueryable<PurchaseOrderDto>> ProcessPurchaseOrdersAsync(ProcessPurchaseOrdersCommand input,
            [Service] IPurchaseOrderQueries purchaseOrderQueries)
        {
            await ExecuteAsync(input, Token);
            return purchaseOrderQueries.GetPurchaseOrders(CurrentUser).Where(j => input.PurchaseOrderIds.Contains(j.Id));
        }

        public async Task<IQueryable<PurchaseOrderDto>> CompletePurchaseOrdersAsync(CompletePurchaseOrdersCommand input,
            [Service] IPurchaseOrderQueries purchaseOrderQueries)
        {
            await ExecuteAsync(input, Token);
            return purchaseOrderQueries.GetPurchaseOrders(CurrentUser).Where(j => input.PurchaseOrderIds.Contains(j.Id));
        }

        public async Task<IQueryable<PurchaseOrderDto>> CancelPurchaseOrdersAsync(CancelPurchaseOrdersCommand input,
            [Service] IPurchaseOrderQueries purchaseOrderQueries)
        {
            await ExecuteAsync(input, Token);
            return purchaseOrderQueries.GetPurchaseOrders(CurrentUser).Where(j => input.PurchaseOrderIds.Contains(j.Id));
        }

        public async Task<IQueryable<PurchaseOrderDto>> WithdrawnPurchaseOrdersAsync(WithdrawnPurchaseOrdersCommand input,
            [Service] IPurchaseOrderQueries purchaseOrderQueries)
        {
            await ExecuteAsync(input, Token);
            return purchaseOrderQueries.GetPurchaseOrders(CurrentUser).Where(j => input.PurchaseOrderIds.Contains(j.Id));
        }
        
        public async Task<IQueryable<PurchaseOrderDto>> RefusePurchaseOrdersAsync(RefusePurchaseOrdersCommand input,
            [Service] IPurchaseOrderQueries purchaseOrderQueries)
        {
            await ExecuteAsync(input, Token);
            return purchaseOrderQueries.GetPurchaseOrders(CurrentUser).Where(j => input.PurchaseOrderIds.Contains(j.Id));
        }

        public async Task<bool> DeletePurchaseOrdersAsync(DeletePurchaseOrdersCommand input)
        {
            return await ExecuteAsync(input, Token);
        }

        public async Task<IQueryable<ProductDto>> CreateProductAsync(CreateProductCommand input,
            [Service] IProductQueries productQueries)
        {
            var result = await ExecuteAsync<CreateProductCommand, Guid>(input, Token);
            return await productQueries.GetProduct(result, CurrentUser, Token);
        }

        public async Task<IQueryable<ProductDto>> UpdateProductAsync(UpdateProductCommand input,
            [Service] IProductQueries productQueries)
        {
            await ExecuteAsync(input, Token);
            return await productQueries.GetProduct(input.ProductId, CurrentUser, Token);
        }

        public async Task<IQueryable<ProductDto>> RateProductAsync(RateProductCommand input,
            [Service] IProductQueries productQueries)
        {
            await ExecuteAsync(input, Token);
            return await productQueries.GetProduct(input.ProductId, CurrentUser, Token);
        }

        public async Task<IQueryable<ProductDto>> UpdateProductPictureAsync(UpdateProductPreviewCommand input,
            [Service] IProductQueries productQueries)
        {
            await ExecuteAsync<UpdateProductPreviewCommand, string>(input, Token);
            return await productQueries.GetProduct(input.ProductId, CurrentUser, Token);
        }

        public async Task<IQueryable<ProductDto>> SetProductsAvailabilityAsync(SetProductsAvailabilityCommand input,
            [Service] IProductQueries productQueries)
        {
            await ExecuteAsync(input, Token);
            return productQueries.GetProducts(CurrentUser).Where(j => input.ProductIds.Contains(j.Id));
        }

        public async Task<bool> DeleteProductsAsync(DeleteProductsCommand input)
        {
            return await ExecuteAsync(input, Token);
        }

        public async Task<IQueryable<StoreDto>> RegisterStoreAsync(RegisterStoreCommand input,
            [Service] IStoreQueries storeQueries)
        {
            var result = await ExecuteAsync<RegisterStoreCommand, Guid>(input, Token);
            return storeQueries.GetStore(result, CurrentUser);
        }

        public async Task<IQueryable<StoreDto>> UpdateStoreAsync(UpdateStoreCommand input,
            [Service] IStoreQueries storeQueries)
        {
            await ExecuteAsync(input, Token);
            return storeQueries.GetStore(input.StoreId, CurrentUser);
        }

        public async Task<IQueryable<ProducerDto>> RegisterProducerAsync(RegisterProducerCommand input,
            [Service] IProducerQueries producerQueries)
        {
            var result = await ExecuteAsync<RegisterProducerCommand, Guid>(input, Token);
            return producerQueries.GetProducer(result, CurrentUser);
        }

        public async Task<IQueryable<ProducerDto>> UpdateProducerAsync(UpdateProducerCommand input,
            [Service] IProducerQueries producerQueries)
        {
            await ExecuteAsync(input, Token);
            return producerQueries.GetProducer(input.ProducerId, CurrentUser);
        }

        public async Task<IQueryable<BusinessLegalDto>> CreateBusinessLegalsAsync(CreateBusinessLegalCommand input,
            [Service] ILegalQueries legalQueries)
        {
            var result = await ExecuteAsync<CreateBusinessLegalCommand, Guid>(input, Token);
            return legalQueries.GetBusinessLegals(result, CurrentUser);
        }

        public async Task<IQueryable<BusinessLegalDto>> UpdateBusinessLegalsAsync(UpdateBusinessLegalCommand input,
            [Service] ILegalQueries legalQueries)
        {
            await ExecuteAsync(input, Token);
            return legalQueries.GetBusinessLegals(input.LegalId, CurrentUser);
        }

        public async Task<IQueryable<ConsumerDto>> RegisterConsumerAsync(RegisterConsumerCommand input,
            [Service] IConsumerQueries consumerQueries)
        {
            var result = await ExecuteAsync<RegisterConsumerCommand, Guid>(input, Token);
            return consumerQueries.GetConsumer(result, CurrentUser);
        }

        public async Task<IQueryable<ConsumerDto>> UpdateConsumerAsync(UpdateConsumerCommand input,
            [Service] IConsumerQueries consumerQueries)
        {
            await ExecuteAsync(input, Token);
            return consumerQueries.GetConsumer(input.ConsumerId, CurrentUser);
        }

        public async Task<IQueryable<ConsumerLegalDto>> CreateConsumerLegalsAsync(CreateConsumerLegalCommand input,
            [Service] ILegalQueries legalQueries)
        {
            var result = await ExecuteAsync<CreateConsumerLegalCommand, Guid>(input, Token);
            return legalQueries.GetConsumerLegals(result, CurrentUser);
        }

        public async Task<IQueryable<ConsumerLegalDto>> UpdateConsumerLegalsAsync(UpdateConsumerLegalCommand input,
            [Service] ILegalQueries legalQueries)
        {
            await ExecuteAsync(input, Token);
            return legalQueries.GetConsumerLegals(input.LegalId, CurrentUser);
        }

        public async Task<IQueryable<UserDto>> UpdateUserPictureAsync(UpdateUserPreviewCommand input,
            [Service] IUserQueries userQueries)
        {
            await ExecuteAsync<UpdateUserPreviewCommand, string>(input, Token);
            return userQueries.GetUser(input.UserId, CurrentUser);
        }

        public async Task<bool> AddPictureToUserAsync(AddPictureToUserCommand input)
        {
            return await ExecuteAsync(input, Token);
        }

        public async Task<bool> RemoveUserPicturesAsync(RemoveUserPicturesCommand input)
        {
            return await ExecuteAsync(input, Token);
        }

        public async Task<bool> AddPictureToProductAsync(AddPictureToProductCommand input)
        {
            return await ExecuteAsync(input, Token);
        }

        public async Task<bool> RemoveProductPicturesAsync(RemoveProductPicturesCommand input)
        {
            return await ExecuteAsync(input, Token);
        }

        public async Task<bool> RemoveUserAsync(RemoveUserCommand input)
        {
            return await ExecuteAsync(input, Token);
        }

        public async Task<IQueryable<QuickOrderDto>> CreateQuickOrderAsync(CreateQuickOrderCommand input,
            [Service] IQuickOrderQueries quickOrderQueries)
        {
            var result = await ExecuteAsync<CreateQuickOrderCommand, Guid>(input, Token);
            return quickOrderQueries.GetQuickOrder(result, CurrentUser);
        }

        public async Task<IQueryable<QuickOrderDto>> SetDefaultQuickOrderAsync(SetQuickOrderAsDefaultCommand input,
            [Service] IQuickOrderQueries quickOrderQueries)
        {
            await ExecuteAsync(input, Token);
            return quickOrderQueries.GetQuickOrder(input.QuickOrderId, CurrentUser);
        }

        public async Task<IQueryable<QuickOrderDto>> UpdateQuickOrderAsync(UpdateQuickOrderCommand input,
            [Service] IQuickOrderQueries quickOrderQueries)
        {
            await ExecuteAsync(input, Token);
            return quickOrderQueries.GetQuickOrder(input.QuickOrderId, CurrentUser);
        }

        public async Task<bool> DeleteQuickOrdersAsync(DeleteQuickOrdersCommand input)
        {
            return await ExecuteAsync(input, Token);
        }

        public async Task<IQueryable<DeliveryModeDto>> SetDeliveryModesAvailabilityAsync(
            SetDeliveryModesAvailabilityCommand input, [Service] IDeliveryQueries deliveryModeQueries)
        {
            await ExecuteAsync(input, Token);
            return deliveryModeQueries.GetDeliveries(CurrentUser).Where(j => input.DeliveryModeIds.Contains(j.Id));
        }

        public async Task<IQueryable<DeliveryModeDto>> CreateDeliveryModeAsync(CreateDeliveryModeCommand input,
            [Service] IDeliveryQueries deliveryQueries)
        {
            var result = await ExecuteAsync<CreateDeliveryModeCommand, Guid>(input, Token);
            return deliveryQueries.GetDelivery(result, CurrentUser);
        }

        public async Task<IQueryable<DeliveryModeDto>> UpdateDeliveryModeAsync(UpdateDeliveryModeCommand input,
            [Service] IDeliveryQueries deliveryQueries)
        {
            await ExecuteAsync(input, Token);
            return deliveryQueries.GetDelivery(input.DeliveryModeId, CurrentUser);
        }

        public async Task<bool> DeleteDeliveryModeAsync(DeleteDeliveryModeCommand input)
        {
            return await ExecuteAsync(input, Token);
        }

        public async Task<IQueryable<ReturnableDto>> CreateReturnableAsync(CreateReturnableCommand input,
            [Service] IReturnableQueries returnableQueries)
        {
            var result = await ExecuteAsync<CreateReturnableCommand, Guid>(input, Token);
            return returnableQueries.GetReturnable(result, CurrentUser);
        }

        public async Task<IQueryable<ReturnableDto>> UpdateReturnableAsync(UpdateReturnableCommand input,
            [Service] IReturnableQueries returnableQueries)
        {
            await ExecuteAsync(input, Token);
            return returnableQueries.GetReturnable(input.ReturnableId, CurrentUser);
        }

        public async Task<bool> DeleteReturnableAsync(DeleteReturnableCommand input)
        {
            return await ExecuteAsync(input, Token);
        }

        public async Task<IQueryable<ClosingDto>> UpdateOrCreateBusinessClosingsAsync(
            UpdateOrCreateBusinessClosingsCommand input, [Service] IBusinessClosingQueries closingQueries)
        {
            var result =
                await ExecuteAsync<UpdateOrCreateBusinessClosingsCommand,
                    IEnumerable<Guid>>(input, Token);
            return closingQueries.GetClosings(CurrentUser).Where(c => result.Contains(c.Id));
        }

        public async Task<IQueryable<ClosingDto>> UpdateOrCreateBusinessClosingAsync(
            UpdateOrCreateBusinessClosingCommand input, [Service] IBusinessClosingQueries closingQueries)
        {
            var result =
                await ExecuteAsync<UpdateOrCreateBusinessClosingCommand, Guid>(
                    input, Token);
            return closingQueries.GetClosing(result, CurrentUser);
        }

        public async Task<IQueryable<ClosingDto>> UpdateOrCreateDeliveryClosingAsync(
            UpdateOrCreateDeliveryClosingCommand input, [Service] IDeliveryClosingQueries closingQueries)
        {
            var result =
                await ExecuteAsync<UpdateOrCreateDeliveryClosingCommand, Guid>(
                    input, Token);
            return closingQueries.GetClosing(result, CurrentUser);
        }

        public async Task<IQueryable<ClosingDto>> UpdateOrCreateDeliveryClosingsAsync(
            UpdateOrCreateDeliveryClosingsCommand input, [Service] IDeliveryClosingQueries closingQueries)
        {
            var result =
                await ExecuteAsync<UpdateOrCreateDeliveryClosingsCommand, IEnumerable<Guid>>(
                    input, Token);
            return closingQueries.GetClosings(input.DeliveryId, CurrentUser).Where(c => result.Contains(c.Id));
        }

        public async Task<bool> DeleteBusinessClosingsAsync(DeleteBusinessClosingsCommand input)
        {
            return await ExecuteAsync(input, Token);
        }

        public async Task<bool> DeleteDeliveryClosingsAsync(DeleteDeliveryClosingsCommand input)
        {
            return await ExecuteAsync(input, Token);
        }

        public async Task<IQueryable<CatalogDto>> CreateCatalogAsync(CreateCatalogCommand input,
            [Service] ICatalogQueries catalogQueries)
        {
            var result = await ExecuteAsync<CreateCatalogCommand, Guid>(input, Token);
            return catalogQueries.GetCatalog(result, CurrentUser);
        }

        public async Task<IQueryable<CatalogDto>> UpdateCatalogAsync(UpdateCatalogCommand input,
            [Service] ICatalogQueries catalogQueries)
        {
            await ExecuteAsync(input, Token);
            return catalogQueries.GetCatalog(input.CatalogId, CurrentUser);
        }

        public async Task<bool> DeleteCatalogsAsync(DeleteCatalogsCommand input)
        {
            return await ExecuteAsync(input, Token);
        }

        public async Task<IQueryable<CatalogDto>> AddOrUpdateProductsToCatalogAsync(AddOrUpdateProductsToCatalogCommand input,
            [Service] ICatalogQueries catalogQueries)
        {
            await ExecuteAsync(input, Token);
            return catalogQueries.GetCatalog(input.CatalogId, CurrentUser);
        }

        public async Task<IQueryable<CatalogDto>> RemoveProductsFromCatalogAsync(RemoveProductsFromCatalogCommand input,
            [Service] ICatalogQueries catalogQueries)
        {
            await ExecuteAsync(input, Token);
            return catalogQueries.GetCatalog(input.CatalogId, CurrentUser);
        }

        public async Task<IQueryable<CatalogDto>> CloneCatalogAsync(CloneCatalogCommand input,
            [Service] ICatalogQueries catalogQueries)
        {
            var result = await ExecuteAsync<CloneCatalogCommand, Guid>(input, Token);
            return catalogQueries.GetCatalog(result, CurrentUser);
        }

        public async Task<IQueryable<CatalogDto>> UpdateAllCatalogPricesAsync(UpdateAllCatalogPricesCommand input,
            [Service] ICatalogQueries catalogQueries)
        {
            await ExecuteAsync(input, Token);
            return catalogQueries.GetCatalog(input.CatalogId, CurrentUser);
        }

        public async Task<IQueryable<CatalogDto>> UpdateCatalogPricesAsync(UpdateCatalogPricesCommand input,
            [Service] ICatalogQueries catalogQueries)
        {
            await ExecuteAsync(input, Token);
            return catalogQueries.GetCatalog(input.CatalogId, CurrentUser);
        }

        public async Task<IQueryable<CatalogDto>> SetCatalogAsDefaultAsync(SetCatalogAsDefaultCommand input,
            [Service] ICatalogQueries catalogQueries)
        {
            await ExecuteAsync(input, Token);
            return catalogQueries.GetCatalog(input.CatalogId, CurrentUser);
        }

        public async Task<IQueryable<CatalogDto>> SetCatalogsAvailabilityAsync(SetCatalogsAvailabilityCommand input,
            [Service] ICatalogQueries catalogQueries)
        {
            await ExecuteAsync(input, Token);
            return catalogQueries.GetCatalogs(CurrentUser).Where(c => input.CatalogIds.Contains(c.Id));
        }

        public async Task<CardRegistrationDto> CreateCardRegistration()
        {
            return await ExecuteAsync<CreateCardRegistrationCommand, CardRegistrationDto>(new CreateCardRegistrationCommand(CurrentUser), Token);
        }

        public async Task<IQueryable<PreAuthorizationDto>> CreatePreAuthorization(CreatePreAuthorizationForOrderCommand input, [Service] IPreAuthorizationQueries preAuthorizationQueries)
        {
            input.IpAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            input.BrowserInfo.AcceptHeader = _httpContextAccessor.HttpContext.Request.Headers["Accept"];
            
            var result = await ExecuteAsync<CreatePreAuthorizationForOrderCommand, Guid>(input, Token);
            return preAuthorizationQueries.GetPreAuthorization(result, CurrentUser);
        }
        
        private async Task<bool> ExecuteAsync<T>(T input, CancellationToken token,
            [CallerMemberName] string memberName = null) where T : ICommand
        {
            SetLogTransaction(input, memberName);

            input.SetRequestUser(CurrentUser);
            var result = await _mediator.Process(input, token);
            if (result.Succeeded)
                return true;
            
            throw new SheaftException(result);
        }
        
        private async Task<TU> ExecuteAsync<T, TU>(T input, CancellationToken token,
            [CallerMemberName] string memberName = null) where T : ICommand<TU>
        {
            SetLogTransaction(input, memberName);

            input.SetRequestUser(CurrentUser);
            var result = await _mediator.Process(input, token);
            if (result.Succeeded)
                return result.Data;

            throw new SheaftException(result);
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