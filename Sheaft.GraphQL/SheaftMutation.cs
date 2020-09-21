﻿using MediatR;
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
using Sheaft.Core.Security;
using HotChocolate;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using Microsoft.AspNetCore.Http;
using HotChocolate.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Sheaft.Exceptions;
using Sheaft.Core;
using Sheaft.GraphQL.Sorts;
using Sheaft.GraphQL.Filters;
using Sheaft.GraphQL.Types;

namespace Sheaft.GraphQL
{
    public class SheaftMutation
    {
        private readonly IMediator _mediator;
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
            IMediator mediator,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            ILogger<SheaftMutation> logger)
        {
            _mediator = mediator;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        [Authorize(Policy = Policies.REGISTERED)]
        [GraphQLName("generateUserSponsoringCode")]
        public async Task<string> GenerateUserSponsoringCodeAsync(IdInput input)
        {
            return await ExecuteCommandAsync<GenerateUserCodeCommand, string>(_mapper.Map(input, new GenerateUserCodeCommand(CurrentUser)), Token);
        }

        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLName("exportPickingFromOrders")]
        [UseSingleOrDefault]
        [UseSelection]
        public async Task<IQueryable<JobDto>> ExportPickingOrdersAsync(ExportPickingOrdersInput input, [Service] IJobQueries jobQueries)
        {
            var result = await ExecuteCommandAsync<QueueExportPickingOrderCommand, Guid>(_mapper.Map(input, new QueueExportPickingOrderCommand(CurrentUser)), Token);
            return jobQueries.GetJob(result, CurrentUser);
        }

        [Authorize(Policy = Policies.REGISTERED)]
        [GraphQLName("exportRGPD")]
        [UseSingleOrDefault]
        [UseSelection]
        public async Task<IQueryable<JobDto>> ExportUserDataAsync(IdInput input, [Service] IJobQueries jobQueries)
        {
            var result = await ExecuteCommandAsync<QueueExportUserDataCommand, Guid>(_mapper.Map(input, new QueueExportUserDataCommand(CurrentUser)), Token);
            return jobQueries.GetJob(result, CurrentUser);
        }

        [Authorize(Policy = Policies.REGISTERED)]
        [GraphQLName("resumeJobs")]
        [UsePaging]
        [UseSorting(SortType = typeof(JobSortType))]
        [UseFiltering(FilterType = typeof(JobFilterType))]
        [UseSelection]
        public async Task<IQueryable<JobDto>> ResumeJobsAsync(IdsInput input, [Service] IJobQueries jobQueries)
        {
            var result = await ExecuteCommandAsync<ResumeJobsCommand, bool>(_mapper.Map(input, new ResumeJobsCommand(CurrentUser)), Token);
            return jobQueries.GetJobs(CurrentUser).Where(j => input.Ids.Contains(j.Id));
        }

        [Authorize(Policy = Policies.REGISTERED)]
        [GraphQLName("pauseJobs")]
        [UsePaging]
        [UseSorting(SortType = typeof(JobSortType))]
        [UseFiltering(FilterType = typeof(JobFilterType))]
        [UseSelection]
        public async Task<IQueryable<JobDto>> PauseJobsAsync(IdsInput input, [Service] IJobQueries jobQueries)
        {
            var result = await ExecuteCommandAsync<PauseJobsCommand, bool>(_mapper.Map(input, new PauseJobsCommand(CurrentUser)), Token);
            return jobQueries.GetJobs(CurrentUser).Where(j => input.Ids.Contains(j.Id));
        }

        [Authorize(Policy = Policies.REGISTERED)]
        [GraphQLName("retryJobs")]
        [UsePaging]
        [UseSorting(SortType = typeof(JobSortType))]
        [UseFiltering(FilterType = typeof(JobFilterType))]
        [UseSelection]
        public async Task<IQueryable<JobDto>> RetryJobsAsync(IdsInput input, [Service] IJobQueries jobQueries)
        {
            var result = await ExecuteCommandAsync<RetryJobsCommand, bool>(_mapper.Map(input, new RetryJobsCommand(CurrentUser)), Token);
            return jobQueries.GetJobs(CurrentUser).Where(j => input.Ids.Contains(j.Id));
        }

        [Authorize(Policy = Policies.REGISTERED)]
        [GraphQLName("cancelJobs")]
        [UsePaging]
        [UseSorting(SortType = typeof(JobSortType))]
        [UseFiltering(FilterType = typeof(JobFilterType))]
        [UseSelection]
        public async Task<IQueryable<JobDto>> CancelJobsAsync(IdsWithReasonInput input, [Service] IJobQueries jobQueries)
        {
            var result = await ExecuteCommandAsync<CancelJobsCommand, bool>(_mapper.Map(input, new CancelJobsCommand(CurrentUser)), Token);
            return jobQueries.GetJobs(CurrentUser).Where(j => input.Ids.Contains(j.Id));
        }

        [Authorize(Policy = Policies.REGISTERED)]
        [GraphQLName("archiveJobs")]
        [UsePaging]
        [UseSorting(SortType = typeof(JobSortType))]
        [UseFiltering(FilterType = typeof(JobFilterType))]
        [UseSelection]
        public async Task<IQueryable<JobDto>> ArchiveJobsAsync(IdsInput input, [Service] IJobQueries jobQueries)
        {
            var result = await ExecuteCommandAsync<ArchiveJobsCommand, bool>(_mapper.Map(input, new ArchiveJobsCommand(CurrentUser)), Token);
            return jobQueries.GetJobs(CurrentUser).Where(j => input.Ids.Contains(j.Id));
        }

        [Authorize(Policy = Policies.STORE_OR_PRODUCER)]
        [GraphQLName("createAgreement")]
        [UseSingleOrDefault]
        [UseSelection]
        public async Task<IQueryable<AgreementDto>> CreateAgreementAsync(CreateAgreementInput input, [Service] IAgreementQueries agreementQueries)
        {
            var result = await ExecuteCommandAsync<CreateAgreementCommand, Guid>(_mapper.Map(input, new CreateAgreementCommand(CurrentUser)), Token);
            return agreementQueries.GetAgreement(result, CurrentUser);
        }

        [Authorize(Policy = Policies.STORE_OR_PRODUCER)]
        [GraphQLName("acceptAgreement")]
        [UseSingleOrDefault]
        [UseSelection]
        public async Task<IQueryable<AgreementDto>> AcceptAgreementAsync(IdTimeSlotGroupInput input, [Service] IAgreementQueries agreementQueries)
        {
            await ExecuteCommandAsync<AcceptAgreementCommand, bool>(_mapper.Map(input, new AcceptAgreementCommand(CurrentUser)), Token);
            return agreementQueries.GetAgreement(input.Id, CurrentUser);
        }

        [Authorize(Policy = Policies.STORE_OR_PRODUCER)]
        [GraphQLName("cancelAgreements")]
        [UsePaging]
        [UseSorting(SortType = typeof(AgreementSortType))]
        [UseFiltering(FilterType = typeof(AgreementFilterType))]
        [UseSelection]
        public async Task<IQueryable<AgreementDto>> CancelAgreementsAsync(IdsWithReasonInput input, [Service] IAgreementQueries agreementQueries)
        {
            await ExecuteCommandAsync<CancelAgreementsCommand, bool>(_mapper.Map(input, new CancelAgreementsCommand(CurrentUser)), Token);
            return agreementQueries.GetAgreements(CurrentUser).Where(j => input.Ids.Contains(j.Id));
        }

        [Authorize(Policy = Policies.STORE_OR_PRODUCER)]
        [GraphQLName("refuseAgreements")]
        [UsePaging]
        [UseSorting(SortType = typeof(AgreementSortType))]
        [UseFiltering(FilterType = typeof(AgreementFilterType))]
        [UseSelection]
        public async Task<IQueryable<AgreementDto>> RefuseAgreementsAsync(IdsWithReasonInput input, [Service] IAgreementQueries agreementQueries)
        {
            await ExecuteCommandAsync<RefuseAgreementsCommand, bool>(_mapper.Map(input, new RefuseAgreementsCommand(CurrentUser)), Token);
            return agreementQueries.GetAgreements(CurrentUser).Where(j => input.Ids.Contains(j.Id));
        }

        [Authorize(Policy = Policies.REGISTERED)]
        [GraphQLName("markUserNotificationsAsRead")]
        public async Task<DateTimeOffset> MarkMyNotificationsAsReadAsync()
        {
            var input = new MarkUserNotificationsAsReadCommand(CurrentUser) { ReadBefore = DateTimeOffset.UtcNow };
            await ExecuteCommandAsync<MarkUserNotificationsAsReadCommand, bool>(input, Token);
            return input.ReadBefore;
        }

        [Authorize(Policy = Policies.REGISTERED)]
        [GraphQLName("markUserNotificationAsRead")]
        [UseSingleOrDefault]
        [UseSelection]
        public async Task<IQueryable<NotificationDto>> MarkNotificationAsReadAsync(IdInput input, [Service] INotificationQueries notificationQueries)
        {
            await ExecuteCommandAsync<MarkUserNotificationAsReadCommand, bool>(_mapper.Map(input, new MarkUserNotificationAsReadCommand(CurrentUser)), Token);
            return notificationQueries.GetNotification(input.Id, CurrentUser);
        }

        [Authorize(Policy = Policies.CONSUMER)]
        [GraphQLName("createOrder")]
        [UseSingleOrDefault]
        [UseSelection]
        public async Task<IQueryable<OrderDto>> CreateOrderAsync(CreateOrderInput input, [Service] IOrderQueries orderQueries)
        {
            var result = await ExecuteCommandAsync<CreateConsumerOrderCommand, Guid>(_mapper.Map(input, new CreateConsumerOrderCommand(CurrentUser)), Token);
            return orderQueries.GetOrder(result, CurrentUser);
        }

        [Authorize(Policy = Policies.CONSUMER)]
        [GraphQLName("updateOrder")]
        [UseSingleOrDefault]
        [UseSelection]
        public async Task<IQueryable<OrderDto>> UpdateOrderAsync(UpdateOrderInput input, [Service] IOrderQueries orderQueries)
        {
            await ExecuteCommandAsync<UpdateConsumerOrderCommand, bool>(_mapper.Map(input, new UpdateConsumerOrderCommand(CurrentUser)), Token);
            return orderQueries.GetOrder(input.Id, CurrentUser);
        }

        [Authorize(Policy = Policies.STORE)]
        [GraphQLName("createPurchaseOrders")]
        [UsePaging]
        [UseSorting(SortType = typeof(PurchaseOrderSortType))]
        [UseFiltering(FilterType = typeof(PurchaseOrderFilterType))]
        [UseSelection]
        public async Task<IQueryable<PurchaseOrderDto>> CreateBusinessOrderAsync(CreateOrderInput input, [Service] IPurchaseOrderQueries purchaseOrderQueries)
        {
            var result = await ExecuteCommandAsync<CreateBusinessOrderCommand, IEnumerable<Guid>>(_mapper.Map(input, new CreateBusinessOrderCommand(CurrentUser)), Token);
            return purchaseOrderQueries.GetPurchaseOrders(CurrentUser).Where(j => result.Contains(j.Id));
        }

        [Authorize(Policy = Policies.CONSUMER)]
        [GraphQLName("payOrder")]
        [UseSingleOrDefault]
        [UseSelection]
        public async Task<IQueryable<WebPayinTransactionDto>> PayOrderAsync(IdInput input, [Service] ITransactionQueries transactionQueries)
        {
            var result = await ExecuteCommandAsync<PayOrderCommand, Guid>(_mapper.Map(input, new PayOrderCommand(CurrentUser)), Token);
            return transactionQueries.GetWebPayinTransaction(result, CurrentUser);
        }

        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLName("createDocument")]
        [UseSingleOrDefault]
        [UseSelection]
        public async Task<IQueryable<DocumentDto>> CreateDocumentAsync(CreateDocumentInput input, [Service] IDocumentQueries documentQueries)
        {
            var result = await ExecuteCommandAsync<CreateDocumentCommand, Guid>(_mapper.Map(input, new CreateDocumentCommand(CurrentUser)), Token);
            return documentQueries.GetDocument(result, CurrentUser);
        }

        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLName("removeDocument")]
        public async Task<bool> RemoveDocumentAsync(IdInput input)
        {
            return await ExecuteCommandAsync<RemoveDocumentCommand, bool>(_mapper.Map(input, new RemoveDocumentCommand(CurrentUser)), Token);
        }

        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLName("acceptPurchaseOrders")]
        [UsePaging]
        [UseSorting(SortType = typeof(PurchaseOrderSortType))]
        [UseFiltering(FilterType = typeof(PurchaseOrderFilterType))]
        [UseSelection]
        public async Task<IQueryable<PurchaseOrderDto>> AcceptPurchaseOrdersAsync(IdsInput input, [Service] IPurchaseOrderQueries purchaseOrderQueries)
        {
            await ExecuteCommandAsync<AcceptPurchaseOrdersCommand, bool>(_mapper.Map(input, new AcceptPurchaseOrdersCommand(CurrentUser)), Token);
            return purchaseOrderQueries.GetPurchaseOrders(CurrentUser).Where(j => input.Ids.Contains(j.Id));
        }

        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLName("shipPurchaseOrders")]
        [UsePaging]
        [UseSorting(SortType = typeof(PurchaseOrderSortType))]
        [UseFiltering(FilterType = typeof(PurchaseOrderFilterType))]
        [UseSelection]
        public async Task<IQueryable<PurchaseOrderDto>> ShipPurchaseOrdersAsync(IdsInput input, [Service] IPurchaseOrderQueries purchaseOrderQueries)
        {
            await ExecuteCommandAsync<ShipPurchaseOrdersCommand, bool>(_mapper.Map(input, new ShipPurchaseOrdersCommand(CurrentUser)), Token);
            return purchaseOrderQueries.GetPurchaseOrders(CurrentUser).Where(j => input.Ids.Contains(j.Id));
        }

        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLName("deliverPurchaseOrders")]
        [UsePaging]
        [UseSorting(SortType = typeof(PurchaseOrderSortType))]
        [UseFiltering(FilterType = typeof(PurchaseOrderFilterType))]
        [UseSelection]
        public async Task<IQueryable<PurchaseOrderDto>> DeliverPurchaseOrdersAsync(IdsInput input, [Service] IPurchaseOrderQueries purchaseOrderQueries)
        {
            await ExecuteCommandAsync<DeliverPurchaseOrdersCommand, bool>(_mapper.Map(input, new DeliverPurchaseOrdersCommand(CurrentUser)), Token);
            return purchaseOrderQueries.GetPurchaseOrders(CurrentUser).Where(j => input.Ids.Contains(j.Id));
        }

        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLName("processPurchaseOrders")]
        [UsePaging]
        [UseSorting(SortType = typeof(PurchaseOrderSortType))]
        [UseFiltering(FilterType = typeof(PurchaseOrderFilterType))]
        [UseSelection]
        public async Task<IQueryable<PurchaseOrderDto>> ProcessPurchaseOrdersAsync(IdsInput input, [Service] IPurchaseOrderQueries purchaseOrderQueries)
        {
            await ExecuteCommandAsync<ProcessPurchaseOrdersCommand, bool>(_mapper.Map(input, new ProcessPurchaseOrdersCommand(CurrentUser)), Token);
            return purchaseOrderQueries.GetPurchaseOrders(CurrentUser).Where(j => input.Ids.Contains(j.Id));
        }

        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLName("completePurchaseOrders")]
        [UsePaging]
        [UseSorting(SortType = typeof(PurchaseOrderSortType))]
        [UseFiltering(FilterType = typeof(PurchaseOrderFilterType))]
        [UseSelection]
        public async Task<IQueryable<PurchaseOrderDto>> CompletePurchaseOrdersAsync(IdsInput input, [Service] IPurchaseOrderQueries purchaseOrderQueries)
        {
            await ExecuteCommandAsync<CompletePurchaseOrdersCommand, bool>(_mapper.Map(input, new CompletePurchaseOrdersCommand(CurrentUser)), Token);
            return purchaseOrderQueries.GetPurchaseOrders(CurrentUser).Where(j => input.Ids.Contains(j.Id));
        }

        [Authorize(Policy = Policies.REGISTERED)]
        [GraphQLName("cancelPurchaseOrders")]
        [UsePaging]
        [UseSorting(SortType = typeof(PurchaseOrderSortType))]
        [UseFiltering(FilterType = typeof(PurchaseOrderFilterType))]
        [UseSelection]
        public async Task<IQueryable<PurchaseOrderDto>> CancelPurchaseOrdersAsync(IdsWithReasonInput input, [Service] IPurchaseOrderQueries purchaseOrderQueries)
        {
            await ExecuteCommandAsync<CancelPurchaseOrdersCommand, bool>(_mapper.Map(input, new CancelPurchaseOrdersCommand(CurrentUser)), Token);
            return purchaseOrderQueries.GetPurchaseOrders(CurrentUser).Where(j => input.Ids.Contains(j.Id));
        }

        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLName("refusePurchaseOrders")]
        [UsePaging]
        [UseSorting(SortType = typeof(PurchaseOrderSortType))]
        [UseFiltering(FilterType = typeof(PurchaseOrderFilterType))]
        [UseSelection]
        public async Task<IQueryable<PurchaseOrderDto>> RefusePurchaseOrdersAsync(IdsWithReasonInput input, [Service] IPurchaseOrderQueries purchaseOrderQueries)
        {
            await ExecuteCommandAsync<RefusePurchaseOrdersCommand, bool>(_mapper.Map(input, new RefusePurchaseOrdersCommand(CurrentUser)), Token);
            return purchaseOrderQueries.GetPurchaseOrders(CurrentUser).Where(j => input.Ids.Contains(j.Id));
        }

        [Authorize(Policy = Policies.REGISTERED)]
        [GraphQLName("deletePurchaseOrders")]
        public async Task<bool> DeletePurchaseOrdersAsync(IdsInput input)
        {
            return await ExecuteCommandAsync<DeletePurchaseOrdersCommand, bool>(_mapper.Map(input, new DeletePurchaseOrdersCommand(CurrentUser)), Token);
        }

        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLName("createProduct")]
        [GraphQLType(typeof(ProductDetailsType))]
        [UseSingleOrDefault]
        [UseSelection]
        public async Task<IQueryable<ProductDto>> CreateProductAsync(CreateProductInput input, [Service] IProductQueries productQueries)
        {
            var result = await ExecuteCommandAsync<CreateProductCommand, Guid>(_mapper.Map(input, new CreateProductCommand(CurrentUser)), Token);
            return productQueries.GetProduct(result, CurrentUser);
        }

        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLName("updateProduct")]
        [GraphQLType(typeof(ProductDetailsType))]
        [UseSingleOrDefault]
        [UseSelection]
        public async Task<IQueryable<ProductDto>> UpdateProductAsync(UpdateProductInput input, [Service] IProductQueries productQueries)
        {
            await ExecuteCommandAsync<UpdateProductCommand, bool>(_mapper.Map(input, new UpdateProductCommand(CurrentUser)), Token);
            return productQueries.GetProduct(input.Id, CurrentUser);
        }

        [Authorize(Policy = Policies.REGISTERED)]
        [GraphQLName("rateProduct")]
        [GraphQLType(typeof(ProductDetailsType))]
        [UseSingleOrDefault]
        [UseSelection]
        public async Task<IQueryable<ProductDto>> RateProductAsync(RateProductInput input, [Service] IProductQueries productQueries)
        {
            await ExecuteCommandAsync<RateProductCommand, bool>(_mapper.Map(input, new RateProductCommand(CurrentUser)), Token);
            return productQueries.GetProduct(input.Id, CurrentUser);
        }

        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLName("updateProductPicture")]
        [GraphQLType(typeof(ProductDetailsType))]
        [UseSingleOrDefault]
        [UseSelection]
        public async Task<IQueryable<ProductDto>> UpdateProductPictureAsync(UpdatePictureInput input, [Service] IProductQueries productQueries)
        {
            await ExecuteCommandAsync<UpdateProductPictureCommand, bool>(_mapper.Map(input, new UpdateProductPictureCommand(CurrentUser)), Token);
            return productQueries.GetProduct(input.Id, CurrentUser);
        }

        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLName("setProductsAvailability")]
        [GraphQLType(typeof(ListType<ProductType>))]
        [UsePaging]
        [UseSorting(SortType = typeof(ProductSortType))]
        [UseFiltering(FilterType = typeof(ProductFilterType))]
        [UseSelection]
        public async Task<IQueryable<ProductDto>> SetProductsAvailabilityAsync(SetProductsAvailabilityInput input, [Service] IProductQueries productQueries)
        {
            await ExecuteCommandAsync<SetProductsAvailabilityCommand, bool>(_mapper.Map(input, new SetProductsAvailabilityCommand(CurrentUser)), Token);
            return productQueries.GetProducts(CurrentUser).Where(j => input.Ids.Contains(j.Id));
        }

        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLName("deleteProducts")]
        public async Task<bool> DeleteProductsAsync(IdsInput input)
        {
            return await ExecuteCommandAsync<DeleteProductsCommand, bool>(_mapper.Map(input, new DeleteProductsCommand(CurrentUser)), Token);
        }

        [Authorize(Policy = Policies.AUTHENTICATED)]
        [GraphQLName("registerStore")]
        [GraphQLType(typeof(StoreType))]
        [UseSingleOrDefault]
        [UseSelection]
        public async Task<IQueryable<StoreDto>> RegisterStoreAsync(RegisterStoreInput input, [Service] IBusinessQueries businessQueries)
        {
            var result = await ExecuteCommandAsync<RegisterStoreCommand, Guid>(_mapper.Map(input, new RegisterStoreCommand(CurrentUser)), Token);
            return businessQueries.GetStore(result, CurrentUser);
        }

        [Authorize(Policy = Policies.STORE)]
        [GraphQLName("updateStore")]
        [GraphQLType(typeof(StoreType))]
        [UseSingleOrDefault]
        [UseSelection]
        public async Task<IQueryable<ProducerDto>> UpdateStoreAsync(UpdateStoreInput input, [Service] IBusinessQueries businessQueries)
        {
            await ExecuteCommandAsync<UpdateStoreCommand, bool>(_mapper.Map(input, new UpdateStoreCommand(CurrentUser)), Token);
            return businessQueries.GetStore(input.Id, CurrentUser);
        }

        [Authorize(Policy = Policies.AUTHENTICATED)]
        [GraphQLName("registerProducer")]
        [GraphQLType(typeof(ProducerType))]
        [UseSingleOrDefault]
        [UseSelection]
        public async Task<IQueryable<ProducerDto>> RegisterProducerAsync(RegisterProducerInput input, [Service] IBusinessQueries businessQueries)
        {
            var result = await ExecuteCommandAsync<RegisterProducerCommand, Guid>(_mapper.Map(input, new RegisterProducerCommand(CurrentUser)), Token);
            return businessQueries.GetProducer(result, CurrentUser);
        }

        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLName("updateProducer")]
        [GraphQLType(typeof(ProducerType))]
        [UseSingleOrDefault]
        [UseSelection]
        public async Task<IQueryable<ProducerDto>> UpdateProducerAsync(UpdateProducerInput input, [Service] IBusinessQueries businessQueries)
        {
            await ExecuteCommandAsync<UpdateProducerCommand, bool>(_mapper.Map(input, new UpdateProducerCommand(CurrentUser)), Token);
            return businessQueries.GetProducer(input.Id, CurrentUser);
        }

        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLName("createBusinessLegals")]
        [GraphQLType(typeof(BusinessLegalType))]
        [UseSingleOrDefault]
        [UseSelection]
        public async Task<IQueryable<BusinessLegalDto>> CreateBusinessLegalsAsync(CreateBusinessLegalInput input, [Service] ILegalQueries legalQueries)
        {
            var result = await ExecuteCommandAsync<CreateBusinessLegalCommand, Guid>(_mapper.Map(input, new CreateBusinessLegalCommand(CurrentUser)), Token);
            return legalQueries.GetBusinessLegals(result, CurrentUser);
        }

        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLName("updateBusinessLegals")]
        [GraphQLType(typeof(BusinessLegalType))]
        [UseSingleOrDefault]
        [UseSelection]
        public async Task<IQueryable<BusinessLegalDto>> UpdateBusinessLegalsAsync(UpdateBusinessLegalInput input, [Service] ILegalQueries legalQueries)
        {
            await ExecuteCommandAsync<UpdateBusinessLegalCommand, bool>(_mapper.Map(input, new UpdateBusinessLegalCommand(CurrentUser)), Token);
            return legalQueries.GetBusinessLegals(input.Id, CurrentUser);
        }

        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLName("createUbo")]
        [GraphQLType(typeof(UboType))]
        [UseSingleOrDefault]
        [UseSelection]
        public async Task<IQueryable<UboDto>> CreateUboAsync(CreateUboInput input, [Service] IUboQueries uboQueries)
        {
            var result = await ExecuteCommandAsync<CreateUboCommand, Guid>(_mapper.Map(input, new CreateUboCommand(CurrentUser)), Token);
            return uboQueries.GetUbo(result, CurrentUser);
        }

        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLName("updateUbo")]
        [GraphQLType(typeof(UboType))]
        [UseSingleOrDefault]
        [UseSelection]
        public async Task<IQueryable<UboDto>> UpdateUboAsync(UpdateUboInput input, [Service] IUboQueries uboQueries)
        {
            await ExecuteCommandAsync<UpdateUboCommand, bool>(_mapper.Map(input, new UpdateUboCommand(CurrentUser)), Token);
            return uboQueries.GetUbo(input.Id, CurrentUser);
        }

        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLName("removeUbo")]
        public async Task<bool> RemoveUboAsync(IdInput input)
        {
            return await ExecuteCommandAsync<RemoveUboCommand, bool>(_mapper.Map(input, new RemoveUboCommand(CurrentUser)), Token);
        }

        [Authorize(Policy = Policies.AUTHENTICATED)]
        [GraphQLName("registerConsumer")]
        [GraphQLType(typeof(ConsumerType))]
        [UseSingleOrDefault]
        [UseSelection]
        public async Task<IQueryable<ConsumerDto>> RegisterConsumerAsync(RegisterConsumerInput input, [Service] IConsumerQueries consumerQueries)
        {
            var result = await ExecuteCommandAsync<RegisterConsumerCommand, Guid>(_mapper.Map(input, new RegisterConsumerCommand(CurrentUser)), Token);
            return consumerQueries.GetConsumer(result, CurrentUser);
        }

        [Authorize(Policy = Policies.REGISTERED)]
        [GraphQLName("updateConsumer")]
        [GraphQLType(typeof(ConsumerType))]
        [UseSingleOrDefault]
        [UseSelection]
        public async Task<IQueryable<ConsumerDto>> UpdateConsumerAsync(UpdateConsumerInput input, [Service] IConsumerQueries consumerQueries)
        {
            await ExecuteCommandAsync<UpdateConsumerCommand, bool>(_mapper.Map(input, new UpdateConsumerCommand(CurrentUser)), Token);
            return consumerQueries.GetConsumer(input.Id, CurrentUser);
        }

        [Authorize(Policy = Policies.CONSUMER)]
        [GraphQLName("createConsumerLegals")]
        [GraphQLType(typeof(ConsumerLegalType))]
        [UseSingleOrDefault]
        [UseSelection]
        public async Task<IQueryable<ConsumerLegalDto>> CreateConsumerLegalsAsync(CreateConsumerLegalInput input, [Service] ILegalQueries legalQueries)
        {
            var result = await ExecuteCommandAsync<CreateConsumerLegalCommand, Guid>(_mapper.Map(input, new CreateConsumerLegalCommand(CurrentUser)), Token);
            return legalQueries.GetConsumerLegals(result, CurrentUser);
        }

        [Authorize(Policy = Policies.CONSUMER)]
        [GraphQLName("updateConsumerLegals")]
        [GraphQLType(typeof(ConsumerLegalType))]
        [UseSingleOrDefault]
        [UseSelection]
        public async Task<IQueryable<ConsumerLegalDto>> UpdateConsumerLegalsAsync(UpdateConsumerLegalInput input, [Service] ILegalQueries legalQueries)
        {
            await ExecuteCommandAsync<UpdateConsumerLegalCommand, bool>(_mapper.Map(input, new UpdateConsumerLegalCommand(CurrentUser)), Token);
            return legalQueries.GetConsumerLegals(input.Id, CurrentUser);
        }

        [Authorize(Policy = Policies.REGISTERED)]
        [GraphQLName("updateUserPicture")]
        public async Task<bool> UpdateUserPictureAsync(UpdatePictureInput input)
        {
            return await ExecuteCommandAsync<UpdateUserPictureCommand, bool>(_mapper.Map(input, new UpdateUserPictureCommand(CurrentUser)), Token);
        }

        [Authorize(Policy = Policies.REGISTERED)]
        [GraphQLName("deleteUser")]
        public async Task<bool> DeleteUserAsync(IdWithReasonInput input)
        {
            return await ExecuteCommandAsync<DeleteUserCommand, bool>(_mapper.Map(input, new DeleteUserCommand(CurrentUser)), Token);
        }

        [Authorize(Policy = Policies.STORE)]
        [GraphQLName("createQuickOrder")]
        [UseSingleOrDefault]
        [UseSelection]
        public async Task<IQueryable<QuickOrderDto>> CreateQuickOrderAsync(CreateQuickOrderInput input, [Service] IQuickOrderQueries quickOrderQueries)
        {
            var result = await ExecuteCommandAsync<CreateQuickOrderCommand, Guid>(_mapper.Map(input, new CreateQuickOrderCommand(CurrentUser)), Token);
            return quickOrderQueries.GetQuickOrder(result, CurrentUser);
        }

        [Authorize(Policy = Policies.STORE)]
        [GraphQLName("setDefaultQuickOrder")]
        [UseSingleOrDefault]
        [UseSelection]
        public async Task<IQueryable<QuickOrderDto>> SetDefaultQuickOrderAsync(IdInput input, [Service] IQuickOrderQueries quickOrderQueries)
        {
            await ExecuteCommandAsync<SetDefaultQuickOrderCommand, bool>(_mapper.Map(input, new SetDefaultQuickOrderCommand(CurrentUser)), Token);
            return quickOrderQueries.GetQuickOrder(input.Id, CurrentUser);
        }

        [Authorize(Policy = Policies.STORE)]
        [GraphQLName("updateQuickOrder")]
        [UseSingleOrDefault]
        [UseSelection]
        public async Task<IQueryable<QuickOrderDto>> UpdateQuickOrderAsync(UpdateQuickOrderInput input, [Service] IQuickOrderQueries quickOrderQueries)
        {
            await ExecuteCommandAsync<UpdateQuickOrderCommand, bool>(_mapper.Map(input, new UpdateQuickOrderCommand(CurrentUser)), Token);
            return quickOrderQueries.GetQuickOrder(input.Id, CurrentUser);
        }

        [Authorize(Policy = Policies.STORE)]
        [GraphQLName("updateQuickOrderProducts")]
        [UseSingleOrDefault]
        [UseSelection]
        public async Task<IQueryable<QuickOrderDto>> UpdateQuickOrderProductsAsync(UpdateIdProductsQuantitiesInput input, [Service] IQuickOrderQueries quickOrderQueries)
        {
            await ExecuteCommandAsync<UpdateQuickOrderProductsCommand, bool>(_mapper.Map(input, new UpdateQuickOrderProductsCommand(CurrentUser)), Token);
            return quickOrderQueries.GetQuickOrder(input.Id, CurrentUser);
        }

        [Authorize(Policy = Policies.STORE)]
        [GraphQLName("deleteQuickOrders")]
        public async Task<bool> DeleteQuickOrdersAsync(IdsWithReasonInput input)
        {
            return await ExecuteCommandAsync<DeleteQuickOrdersCommand, bool>(_mapper.Map(input, new DeleteQuickOrdersCommand(CurrentUser)), Token);
        }

        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLName("createDeliveryMode")]
        [UseSingleOrDefault]
        [UseSelection]
        public async Task<IQueryable<DeliveryModeDto>> CreateDeliveryModeAsync(CreateDeliveryModeInput input, [Service] IDeliveryQueries deliveryQueries)
        {
            var result = await ExecuteCommandAsync<CreateDeliveryModeCommand, Guid>(_mapper.Map(input, new CreateDeliveryModeCommand(CurrentUser)), Token);
            return deliveryQueries.GetDelivery(result, CurrentUser);
        }

        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLName("updateDeliveryMode")]
        [UseSingleOrDefault]
        [UseSelection]
        public async Task<IQueryable<DeliveryModeDto>>UpdateDeliveryModeAsync(UpdateDeliveryModeInput input, [Service] IDeliveryQueries deliveryQueries)
        {
            await ExecuteCommandAsync<UpdateDeliveryModeCommand, bool>(_mapper.Map(input, new UpdateDeliveryModeCommand(CurrentUser)), Token);
            return deliveryQueries.GetDelivery(input.Id, CurrentUser);
        }

        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLName("deleteDeliveryMode")]
        public async Task<bool> DeleteDeliveryModeAsync(IdInput input)
        {
            return await ExecuteCommandAsync<DeleteDeliveryModeCommand, bool>(_mapper.Map(input, new DeleteDeliveryModeCommand(CurrentUser)), Token);
        }

        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLName("createReturnable")]
        [UseSingleOrDefault]
        [UseSelection]
        public async Task<IQueryable<ReturnableDto>> CreateReturnableAsync(CreateReturnableInput input, [Service] IReturnableQueries returnableQueries)
        {
            var result = await ExecuteCommandAsync<CreateReturnableCommand, Guid>(_mapper.Map(input, new CreateReturnableCommand(CurrentUser)), Token);
            return returnableQueries.GetReturnable(result, CurrentUser);
        }

        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLName("updateReturnable")]
        [UseSingleOrDefault]
        [UseSelection]
        public async Task<IQueryable<ReturnableDto>> UpdateReturnableAsync(UpdateReturnableInput input, [Service] IReturnableQueries returnableQueries)
        {
            await ExecuteCommandAsync<UpdateReturnableCommand, bool>(_mapper.Map(input, new UpdateReturnableCommand(CurrentUser)), Token);
            return returnableQueries.GetReturnable(input.Id, CurrentUser);
        }

        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLName("deleteReturnable")]
        public async Task<bool> DeleteReturnableAsync(IdInput input)
        {
            return await ExecuteCommandAsync<DeleteReturnableCommand, bool>(_mapper.Map(input, new DeleteReturnableCommand(CurrentUser)), Token);
        }

        private async Task<T> ExecuteCommandAsync<U, T>(U input, CancellationToken token) where U : Command<T>
        {
            var commandName = typeof(U).Name;
            _logger.LogTrace($"{nameof(SheaftMutation.ExecuteCommandAsync)} - {commandName}");

            var result = await _mediator.Send(input, token);
            _logger.LogTrace($"{nameof(SheaftMutation.ExecuteCommandAsync)} - {commandName} - {result.Success}");

            if (result.Success)
                return result.Data;

            if (result.Exception != null)
            {
                _logger.LogError(result.Exception, $"{nameof(SheaftMutation.ExecuteCommandAsync)} - {commandName} - {result.Exception.Message}");
                throw result.Exception;
            }

            throw new UnexpectedException(result.Message);
        }
    }
}
