using MediatR;
using Sheaft.Application.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Sheaft.Application.Queries;
using AutoMapper;
using Sheaft.Models.Inputs;
using Sheaft.Models.Dto;
using Sheaft.Core.Extensions;
using Sheaft.Core.Security;
using HotChocolate;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using Microsoft.AspNetCore.Http;
using HotChocolate.AspNetCore.Authorization;
using Sheaft.GraphQL.Types.Filters;
using Sheaft.GraphQL.Types.Sorts;
using Sheaft.GraphQL.Types;
using Microsoft.Extensions.Logging;
using Sheaft.Exceptions;
using Sheaft.Core;

namespace Sheaft.GraphQL
{
    public class SheaftMutation
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<SheaftMutation> _logger;
        private CancellationToken _cancellationToken => _httpContextAccessor.HttpContext.RequestAborted;
        private RequestUser _currentUser
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
            return await ExecuteCommandAsync<GenerateUserSponsoringCodeCommand, string>(_mapper.Map(input, new GenerateUserSponsoringCodeCommand(_currentUser)), _cancellationToken);            
        }

        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLName("exportPickingFromOrders")]
        [UseSingleOrDefault]
        [UseSelection]
        public async Task<IQueryable<JobDto>> ExportPickingOrdersAsync(ExportPickingOrdersInput input, [Service] IJobQueries jobQueries)
        {
            var result = await ExecuteCommandAsync<QueueExportPickingOrderCommand, Guid>(_mapper.Map(input, new QueueExportPickingOrderCommand(_currentUser)), _cancellationToken);
            return jobQueries.GetJob(result, _currentUser);
        }

        [Authorize(Policy = Policies.REGISTERED)]
        [GraphQLName("exportRGPD")]
        [UseSingleOrDefault]
        [UseSelection]
        public async Task<IQueryable<JobDto>> ExportAccountDatasAsync(IdInput input, [Service] IJobQueries jobQueries)
        {
            var result = await ExecuteCommandAsync<QueueExportAccountDataCommand, Guid>(_mapper.Map(input, new QueueExportAccountDataCommand(_currentUser)), _cancellationToken);
            return jobQueries.GetJob(result, _currentUser);
        }

        [Authorize(Policy = Policies.REGISTERED)]
        [GraphQLName("resumeJobs")]
        [UsePaging]
        [UseSorting(SortType = typeof(JobSortType))]
        [UseFiltering(FilterType = typeof(JobFilterType))]
        [UseSelection]
        public async Task<IQueryable<JobDto>> ResumeJobsAsync(IdsInput input, [Service] IJobQueries jobQueries)
        {
            var result = await ExecuteCommandAsync<ResumeJobsCommand, bool>(_mapper.Map(input, new ResumeJobsCommand(_currentUser)), _cancellationToken);
            return jobQueries.GetJobs(_currentUser).Where(j => input.Ids.Contains(j.Id));
        }

        [Authorize(Policy = Policies.REGISTERED)]
        [GraphQLName("pauseJobs")]
        [UsePaging]
        [UseSorting(SortType = typeof(JobSortType))]
        [UseFiltering(FilterType = typeof(JobFilterType))]
        [UseSelection]
        public async Task<IQueryable<JobDto>> PauseJobsAsync(IdsInput input, [Service] IJobQueries jobQueries)
        {
            var result = await ExecuteCommandAsync<PauseJobsCommand, bool>(_mapper.Map(input, new PauseJobsCommand(_currentUser)), _cancellationToken);
            return jobQueries.GetJobs(_currentUser).Where(j => input.Ids.Contains(j.Id));
        }

        [Authorize(Policy = Policies.REGISTERED)]
        [GraphQLName("retryJobs")]
        [UsePaging]
        [UseSorting(SortType = typeof(JobSortType))]
        [UseFiltering(FilterType = typeof(JobFilterType))]
        [UseSelection]
        public async Task<IQueryable<JobDto>> RetryJobsAsync(IdsInput input, [Service] IJobQueries jobQueries)
        {
            var result = await ExecuteCommandAsync<RetryJobsCommand, bool>(_mapper.Map(input, new RetryJobsCommand(_currentUser)), _cancellationToken);
            return jobQueries.GetJobs(_currentUser).Where(j => input.Ids.Contains(j.Id));
        }

        [Authorize(Policy = Policies.REGISTERED)]
        [GraphQLName("cancelJobs")]
        [UsePaging]
        [UseSorting(SortType = typeof(JobSortType))]
        [UseFiltering(FilterType = typeof(JobFilterType))]
        [UseSelection]
        public async Task<IQueryable<JobDto>> CancelJobsAsync(IdsWithReasonInput input, [Service] IJobQueries jobQueries)
        {
            var result = await ExecuteCommandAsync<CancelJobsCommand, bool>(_mapper.Map(input, new CancelJobsCommand(_currentUser)), _cancellationToken);
            return jobQueries.GetJobs(_currentUser).Where(j => input.Ids.Contains(j.Id));
        }

        [Authorize(Policy = Policies.REGISTERED)]
        [GraphQLName("archiveJobs")]
        [UsePaging]
        [UseSorting(SortType = typeof(JobSortType))]
        [UseFiltering(FilterType = typeof(JobFilterType))]
        [UseSelection]
        public async Task<IQueryable<JobDto>> ArchiveJobsAsync(IdsInput input, [Service] IJobQueries jobQueries)
        {
            var result = await ExecuteCommandAsync<ArchiveJobsCommand, bool>(_mapper.Map(input, new ArchiveJobsCommand(_currentUser)), _cancellationToken);
            return jobQueries.GetJobs(_currentUser).Where(j => input.Ids.Contains(j.Id));
        }
        
        [Authorize(Policy = Policies.STORE_OR_PRODUCER)]
        [GraphQLName("createAgreement")]
        [UseSingleOrDefault]
        [UseSelection]
        public async Task<IQueryable<AgreementDto>> CreateAgreementAsync(CreateAgreementInput input, [Service] IAgreementQueries agreementQueries)
        {
            var result = await ExecuteCommandAsync<CreateAgreementCommand, Guid>(_mapper.Map(input, new CreateAgreementCommand(_currentUser)), _cancellationToken);
            return agreementQueries.GetAgreement(result, _currentUser);
        }
        
        [Authorize(Policy = Policies.STORE_OR_PRODUCER)]
        [GraphQLName("acceptAgreement")]
        [UseSingleOrDefault]
        [UseSelection]
        public async Task<IQueryable<AgreementDto>> AcceptAgreementAsync(IdTimeSlotGroupInput input, [Service] IAgreementQueries agreementQueries)
        {
            await ExecuteCommandAsync<AcceptAgreementCommand, bool>(_mapper.Map(input, new AcceptAgreementCommand(_currentUser)), _cancellationToken);
            return agreementQueries.GetAgreement(input.Id, _currentUser);
        }
        
        [Authorize(Policy = Policies.STORE_OR_PRODUCER)]
        [GraphQLName("cancelAgreements")]
        [UsePaging]
        [UseSorting(SortType = typeof(AgreementSortType))]
        [UseFiltering(FilterType = typeof(AgreementFilterType))]
        [UseSelection]
        public async Task<IQueryable<AgreementDto>> CancelAgreementsAsync(IdsWithReasonInput input, [Service] IAgreementQueries agreementQueries)
        {
            await ExecuteCommandAsync<CancelAgreementsCommand, bool>(_mapper.Map(input, new CancelAgreementsCommand(_currentUser)), _cancellationToken);
            return agreementQueries.GetAgreements(_currentUser).Where(j => input.Ids.Contains(j.Id));
        }
        
        [Authorize(Policy = Policies.STORE_OR_PRODUCER)]
        [GraphQLName("refuseAgreements")]
        [UsePaging]
        [UseSorting(SortType = typeof(AgreementSortType))]
        [UseFiltering(FilterType = typeof(AgreementFilterType))]
        [UseSelection]
        public async Task<IQueryable<AgreementDto>> RefuseAgreementsAsync(IdsWithReasonInput input, [Service] IAgreementQueries agreementQueries)
        {
            await ExecuteCommandAsync<RefuseAgreementsCommand, bool>(_mapper.Map(input, new RefuseAgreementsCommand(_currentUser)), _cancellationToken);
            return agreementQueries.GetAgreements(_currentUser).Where(j => input.Ids.Contains(j.Id));
        }

        [Authorize(Policy = Policies.REGISTERED)]
        [GraphQLName("markUserNotificationsAsRead")]
        public async Task<DateTimeOffset> MarkMyNotificationsAsReadAsync()
        {
            var input = new MarkUserNotificationsAsReadCommand(_currentUser) { ReadBefore = DateTimeOffset.UtcNow };
            await ExecuteCommandAsync<MarkUserNotificationsAsReadCommand, bool>(input, _cancellationToken);
            return input.ReadBefore;
        }

        [Authorize(Policy = Policies.REGISTERED)]
        [GraphQLName("markUserNotificationAsRead")]
        [UseSingleOrDefault]
        [UseSelection]
        public async Task<IQueryable<NotificationDto>> MarkNotificationAsReadAsync(IdInput input, [Service] INotificationQueries notificationQueries)
        {
            await ExecuteCommandAsync<MarkUserNotificationAsReadCommand, bool>(_mapper.Map(input, new MarkUserNotificationAsReadCommand(_currentUser)), _cancellationToken);
            return notificationQueries.GetNotification(input.Id, _currentUser);
        }

        [Authorize(Policy = Policies.REGISTERED)]
        [GraphQLName("createPurchaseOrders")]
        [UsePaging]
        [UseSorting(SortType = typeof(PurchaseOrderSortType))]
        [UseFiltering(FilterType = typeof(PurchaseOrderFilterType))]
        [UseSelection]
        public async Task<IQueryable<PurchaseOrderDto>> CreatePurchaseOrdersAsync(CreatePurchaseOrdersInput input, [Service] IPurchaseOrderQueries purchaseOrderQueries)
        {
            var result = await ExecuteCommandAsync<CreatePurchaseOrdersCommand, IEnumerable<Guid>>(_mapper.Map(input, new CreatePurchaseOrdersCommand(_currentUser)), _cancellationToken);
            return purchaseOrderQueries.GetPurchaseOrders(_currentUser).Where(j => result.Contains(j.Id));
        }

        [Authorize(Policy = Policies.REGISTERED)]
        [GraphQLName("updatePurchaseOrderProducts")]
        [UseSingleOrDefault]
        [UseSelection]
        public async Task<IQueryable<PurchaseOrderDto>> UpdatePurchaseOrderProductsAsync(UpdateIdProductsQuantitiesInput input, [Service] IPurchaseOrderQueries purchaseOrderQueries)
        {
            await ExecuteCommandAsync<UpdatePurchaseOrderProductsCommand, bool>(_mapper.Map(input, new UpdatePurchaseOrderProductsCommand(_currentUser)), _cancellationToken);
            return purchaseOrderQueries.GetPurchaseOrder(input.Id, _currentUser);
        }

        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLName("acceptPurchaseOrders")]
        [UsePaging]
        [UseSorting(SortType = typeof(PurchaseOrderSortType))]
        [UseFiltering(FilterType = typeof(PurchaseOrderFilterType))]
        [UseSelection]
        public async Task<IQueryable<PurchaseOrderDto>> AcceptPurchaseOrdersAsync(IdsInput input, [Service] IPurchaseOrderQueries purchaseOrderQueries)
        {
            await ExecuteCommandAsync<AcceptPurchaseOrdersCommand, bool>(_mapper.Map(input, new AcceptPurchaseOrdersCommand(_currentUser)), _cancellationToken);
            return purchaseOrderQueries.GetPurchaseOrders(_currentUser).Where(j => input.Ids.Contains(j.Id));
        }

        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLName("shipPurchaseOrders")]
        [UsePaging]
        [UseSorting(SortType = typeof(PurchaseOrderSortType))]
        [UseFiltering(FilterType = typeof(PurchaseOrderFilterType))]
        [UseSelection]
        public async Task<IQueryable<PurchaseOrderDto>> ShipPurchaseOrdersAsync(IdsInput input, [Service] IPurchaseOrderQueries purchaseOrderQueries)
        {
            await ExecuteCommandAsync<ShipPurchaseOrdersCommand, bool>(_mapper.Map(input, new ShipPurchaseOrdersCommand(_currentUser)), _cancellationToken);
            return purchaseOrderQueries.GetPurchaseOrders(_currentUser).Where(j => input.Ids.Contains(j.Id));
        }

        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLName("deliverPurchaseOrders")]
        [UsePaging]
        [UseSorting(SortType = typeof(PurchaseOrderSortType))]
        [UseFiltering(FilterType = typeof(PurchaseOrderFilterType))]
        [UseSelection]
        public async Task<IQueryable<PurchaseOrderDto>> DeliverPurchaseOrdersAsync(IdsInput input, [Service] IPurchaseOrderQueries purchaseOrderQueries)
        {
            await ExecuteCommandAsync<DeliverPurchaseOrdersCommand, bool>(_mapper.Map(input, new DeliverPurchaseOrdersCommand(_currentUser)), _cancellationToken);
            return purchaseOrderQueries.GetPurchaseOrders(_currentUser).Where(j => input.Ids.Contains(j.Id));
        }

        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLName("processPurchaseOrders")]
        [UsePaging]
        [UseSorting(SortType = typeof(PurchaseOrderSortType))]
        [UseFiltering(FilterType = typeof(PurchaseOrderFilterType))]
        [UseSelection]
        public async Task<IQueryable<PurchaseOrderDto>> ProcessPurchaseOrdersAsync(IdsInput input, [Service] IPurchaseOrderQueries purchaseOrderQueries)
        {
            await ExecuteCommandAsync<ProcessPurchaseOrdersCommand, bool>(_mapper.Map(input, new ProcessPurchaseOrdersCommand(_currentUser)), _cancellationToken);
            return purchaseOrderQueries.GetPurchaseOrders(_currentUser).Where(j => input.Ids.Contains(j.Id));
        }

        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLName("completePurchaseOrders")]
        [UsePaging]
        [UseSorting(SortType = typeof(PurchaseOrderSortType))]
        [UseFiltering(FilterType = typeof(PurchaseOrderFilterType))]
        [UseSelection]
        public async Task<IQueryable<PurchaseOrderDto>> CompletePurchaseOrdersAsync(IdsInput input, [Service] IPurchaseOrderQueries purchaseOrderQueries)
        {
            await ExecuteCommandAsync<CompletePurchaseOrdersCommand, bool>(_mapper.Map(input, new CompletePurchaseOrdersCommand(_currentUser)), _cancellationToken);
            return purchaseOrderQueries.GetPurchaseOrders(_currentUser).Where(j => input.Ids.Contains(j.Id));
        }

        [Authorize(Policy = Policies.REGISTERED)]
        [GraphQLName("cancelPurchaseOrders")]
        [UsePaging]
        [UseSorting(SortType = typeof(PurchaseOrderSortType))]
        [UseFiltering(FilterType = typeof(PurchaseOrderFilterType))]
        [UseSelection]
        public async Task<IQueryable<PurchaseOrderDto>> CancelPurchaseOrdersAsync(IdsWithReasonInput input, [Service] IPurchaseOrderQueries purchaseOrderQueries)
        {
            await ExecuteCommandAsync<CancelPurchaseOrdersCommand, bool>(_mapper.Map(input, new CancelPurchaseOrdersCommand(_currentUser)), _cancellationToken);
            return purchaseOrderQueries.GetPurchaseOrders(_currentUser).Where(j => input.Ids.Contains(j.Id));
        }

        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLName("refusePurchaseOrders")]
        [UsePaging]
        [UseSorting(SortType = typeof(PurchaseOrderSortType))]
        [UseFiltering(FilterType = typeof(PurchaseOrderFilterType))]
        [UseSelection]
        public async Task<IQueryable<PurchaseOrderDto>> RefusePurchaseOrdersAsync(IdsWithReasonInput input, [Service] IPurchaseOrderQueries purchaseOrderQueries)
        {
            await ExecuteCommandAsync<RefusePurchaseOrdersCommand, bool>(_mapper.Map(input, new RefusePurchaseOrdersCommand(_currentUser)), _cancellationToken);
            return purchaseOrderQueries.GetPurchaseOrders(_currentUser).Where(j => input.Ids.Contains(j.Id));
        }

        [Authorize(Policy = Policies.REGISTERED)]
        [GraphQLName("deletePurchaseOrders")]
        public async Task<bool> DeletePurchaseOrdersAsync(IdsInput input)
        {
            return await ExecuteCommandAsync<DeletePurchaseOrdersCommand, bool>(_mapper.Map(input, new DeletePurchaseOrdersCommand(_currentUser)), _cancellationToken);
        }

        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLName("createProduct")]
        [GraphQLType(typeof(ProductDetailsType))]
        [UseSingleOrDefault]
        [UseSelection]
        public async Task<IQueryable<ProductDto>> CreateProductAsync(CreateProductInput input, [Service] IProductQueries productQueries)
        {
            var result = await ExecuteCommandAsync<CreateProductCommand, Guid>(_mapper.Map(input, new CreateProductCommand(_currentUser)), _cancellationToken);
            return productQueries.GetProduct(result, _currentUser);
        }

        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLName("updateProduct")]
        [GraphQLType(typeof(ProductDetailsType))]
        [UseSingleOrDefault]
        [UseSelection]
        public async Task<IQueryable<ProductDto>> UpdateProductAsync(UpdateProductInput input, [Service] IProductQueries productQueries)
        {
            await ExecuteCommandAsync<UpdateProductCommand, bool>(_mapper.Map(input, new UpdateProductCommand(_currentUser)), _cancellationToken);
            return productQueries.GetProduct(input.Id, _currentUser);
        }

        [Authorize(Policy = Policies.CONSUMER)]
        [GraphQLName("rateProduct")]
        [GraphQLType(typeof(ProductDetailsType))]
        [UseSingleOrDefault]
        [UseSelection]
        public async Task<IQueryable<ProductDto>> RateProductAsync(RateProductInput input, [Service] IProductQueries productQueries)
        {
            await ExecuteCommandAsync<RateProductCommand, bool>(_mapper.Map(input, new RateProductCommand(_currentUser)), _cancellationToken);
            return productQueries.GetProduct(input.Id, _currentUser);
        }

        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLName("updateProductPicture")]
        [GraphQLType(typeof(ProductDetailsType))]
        [UseSingleOrDefault]
        [UseSelection]
        public async Task<IQueryable<ProductDto>> UpdateProductPictureAsync(UpdatePictureInput input, [Service] IProductQueries productQueries)
        {
            await ExecuteCommandAsync<UpdateProductPictureCommand, bool>(_mapper.Map(input, new UpdateProductPictureCommand(_currentUser)), _cancellationToken);
            return productQueries.GetProduct(input.Id, _currentUser);
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
            await ExecuteCommandAsync<SetProductsAvailabilityCommand, bool>(_mapper.Map(input, new SetProductsAvailabilityCommand(_currentUser)), _cancellationToken);
            return productQueries.GetProducts(_currentUser).Where(j => input.Ids.Contains(j.Id));
        }

        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLName("deleteProducts")]
        public async Task<bool> DeleteProductsAsync(IdsInput input)
        {
            return await ExecuteCommandAsync<DeleteProductsCommand, bool>(_mapper.Map(input, new DeleteProductsCommand(_currentUser)), _cancellationToken);
        }

        [Authorize(Policy = Policies.AUTHENTICATED)]
        [GraphQLName("registerCompany")]
        [GraphQLType(typeof(CompanyType))]
        [UseSingleOrDefault]
        [UseSelection]
        public async Task<IQueryable<CompanyDto>> RegisterCompanyAsync(RegisterCompanyInput input, [Service] ICompanyQueries companyQueries)
        {
            var result = await ExecuteCommandAsync<RegisterCompanyCommand, Guid>(_mapper.Map(input, new RegisterCompanyCommand(_currentUser)), _cancellationToken);
            return companyQueries.GetCompany(result, _currentUser);
        }

        [Authorize(Policy = Policies.OWNER)]
        [GraphQLName("updateCompany")]
        [GraphQLType(typeof(CompanyType))]
        [UseSingleOrDefault]
        [UseSelection]
        public async Task<IQueryable<CompanyDto>> UpdateCompanyAsync(UpdateCompanyInput input, [Service] ICompanyQueries companyQueries)
        {
            await ExecuteCommandAsync<UpdateCompanyCommand, bool>(_mapper.Map(input, new UpdateCompanyCommand(_currentUser)), _cancellationToken);
            return companyQueries.GetCompany(input.Id, _currentUser);
        }

        [Authorize(Policy = Policies.OWNER)]
        [GraphQLName("updateCompanyPicture")]
        [GraphQLType(typeof(CompanyType))]
        [UseSingleOrDefault]
        [UseSelection]
        public async Task<IQueryable<CompanyDto>> UpdateCompanyPictureAsync(UpdatePictureInput input, [Service] ICompanyQueries companyQueries)
        {
            await ExecuteCommandAsync<UpdateCompanyPictureCommand, bool>(_mapper.Map(input, new UpdateCompanyPictureCommand(_currentUser)), _cancellationToken);
            return companyQueries.GetCompany(input.Id, _currentUser);
        }

        [Authorize(Policy = Policies.OWNER)]
        [GraphQLName("deleteCompany")]
        public async Task<bool> DeleteCompanyAsync(IdWithReasonInput input)
        {
            return await ExecuteCommandAsync<DeleteCompanyCommand, bool>(_mapper.Map(input, new DeleteCompanyCommand(_currentUser)), _cancellationToken);
        }

        [Authorize(Policy = Policies.AUTHENTICATED)]
        [GraphQLName("registerConsumer")]
        [GraphQLType(typeof(UserType))]
        [UseSingleOrDefault]
        [UseSelection]
        public async Task<IQueryable<UserDto>> RegisterConsumerAsync(RegisterConsumerInput input, [Service] IUserQueries userQueries)
        {
            var result = await ExecuteCommandAsync<RegisterConsumerCommand, Guid>(_mapper.Map(input, new RegisterConsumerCommand(_currentUser)), _cancellationToken);
            return userQueries.GetUserProfile(result, _currentUser);
        }

        [Authorize(Policy = Policies.REGISTERED)]
        [GraphQLName("updateUser")]
        [GraphQLType(typeof(UserType))]
        [UseSingleOrDefault]
        [UseSelection]
        public async Task<IQueryable<UserDto>> UpdateUserAsync(UpdateUserInput input, [Service] IUserQueries userQueries)
        {
            await ExecuteCommandAsync<UpdateUserCommand, bool>(_mapper.Map(input, new UpdateUserCommand(_currentUser)), _cancellationToken);
            return userQueries.GetUserProfile(input.Id, _currentUser);
        }

        [Authorize(Policy = Policies.REGISTERED)]
        [GraphQLName("updateUserPicture")]
        [GraphQLType(typeof(UserType))]
        [UseSingleOrDefault]
        [UseSelection]
        public async Task<IQueryable<UserDto>> UpdateUserPictureAsync(UpdatePictureInput input, [Service] IUserQueries userQueries)
        {
            await ExecuteCommandAsync<UpdateUserPictureCommand, bool>(_mapper.Map(input, new UpdateUserPictureCommand(_currentUser)), _cancellationToken);
            return userQueries.GetUserProfile(input.Id, _currentUser);
        }

        [Authorize(Policy = Policies.REGISTERED)]
        [GraphQLName("deleteUser")]
        public async Task<bool> DeleteUserAsync(IdWithReasonInput input)
        {
            return await ExecuteCommandAsync<DeleteUserCommand, bool>(_mapper.Map(input, new DeleteUserCommand(_currentUser)), _cancellationToken);
        }

        [Authorize(Policy = Policies.STORE)]
        [GraphQLName("createQuickOrder")]
        [UseSingleOrDefault]
        [UseSelection]
        public async Task<IQueryable<QuickOrderDto>> CreateQuickOrderAsync(CreateQuickOrderInput input, [Service] IQuickOrderQueries quickOrderQueries)
        {
            var result = await ExecuteCommandAsync<CreateQuickOrderCommand, Guid>(_mapper.Map(input, new CreateQuickOrderCommand(_currentUser)), _cancellationToken);
            return quickOrderQueries.GetQuickOrder(result, _currentUser);
        }

        [Authorize(Policy = Policies.STORE)]
        [GraphQLName("setDefaultQuickOrder")]
        [UseSingleOrDefault]
        [UseSelection]
        public async Task<IQueryable<QuickOrderDto>> SetDefaultQuickOrderAsync(IdInput input, [Service] IQuickOrderQueries quickOrderQueries)
        {
            await ExecuteCommandAsync<SetDefaultQuickOrderCommand, bool>(_mapper.Map(input, new SetDefaultQuickOrderCommand(_currentUser)), _cancellationToken);
            return quickOrderQueries.GetQuickOrder(input.Id, _currentUser);
        }

        [Authorize(Policy = Policies.STORE)]
        [GraphQLName("updateQuickOrder")]
        [UseSingleOrDefault]
        [UseSelection]
        public async Task<IQueryable<QuickOrderDto>> UpdateQuickOrderAsync(UpdateQuickOrderInput input, [Service] IQuickOrderQueries quickOrderQueries)
        {
            await ExecuteCommandAsync<UpdateQuickOrderCommand, bool>(_mapper.Map(input, new UpdateQuickOrderCommand(_currentUser)), _cancellationToken);
            return quickOrderQueries.GetQuickOrder(input.Id, _currentUser);
        }

        [Authorize(Policy = Policies.STORE)]
        [GraphQLName("updateQuickOrderProducts")]
        [UseSingleOrDefault]
        [UseSelection]
        public async Task<IQueryable<QuickOrderDto>> UpdateQuickOrderProductsAsync(UpdateIdProductsQuantitiesInput input, [Service] IQuickOrderQueries quickOrderQueries)
        {
            await ExecuteCommandAsync<UpdateQuickOrderProductsCommand, bool>(_mapper.Map(input, new UpdateQuickOrderProductsCommand(_currentUser)), _cancellationToken);
            return quickOrderQueries.GetQuickOrder(input.Id, _currentUser);
        }

        [Authorize(Policy = Policies.STORE)]
        [GraphQLName("deleteQuickOrders")]
        public async Task<bool> DeleteQuickOrdersAsync(IdsWithReasonInput input)
        {
            return await ExecuteCommandAsync<DeleteQuickOrdersCommand, bool>(_mapper.Map(input, new DeleteQuickOrdersCommand(_currentUser)), _cancellationToken);
        }

        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLName("createDeliveryMode")]
        [UseSingleOrDefault]
        [UseSelection]
        public async Task<IQueryable<DeliveryModeDto>> CreateDeliveryModeAsync(CreateDeliveryModeInput input, [Service] IDeliveryQueries deliveryQueries)
        {
            var result = await ExecuteCommandAsync<CreateDeliveryModeCommand, Guid>(_mapper.Map(input, new CreateDeliveryModeCommand(_currentUser)), _cancellationToken);
            return deliveryQueries.GetDelivery(result, _currentUser);
        }

        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLName("updateDeliveryMode")]
        [UseSingleOrDefault]
        [UseSelection]
        public async Task<IQueryable<DeliveryModeDto>>UpdateDeliveryModeAsync(UpdateDeliveryModeInput input, [Service] IDeliveryQueries deliveryQueries)
        {
            await ExecuteCommandAsync<UpdateDeliveryModeCommand, bool>(_mapper.Map(input, new UpdateDeliveryModeCommand(_currentUser)), _cancellationToken);
            return deliveryQueries.GetDelivery(input.Id, _currentUser);
        }

        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLName("deleteDeliveryMode")]
        public async Task<bool> DeleteDeliveryModeAsync(IdInput input)
        {
            return await ExecuteCommandAsync<DeleteDeliveryModeCommand, bool>(_mapper.Map(input, new DeleteDeliveryModeCommand(_currentUser)), _cancellationToken);
        }

        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLName("createPackaging")]
        [UseSingleOrDefault]
        [UseSelection]
        public async Task<IQueryable<PackagingDto>> CreatePackagingAsync(CreatePackagingInput input, [Service] IPackagingQueries packagingQueries)
        {
            var result = await ExecuteCommandAsync<CreatePackagingCommand, Guid>(_mapper.Map(input, new CreatePackagingCommand(_currentUser)), _cancellationToken);
            return packagingQueries.GetPackaging(result, _currentUser);
        }

        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLName("updatePackaging")]
        [UseSingleOrDefault]
        [UseSelection]
        public async Task<IQueryable<PackagingDto>> UpdatePackagingAsync(UpdatePackagingInput input, [Service] IPackagingQueries packagingQueries)
        {
            await ExecuteCommandAsync<UpdatePackagingCommand, bool>(_mapper.Map(input, new UpdatePackagingCommand(_currentUser)), _cancellationToken);
            return packagingQueries.GetPackaging(input.Id, _currentUser);
        }

        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLName("deletePackaging")]
        public async Task<bool> DeletePackagingAsync(IdInput input)
        {
            return await ExecuteCommandAsync<DeletePackagingCommand, bool>(_mapper.Map(input, new DeletePackagingCommand(_currentUser)), _cancellationToken);
        }

        private async Task<T> ExecuteCommandAsync<U, T>(U input, CancellationToken token) where U : Command<T>
        {
            var commandName = typeof(U).Name;
            _logger.LogTrace($"{nameof(SheaftMutation.ExecuteCommandAsync)} - {commandName}");

            var result = await _mediator.Send(input, token);
            _logger.LogTrace($"{nameof(SheaftMutation.ExecuteCommandAsync)} - {commandName} - {result.Success}");

            if (result.Success)
                return result.Result;

            if (result.Exception != null)
            {
                _logger.LogError(result.Exception, $"{nameof(SheaftMutation.ExecuteCommandAsync)} - {commandName} - {result.Exception.Message}");
                throw result.Exception;
            }

            throw new UnexpectedException(result.Message);
        }
    }
}
