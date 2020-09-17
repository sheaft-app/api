using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Sheaft.Application.Queries;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading;
using Sheaft.Models.Dto;
using Sheaft.Models.Inputs;
using Sheaft.Core;
using Sheaft.Core.Extensions;
using Sheaft.Core.Security;
using HotChocolate.Types.Relay;
using HotChocolate;
using HotChocolate.Types;
using HotChocolate.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Sheaft.GraphQL.Types;
using Sheaft.GraphQL.Sorts;
using Sheaft.GraphQL.Filters;

namespace Sheaft.GraphQL
{
    public class SheaftQuery
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<SheaftQuery> _logger;
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

        public SheaftQuery(IHttpContextAccessor httpContextAccessor, ILogger<SheaftQuery> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        [Authorize(Policy = Policies.AUTHENTICATED)]
        [GraphQLName("generateFreshdeskToken")]
        public async Task<string> GetFreshdeskTokenAsync([Service] IUserQueries userQueries)
        {
            return await userQueries.GetFreshdeskTokenAsync(_currentUser, _cancellationToken);
        }

        [Authorize(Policy = Policies.CONSUMER)]
        [GraphQLName("myRankInformation")]
        [GraphQLType(typeof(RankInformationType))]
        public async Task<RankInformationDto> GetMyRankInformationAsync([Service] ILeaderboardQueries leaderboardQueries)
        {
            return await leaderboardQueries.UserRankInformationAsync(_currentUser.Id, _currentUser, _cancellationToken);
        }

        [Authorize(Policy = Policies.CONSUMER)]
        [GraphQLName("userPositionInDepartment")]
        [GraphQLType(typeof(UserPositionType))]
        public async Task<UserPositionDto> GetMyPositionInDepartment([Service] ILeaderboardQueries leaderboardQueries)
        {
            return await leaderboardQueries.UserPositionInDepartmentAsync(_currentUser.Id, _currentUser, _cancellationToken);
        }

        [Authorize(Policy = Policies.CONSUMER)]
        [GraphQLName("userPositionInRegion")]
        [GraphQLType(typeof(UserPositionType))]
        public async Task<UserPositionDto> GetMyPositionInRegion([Service] ILeaderboardQueries leaderboardQueries)
        {
            return await leaderboardQueries.UserPositionInRegionAsync(_currentUser.Id, _currentUser, _cancellationToken);
        }

        [Authorize(Policy = Policies.CONSUMER)]
        [GraphQLName("userPositionInCountry")]
        [GraphQLType(typeof(UserPositionType))]
        public async Task<UserPositionDto> GetMyPositionAsync([Service] ILeaderboardQueries leaderboardQueries)
        {
            return await leaderboardQueries.UserPositionInCountryAsync(_currentUser.Id, _currentUser, _cancellationToken);
        }

        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLName("productsImportInProgress")]
        public async Task<bool> HasProductsImportsInProgressAsync([Service] IJobQueries jobQueries)
        {
            return await jobQueries.HasProductsImportsInProgressAsync(_currentUser.Id, _currentUser, _cancellationToken);
        }

        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLName("pickingOrdersExportInProgress")]
        public async Task<bool> HasPickingOrdersExportsInProgressAsync([Service] IJobQueries jobQueries)
        {
            return await jobQueries.HasPickingOrdersExportsInProgressAsync(_currentUser.Id, _currentUser, _cancellationToken);
        }

        [Authorize(Policy = Policies.STORE)]
        [GraphQLName("getStoreDeliveriesForProducers")]
        [GraphQLType(typeof(ListType<ProducerDeliveriesType>))]
        [UseSorting(SortType = typeof(ProducerDeliveriesSortType))]
        public async Task<IEnumerable<ProducerDeliveriesDto>> GetStoreDeliveriesForProducersAsync(SearchProducersDeliveriesInput input, [Service] IDeliveryQueries deliveryQueries)
        {
            return await deliveryQueries.GetStoreDeliveriesForProducersAsync(_currentUser.Id, input.Ids, input.Kinds, DateTimeOffset.UtcNow, _currentUser, _cancellationToken);
        }

        [GraphQLName("getDeliveriesForProducers")]
        [GraphQLType(typeof(ListType<ProducerDeliveriesType>))]
        [UseSorting(SortType = typeof(ProducerDeliveriesSortType))]
        public async Task<IEnumerable<ProducerDeliveriesDto>> GetProducersDeliveriesAsync(SearchProducersDeliveriesInput input, [Service] IDeliveryQueries deliveryQueries)
        {
            return await deliveryQueries.GetProducersDeliveriesAsync(input.Ids, input.Kinds, DateTimeOffset.UtcNow, _currentUser, _cancellationToken);
        }

        [Authorize(Policy = Policies.UNREGISTERED)]
        [GraphQLName("searchBusinessWithSiret")]
        [GraphQLType(typeof(SirenBusinessType))]
        public async Task<SirenBusinessDto> SearchBusinessWithSiretAsync(string input, [Service] IBusinessQueries businessQueries)
        {
            return await businessQueries.RetrieveSiretInfosAsync(input, _currentUser, _cancellationToken);
        }

        [Authorize(Policy = Policies.STORE)]
        [GraphQLName("searchProducers")]
        [GraphQLType(typeof(ProducersSearchType))]
        public async Task<ProducersSearchDto> SearchProducersAsync(SearchTermsInput input, [Service] IBusinessQueries businessQueries)
        {
            return await businessQueries.SearchProducersAsync(_currentUser.Id, input, _currentUser, _cancellationToken);
        }

        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLName("searchStores")]
        [GraphQLType(typeof(StoresSearchType))]
        public async Task<StoresSearchDto> SearchStoresAsync(SearchTermsInput input, [Service] IBusinessQueries businessQueries)
        {
            return await businessQueries.SearchStoresAsync(_currentUser.Id, input, _currentUser, _cancellationToken);
        }

        [GraphQLName("searchProducts")]
        [GraphQLType(typeof(ProductsSearchType))]
        public async Task<ProductsSearchDto> SearchProductsAsync(SearchTermsInput input, [Service] IProductQueries productQueries)
        {
            return await productQueries.SearchAsync(input, _currentUser, _cancellationToken);
        }

        [Authorize(Policy = Policies.AUTHENTICATED)]
        [GraphQLName("me")]
        [UseSingleOrDefault]
        [UseSelection]
        public IQueryable<UserProfileDto> Me([Service] IUserQueries userQueries)
        {
            return userQueries.GetUserProfile(_currentUser);
        }

        [Authorize(Policy = Policies.STORE)]
        [GraphQLName("storeAgreementsProducts")]
        [UsePaging(SchemaType = typeof(ProductType))]
        [UseSorting(SortType = typeof(ProductSortType))]
        [UseFiltering(FilterType = typeof(ProductFilterType))]
        [UseSelection]
        public IQueryable<ProductDto> GetStoreProducts([Service] IProductQueries productQueries)
        {
            return productQueries.GetStoreProducts(_currentUser.Id, _currentUser);
        }

        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLName("producer")]
        [UseSingleOrDefault]
        [UseSelection]
        public IQueryable<ProducerDto> GetProducer(Guid input, [Service] IBusinessQueries businessQueries)
        {
            return businessQueries.GetProducer(input, _currentUser);
        }

        [Authorize(Policy = Policies.CONSUMER)]
        [GraphQLName("consumer")]
        [UseSingleOrDefault]
        [UseSelection]
        public IQueryable<ConsumerDto> GetConsumer(Guid input, [Service] IConsumerQueries consumerQueries)
        {
            return consumerQueries.GetConsumer(input, _currentUser);
        }

        [Authorize(Policy = Policies.STORE)]
        [GraphQLName("store")]
        [UseSingleOrDefault]
        [UseSelection]
        public IQueryable<StoreDto> GetStore(Guid input, [Service] IBusinessQueries businessQueries)
        {
            return businessQueries.GetStore(input, _currentUser);
        }

        [Authorize(Policy = Policies.CONSUMER)]
        [GraphQLName("getMyConsumerLegals")]
        [UseSingleOrDefault]
        [UseSelection]
        public IQueryable<ConsumerLegalDto> GetConsumerLegals([Service] ILegalQueries legalQueries)
        {
            return legalQueries.GetMyConsumerLegals(_currentUser);
        }

        [Authorize(Policy = Policies.STORE_OR_PRODUCER)]
        [GraphQLName("getMyBusinessLegals")]
        [UseSingleOrDefault]
        [UseSelection]
        public IQueryable<BusinessLegalDto> GetBusinessLegals([Service] ILegalQueries legalQueries)
        {
            return legalQueries.GetMyBusinessLegals(_currentUser);
        }

        [Authorize(Policy = Policies.REGISTERED)]
        [GraphQLName("webPayinTransaction")]
        [UseSingleOrDefault]
        [UseSelection]
        public IQueryable<WebPayinTransactionDto> GetPayinTransaction(string input, [Service] ITransactionQueries transactionQueries)
        {
            return transactionQueries.GetWebPayinTransaction(input, _currentUser);
        }

        [GraphQLName("pointsPerCountry")]
        [UseSelection]
        public IQueryable<CountryPointsDto> GetCountryPoints(Guid? input, [Service] ILeaderboardQueries leaderboardQueries)
        {
            return leaderboardQueries.CountriesPoints(input, _currentUser);
        }

        [GraphQLName("pointsPerRegion")]
        [UseSelection]
        public IQueryable<RegionPointsDto> GetRegionsPoints(Guid? input, [Service] ILeaderboardQueries leaderboardQueries)
        {
            return leaderboardQueries.RegionsPoints(input, _currentUser);
        }

        [GraphQLName("pointsPerDepartment")]
        [UseSelection]
        public IQueryable<DepartmentPointsDto> GetDepartmentsPoints(Guid? input, [Service] ILeaderboardQueries leaderboardQueries)
        {
            return leaderboardQueries.DepartmentsPoints(input, _currentUser);
        }

        [GraphQLName("userPointsPerCountry")]
        [UseSelection]
        public IQueryable<CountryUserPointsDto> GetCountryUsersPoints(Guid? input, [Service] ILeaderboardQueries leaderboardQueries)
        {
            return leaderboardQueries.CountryUsersPoints(input, _currentUser);
        }

        [GraphQLName("userPointsPerRegion")]
        [UseSelection]
        public IQueryable<RegionUserPointsDto> GetRegionUsersPoints(Guid? input, [Service] ILeaderboardQueries leaderboardQueries)
        {
            return leaderboardQueries.RegionUsersPoints(input, _currentUser);
        }

        [GraphQLName("userPointsPerDepartment")]
        [UseSelection]
        public IQueryable<DepartmentUserPointsDto> GetDepartmentUsersPoints(Guid? input, [Service] ILeaderboardQueries leaderboardQueries)
        {
            return leaderboardQueries.DepartmentUsersPoints(input, _currentUser);
        }

        [Authorize(Policy = Policies.STORE)]
        [GraphQLName("defaultQuickOrder")]
        [UseSingleOrDefault]
        [UseSelection]
        public IQueryable<QuickOrderDto> GetMyDefaultQuickOrder([Service] IQuickOrderQueries quickOrderQueries)
        {
            return quickOrderQueries.GetUserDefaultQuickOrder(_currentUser.Id, _currentUser);
        }

        [Authorize(Policy = Policies.STORE)]
        [GraphQLName("quickOrder")]
        [UseSingleOrDefault]
        [UseSelection]
        public IQueryable<QuickOrderDto> GetQuickOrder(Guid input, [Service] IQuickOrderQueries quickOrderQueries)
        {
            return quickOrderQueries.GetQuickOrder(input, _currentUser);
        }

        [Authorize(Policy = Policies.STORE)]
        [GraphQLName("quickOrders")]
        [UsePaging]
        [UseSorting(SortType = typeof(QuickOrderSortType))]
        [UseFiltering(FilterType = typeof(QuickOrderFilterType))]
        [UseSelection]
        public IQueryable<QuickOrderDto> GetQuickOrders([Service] IQuickOrderQueries quickOrderQueries)
        {
            return quickOrderQueries.GetQuickOrders(_currentUser);
        }

        [Authorize(Policy = Policies.REGISTERED)]
        [GraphQLName("documents")]
        [UsePaging]
        [UseSelection]
        public IQueryable<DocumentDto> GetDocuments([Service] IDocumentQueries documentQueries)
        {
            return documentQueries.GetDocuments(_currentUser);
        }

        [GraphQLName("departments")]
        [UseSelection]
        public IQueryable<DepartmentDto> GetDepartments([Service] IDepartmentQueries departmentQueries)
        {
            return departmentQueries.GetDepartments(_currentUser);
        }

        [GraphQLName("regions")]
        [UseSelection]
        public IQueryable<RegionDto> GetRegions([Service] IRegionQueries regionQueries)
        {
            return regionQueries.GetRegions(_currentUser);
        }

        [GraphQLName("nationalities")]
        [UseSelection]
        public IQueryable<NationalityDto> GetNationalities([Service] INationalityQueries nationalityQueries)
        {
            return nationalityQueries.GetNationalities(_currentUser);
        }

        [GraphQLName("countries")]
        [UseSelection]
        public IQueryable<CountryDto> GetCountries([Service] ICountryQueries countryQueries)
        {
            return countryQueries.GetCountries(_currentUser);
        }

        [Authorize(Policy = Policies.REGISTERED)]
        [GraphQLName("notifications")]
        [UsePaging]
        [UseSorting(SortType = typeof(NotificationSortType))]
        [UseFiltering(FilterType = typeof(NotificationFilterType))]
        [UseSelection]
        public IQueryable<NotificationDto> GetNotifications([Service] INotificationQueries notificationQueries)
        {
            return notificationQueries.GetNotifications(_currentUser);
        }

        [GraphQLName("tags")]
        [UsePaging(SchemaType=typeof(TagType))]
        [UseSorting(SortType = typeof(TagSortType))]
        [UseFiltering(FilterType = typeof(TagFilterType))]
        [UseSelection]
        public IQueryable<TagDto> GetTags([Service] ITagQueries tagQueries)
        {
            return tagQueries.GetTags(_currentUser);
        }

        [Authorize(Policy = Policies.STORE_OR_PRODUCER)]
        [GraphQLName("agreement")]
        [UseSingleOrDefault]
        [UseSelection]
        public IQueryable<AgreementDto> GetAgreement(Guid input, [Service] IAgreementQueries agreementQueries)
        {
            return agreementQueries.GetAgreement(input, _currentUser);
        }

        [Authorize(Policy = Policies.STORE_OR_PRODUCER)]
        [GraphQLName("agreements")]
        [UsePaging]
        [UseSorting(SortType =typeof(AgreementSortType))]
        [UseFiltering(FilterType =typeof(AgreementFilterType))]
        [UseSelection]
        public IQueryable<AgreementDto> GetAgreements([Service] IAgreementQueries agreementQueries)
        {
            return agreementQueries.GetAgreements(_currentUser);
        }

        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLName("storeAgreements")]
        [UsePaging]
        [UseSorting(SortType = typeof(AgreementSortType))]
        [UseFiltering(FilterType = typeof(AgreementFilterType))]
        [UseSelection]
        public IQueryable<AgreementDto> GetStoreAgreements(Guid input, [Service] IAgreementQueries agreementQueries)
        {
            return agreementQueries.GetStoreAgreements(input, _currentUser);
        }

        [Authorize(Policy = Policies.STORE)]
        [GraphQLName("producerAgreements")]
        [UsePaging]
        [UseSorting(SortType = typeof(AgreementSortType))]
        [UseFiltering(FilterType = typeof(AgreementFilterType))]
        [UseSelection]
        public IQueryable<AgreementDto> GetProducerAgreements(Guid input, [Service] IAgreementQueries agreementQueries)
        {
            return agreementQueries.GetProducerAgreements(input, _currentUser);
        }

        [GraphQLName("product")]
        [GraphQLType(typeof(ProductDetailsType))]
        [UseSingleOrDefault]
        [UseSelection]
        public IQueryable<ProductDto> GetProduct(Guid input, [Service] IProductQueries productQueries)
        {
            return productQueries.GetProduct(input, _currentUser);
        }

        [Authorize(Policy = Policies.STORE)]
        [GraphQLName("producerProducts")]
        [UsePaging(SchemaType = typeof(ProductType))]
        [UseSorting(SortType = typeof(ProductSortType))]
        [UseFiltering(FilterType = typeof(ProductFilterType))]
        [UseSelection]
        public IQueryable<ProductDto> GetProducerProducts(Guid input, [Service] IProductQueries productQueries)
        {
            return productQueries.GetProducerProducts(input, _currentUser);
        }

        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLName("products")]
        [UsePaging(SchemaType = typeof(ProductType))]
        [UseSorting(SortType = typeof(ProductSortType))]
        [UseFiltering(FilterType = typeof(ProductFilterType))]
        [UseSelection]
        public IQueryable<ProductDto> GetProducts([Service] IProductQueries productQueries)
        {
            return productQueries.GetProducts(_currentUser);
        }

        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLName("returnable")]
        [UseSingleOrDefault]
        [UseSelection]
        public IQueryable<ReturnableDto> GetReturnable(Guid input, [Service] IReturnableQueries returnableQueries)
        {
            return returnableQueries.GetReturnable(input, _currentUser);
        }

        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLName("returnables")]
        [UsePaging]
        [UseSorting(SortType = typeof(ReturnableSortType))]
        [UseFiltering(FilterType = typeof(ReturnableFilterType))]
        [UseSelection]
        public IQueryable<ReturnableDto> GetReturnables([Service] IReturnableQueries returnableQueries)
        {
            return returnableQueries.GetReturnables(_currentUser);
        }

        [Authorize(Policy = Policies.REGISTERED)]
        [GraphQLName("job")]
        [UseSingleOrDefault]
        [UseSelection]
        public IQueryable<JobDto> GetJob(Guid input, [Service] IJobQueries jobQueries)
        {
            return jobQueries.GetJob(input, _currentUser);
        }

        [Authorize(Policy = Policies.REGISTERED)]
        [GraphQLName("jobs")]
        [UsePaging]
        [UseSorting(SortType = typeof(JobSortType))]
        [UseFiltering(FilterType = typeof(JobFilterType))]
        [UseSelection]
        public IQueryable<JobDto> GetJobs([Service] IJobQueries jobQueries)
        {
            return jobQueries.GetJobs(_currentUser);
        }

        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLName("delivery")]
        [UseSingleOrDefault]
        [UseSelection]
        public IQueryable<DeliveryModeDto> GetDelivery(Guid input, [Service] IDeliveryQueries deliveryQueries)
        {
            return deliveryQueries.GetDelivery(input, _currentUser);
        }

        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLName("deliveries")]
        [UsePaging]
        [UseSorting(SortType = typeof(DeliveryModeSortType))]
        [UseFiltering(FilterType = typeof(DeliveryModeFilterType))]
        [UseSelection]
        public IQueryable<DeliveryModeDto> GetDeliveries([Service] IDeliveryQueries deliveryQueries)
        {
            return deliveryQueries.GetDeliveries(_currentUser);
        }

        [Authorize(Policy = Policies.REGISTERED)]
        [GraphQLName("purchaseOrder")]
        [UseSingleOrDefault]
        [UseSelection]
        public IQueryable<PurchaseOrderDto> GetPurchaseOrder(Guid input, [Service] IPurchaseOrderQueries purchaseOrderQueries)
        {
            return purchaseOrderQueries.GetPurchaseOrder(input, _currentUser);
        }

        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLName("purchaseOrders")]
        [UsePaging]
        [UseSorting(SortType = typeof(PurchaseOrderSortType))]
        [UseFiltering(FilterType = typeof(PurchaseOrderFilterType))]
        [UseSelection]
        public IQueryable<PurchaseOrderDto> GetPurchaseOrders([Service] IPurchaseOrderQueries purchaseOrderQueries)
        {
            return purchaseOrderQueries.GetPurchaseOrders(_currentUser);
        }

        [Authorize(Policy = Policies.REGISTERED)]
        [GraphQLName("myOrders")]
        [UsePaging]
        [UseSorting(SortType = typeof(PurchaseOrderSortType))]
        [UseFiltering(FilterType = typeof(PurchaseOrderFilterType))]
        [UseSelection]
        public IQueryable<PurchaseOrderDto> GetMyOrders([Service] IPurchaseOrderQueries purchaseOrderQueries)
        {
            return purchaseOrderQueries.MyPurchaseOrders(_currentUser);
        }

        [Authorize(Policy = Policies.OWNER)]
        [GraphQLName("myBusiness")]
        [UseSingleOrDefault]
        [UseSelection]
        public IQueryable<BusinessProfileDto> GetMyBusiness([Service] IBusinessQueries businessQueries)
        {
            return businessQueries.GetMyProfile(_currentUser);
        }
    }
}