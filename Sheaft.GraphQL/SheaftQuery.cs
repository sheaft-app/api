using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Sheaft.Application.Queries;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading;
using Sheaft.Application.Models;
using Sheaft.Core;
using Sheaft.Core.Extensions;
using Sheaft.Core.Security;
using HotChocolate.Types.Relay;
using HotChocolate;
using HotChocolate.Types;
using HotChocolate.AspNetCore.Authorization;
using Sheaft.GraphQL.Types;
using Sheaft.GraphQL.Sorts;
using Sheaft.GraphQL.Filters;

namespace Sheaft.GraphQL
{
    public class SheaftQuery
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
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

        public SheaftQuery(
            IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        [Authorize(Policy = Policies.AUTHENTICATED)]
        [GraphQLName("generateFreshdeskToken")]
        public async Task<string> GetFreshdeskTokenAsync([Service] IUserQueries userQueries)
        {
            return await userQueries.GetFreshdeskTokenAsync(CurrentUser, Token);
        }

        [Authorize(Policy = Policies.CONSUMER)]
        [GraphQLName("myRankInformation")]
        [GraphQLType(typeof(RankInformationType))]
        public async Task<RankInformationDto> GetMyRankInformationAsync([Service] ILeaderboardQueries leaderboardQueries)
        {
            return await leaderboardQueries.UserRankInformationAsync(CurrentUser.Id, CurrentUser, Token);
        }

        [Authorize(Policy = Policies.CONSUMER)]
        [GraphQLName("userPositionInDepartment")]
        [GraphQLType(typeof(UserPositionType))]
        public async Task<UserPositionDto> GetMyPositionInDepartment([Service] ILeaderboardQueries leaderboardQueries)
        {
            return await leaderboardQueries.UserPositionInDepartmentAsync(CurrentUser.Id, CurrentUser, Token);
        }

        [Authorize(Policy = Policies.CONSUMER)]
        [GraphQLName("userPositionInRegion")]
        [GraphQLType(typeof(UserPositionType))]
        public async Task<UserPositionDto> GetMyPositionInRegion([Service] ILeaderboardQueries leaderboardQueries)
        {
            return await leaderboardQueries.UserPositionInRegionAsync(CurrentUser.Id, CurrentUser, Token);
        }

        [Authorize(Policy = Policies.CONSUMER)]
        [GraphQLName("userPositionInCountry")]
        [GraphQLType(typeof(UserPositionType))]
        public async Task<UserPositionDto> GetMyPositionAsync([Service] ILeaderboardQueries leaderboardQueries)
        {
            return await leaderboardQueries.UserPositionInCountryAsync(CurrentUser.Id, CurrentUser, Token);
        }

        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLName("productsImportInProgress")]
        public async Task<bool> HasProductsImportsInProgressAsync([Service] IJobQueries jobQueries)
        {
            return await jobQueries.HasProductsImportsInProgressAsync(CurrentUser.Id, CurrentUser, Token);
        }

        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLName("pickingOrdersExportInProgress")]
        public async Task<bool> HasPickingOrdersExportsInProgressAsync([Service] IJobQueries jobQueries)
        {
            return await jobQueries.HasPickingOrdersExportsInProgressAsync(CurrentUser.Id, CurrentUser, Token);
        }

        [Authorize(Policy = Policies.STORE)]
        [GraphQLName("getStoreDeliveriesForProducers")]
        [GraphQLType(typeof(ListType<ProducerDeliveriesType>))]
        [UseSorting(SortType = typeof(ProducerDeliveriesSortType))]
        public async Task<IEnumerable<ProducerDeliveriesDto>> GetStoreDeliveriesForProducersAsync(SearchProducersDeliveriesInput input, [Service] IDeliveryQueries deliveryQueries)
        {
            return await deliveryQueries.GetStoreDeliveriesForProducersAsync(CurrentUser.Id, input.Ids, input.Kinds, DateTimeOffset.UtcNow, CurrentUser, Token);
        }

        [GraphQLName("getDeliveriesForProducers")]
        [GraphQLType(typeof(ListType<ProducerDeliveriesType>))]
        [UseSorting(SortType = typeof(ProducerDeliveriesSortType))]
        public async Task<IEnumerable<ProducerDeliveriesDto>> GetProducersDeliveriesAsync(SearchProducersDeliveriesInput input, [Service] IDeliveryQueries deliveryQueries)
        {
            return await deliveryQueries.GetProducersDeliveriesAsync(input.Ids, input.Kinds, DateTimeOffset.UtcNow, CurrentUser, Token);
        }

        [Authorize(Policy = Policies.UNREGISTERED)]
        [GraphQLName("searchBusinessWithSiret")]
        [GraphQLType(typeof(SirenBusinessType))]
        public async Task<SirenBusinessDto> SearchBusinessWithSiretAsync(string input, [Service] IBusinessQueries businessQueries)
        {
            return await businessQueries.RetrieveSiretInfosAsync(input, CurrentUser, Token);
        }

        [Authorize(Policy = Policies.STORE)]
        [GraphQLName("searchProducers")]
        [GraphQLType(typeof(ProducersSearchType))]
        public async Task<ProducersSearchDto> SearchProducersAsync(SearchTermsInput input, [Service] IBusinessQueries businessQueries)
        {
            return await businessQueries.SearchProducersAsync(CurrentUser.Id, input, CurrentUser, Token);
        }

        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLName("searchStores")]
        [GraphQLType(typeof(StoresSearchType))]
        public async Task<StoresSearchDto> SearchStoresAsync(SearchTermsInput input, [Service] IBusinessQueries businessQueries)
        {
            return await businessQueries.SearchStoresAsync(CurrentUser.Id, input, CurrentUser, Token);
        }

        [GraphQLName("searchProducts")]
        [GraphQLType(typeof(ProductsSearchType))]
        public async Task<ProductsSearchDto> SearchProductsAsync(SearchTermsInput input, [Service] IProductQueries productQueries)
        {
            return await productQueries.SearchAsync(input, CurrentUser, Token);
        }

        [Authorize(Policy = Policies.AUTHENTICATED)]
        [GraphQLName("me")]
        [UseSingleOrDefault]
        [UseSelection]
        public IQueryable<UserProfileDto> Me([Service] IUserQueries userQueries)
        {
            return userQueries.GetUserProfile(CurrentUser);
        }

        [Authorize(Policy = Policies.STORE)]
        [GraphQLName("storeAgreementsProducts")]
        [UsePaging(SchemaType = typeof(ProductType))]
        [UseSorting(SortType = typeof(ProductSortType))]
        [UseFiltering(FilterType = typeof(ProductFilterType))]
        [UseSelection]
        public IQueryable<ProductDto> GetStoreProducts([Service] IProductQueries productQueries)
        {
            return productQueries.GetStoreProducts(CurrentUser.Id, CurrentUser);
        }

        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLName("producer")]
        [UseSingleOrDefault]
        [UseSelection]
        public IQueryable<ProducerDto> GetProducer(Guid input, [Service] IBusinessQueries businessQueries)
        {
            return businessQueries.GetProducer(input, CurrentUser);
        }

        [Authorize(Policy = Policies.CONSUMER)]
        [GraphQLName("consumer")]
        [UseSingleOrDefault]
        [UseSelection]
        public IQueryable<ConsumerDto> GetConsumer(Guid input, [Service] IConsumerQueries consumerQueries)
        {
            return consumerQueries.GetConsumer(input, CurrentUser);
        }

        [Authorize(Policy = Policies.STORE)]
        [GraphQLName("store")]
        [UseSingleOrDefault]
        [UseSelection]
        public IQueryable<StoreDto> GetStore(Guid input, [Service] IBusinessQueries businessQueries)
        {
            return businessQueries.GetStore(input, CurrentUser);
        }

        [Authorize(Policy = Policies.CONSUMER)]
        [GraphQLName("getMyConsumerLegals")]
        [UseSingleOrDefault]
        [UseSelection]
        public IQueryable<ConsumerLegalDto> GetConsumerLegals([Service] ILegalQueries legalQueries)
        {
            return legalQueries.GetMyConsumerLegals(CurrentUser);
        }

        [Authorize(Policy = Policies.STORE_OR_PRODUCER)]
        [GraphQLName("getMyBusinessLegals")]
        [UseSingleOrDefault]
        [UseSelection]
        public IQueryable<BusinessLegalDto> GetBusinessLegals([Service] ILegalQueries legalQueries)
        {
            return legalQueries.GetMyBusinessLegals(CurrentUser);
        }

        [Authorize(Policy = Policies.REGISTERED)]
        [GraphQLName("webPayinTransaction")]
        [UseSingleOrDefault]
        [UseSelection]
        public IQueryable<WebPayinTransactionDto> GetWebPayinTransaction(string input, [Service] ITransactionQueries transactionQueries)
        {
            return transactionQueries.GetWebPayinTransaction(input, CurrentUser);
        }

        [GraphQLName("pointsPerCountry")]
        [UseSelection]
        public IQueryable<CountryPointsDto> GetCountryPoints(Guid? input, [Service] ILeaderboardQueries leaderboardQueries)
        {
            return leaderboardQueries.CountriesPoints(input, CurrentUser);
        }

        [GraphQLName("pointsPerRegion")]
        [UseSelection]
        public IQueryable<RegionPointsDto> GetRegionsPoints(Guid? input, [Service] ILeaderboardQueries leaderboardQueries)
        {
            return leaderboardQueries.RegionsPoints(input, CurrentUser);
        }

        [GraphQLName("pointsPerDepartment")]
        [UseSelection]
        public IQueryable<DepartmentPointsDto> GetDepartmentsPoints(Guid? input, [Service] ILeaderboardQueries leaderboardQueries)
        {
            return leaderboardQueries.DepartmentsPoints(input, CurrentUser);
        }

        [GraphQLName("userPointsPerCountry")]
        [UseSelection]
        public IQueryable<CountryUserPointsDto> GetCountryUsersPoints(Guid? input, [Service] ILeaderboardQueries leaderboardQueries)
        {
            return leaderboardQueries.CountryUsersPoints(input, CurrentUser);
        }

        [GraphQLName("userPointsPerRegion")]
        [UseSelection]
        public IQueryable<RegionUserPointsDto> GetRegionUsersPoints(Guid? input, [Service] ILeaderboardQueries leaderboardQueries)
        {
            return leaderboardQueries.RegionUsersPoints(input, CurrentUser);
        }

        [GraphQLName("userPointsPerDepartment")]
        [UseSelection]
        public IQueryable<DepartmentUserPointsDto> GetDepartmentUsersPoints(Guid? input, [Service] ILeaderboardQueries leaderboardQueries)
        {
            return leaderboardQueries.DepartmentUsersPoints(input, CurrentUser);
        }

        [Authorize(Policy = Policies.STORE)]
        [GraphQLName("defaultQuickOrder")]
        [UseSingleOrDefault]
        [UseSelection]
        public IQueryable<QuickOrderDto> GetMyDefaultQuickOrder([Service] IQuickOrderQueries quickOrderQueries)
        {
            return quickOrderQueries.GetUserDefaultQuickOrder(CurrentUser.Id, CurrentUser);
        }

        [Authorize(Policy = Policies.STORE)]
        [GraphQLName("quickOrder")]
        [UseSingleOrDefault]
        [UseSelection]
        public IQueryable<QuickOrderDto> GetQuickOrder(Guid input, [Service] IQuickOrderQueries quickOrderQueries)
        {
            return quickOrderQueries.GetQuickOrder(input, CurrentUser);
        }

        [Authorize(Policy = Policies.STORE)]
        [GraphQLName("quickOrders")]
        [UsePaging]
        [UseSorting(SortType = typeof(QuickOrderSortType))]
        [UseFiltering(FilterType = typeof(QuickOrderFilterType))]
        [UseSelection]
        public IQueryable<QuickOrderDto> GetQuickOrders([Service] IQuickOrderQueries quickOrderQueries)
        {
            return quickOrderQueries.GetQuickOrders(CurrentUser);
        }

        [Authorize(Policy = Policies.REGISTERED)]
        [GraphQLName("documents")]
        [UsePaging]
        [UseSelection]
        public IQueryable<DocumentDto> GetDocuments([Service] IDocumentQueries documentQueries)
        {
            return documentQueries.GetDocuments(CurrentUser);
        }

        [GraphQLName("departments")]
        [UseSelection]
        public IQueryable<DepartmentDto> GetDepartments([Service] IDepartmentQueries departmentQueries)
        {
            return departmentQueries.GetDepartments(CurrentUser);
        }

        [GraphQLName("regions")]
        [UseSelection]
        public IQueryable<RegionDto> GetRegions([Service] IRegionQueries regionQueries)
        {
            return regionQueries.GetRegions(CurrentUser);
        }

        [GraphQLName("nationalities")]
        [UseSelection]
        public IQueryable<NationalityDto> GetNationalities([Service] INationalityQueries nationalityQueries)
        {
            return nationalityQueries.GetNationalities(CurrentUser);
        }

        [GraphQLName("countries")]
        [UseSelection]
        public IQueryable<CountryDto> GetCountries([Service] ICountryQueries countryQueries)
        {
            return countryQueries.GetCountries(CurrentUser);
        }

        [Authorize(Policy = Policies.REGISTERED)]
        [GraphQLName("notifications")]
        [UsePaging]
        [UseSorting(SortType = typeof(NotificationSortType))]
        [UseFiltering(FilterType = typeof(NotificationFilterType))]
        [UseSelection]
        public IQueryable<NotificationDto> GetNotifications([Service] INotificationQueries notificationQueries)
        {
            return notificationQueries.GetNotifications(CurrentUser);
        }

        [GraphQLName("tags")]
        [UsePaging(SchemaType=typeof(TagType))]
        [UseSorting(SortType = typeof(TagSortType))]
        [UseFiltering(FilterType = typeof(TagFilterType))]
        [UseSelection]
        public IQueryable<TagDto> GetTags([Service] ITagQueries tagQueries)
        {
            return tagQueries.GetTags(CurrentUser);
        }

        [Authorize(Policy = Policies.REGISTERED)]
        [GraphQLName("order")]
        [UseSingleOrDefault]
        [UseSelection]
        public IQueryable<OrderDto> GetOrder(Guid input, [Service] IOrderQueries orderQueries)
        {
            return orderQueries.GetOrder(input, CurrentUser);
        }

        [Authorize(Policy = Policies.REGISTERED)]
        [GraphQLName("orders")]
        [UsePaging(SchemaType = typeof(OrderType))]
        [UseSorting(SortType = typeof(OrderSortType))]
        [UseFiltering(FilterType = typeof(OrderFilterType))]
        [UseSelection]
        public IQueryable<OrderDto> GetOrders([Service] IOrderQueries orderQueries)
        {
            return orderQueries.GetOrders(CurrentUser);
        }

        [Authorize(Policy = Policies.STORE_OR_PRODUCER)]
        [GraphQLName("agreement")]
        [UseSingleOrDefault]
        [UseSelection]
        public IQueryable<AgreementDto> GetAgreement(Guid input, [Service] IAgreementQueries agreementQueries)
        {
            return agreementQueries.GetAgreement(input, CurrentUser);
        }

        [Authorize(Policy = Policies.STORE_OR_PRODUCER)]
        [GraphQLName("agreements")]
        [UsePaging]
        [UseSorting(SortType =typeof(AgreementSortType))]
        [UseFiltering(FilterType =typeof(AgreementFilterType))]
        [UseSelection]
        public IQueryable<AgreementDto> GetAgreements([Service] IAgreementQueries agreementQueries)
        {
            return agreementQueries.GetAgreements(CurrentUser);
        }

        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLName("storeAgreements")]
        [UsePaging]
        [UseSorting(SortType = typeof(AgreementSortType))]
        [UseFiltering(FilterType = typeof(AgreementFilterType))]
        [UseSelection]
        public IQueryable<AgreementDto> GetStoreAgreements(Guid input, [Service] IAgreementQueries agreementQueries)
        {
            return agreementQueries.GetStoreAgreements(input, CurrentUser);
        }

        [Authorize(Policy = Policies.STORE)]
        [GraphQLName("producerAgreements")]
        [UsePaging]
        [UseSorting(SortType = typeof(AgreementSortType))]
        [UseFiltering(FilterType = typeof(AgreementFilterType))]
        [UseSelection]
        public IQueryable<AgreementDto> GetProducerAgreements(Guid input, [Service] IAgreementQueries agreementQueries)
        {
            return agreementQueries.GetProducerAgreements(input, CurrentUser);
        }

        [GraphQLName("product")]
        [GraphQLType(typeof(ProductDetailsType))]
        [UseSingleOrDefault]
        [UseSelection]
        public IQueryable<ProductDto> GetProduct(Guid input, [Service] IProductQueries productQueries)
        {
            return productQueries.GetProduct(input, CurrentUser);
        }

        [Authorize(Policy = Policies.STORE)]
        [GraphQLName("producerProducts")]
        [UsePaging(SchemaType = typeof(ProductType))]
        [UseSorting(SortType = typeof(ProductSortType))]
        [UseFiltering(FilterType = typeof(ProductFilterType))]
        [UseSelection]
        public IQueryable<ProductDto> GetProducerProducts(Guid input, [Service] IProductQueries productQueries)
        {
            return productQueries.GetProducerProducts(input, CurrentUser);
        }

        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLName("products")]
        [UsePaging(SchemaType = typeof(ProductType))]
        [UseSorting(SortType = typeof(ProductSortType))]
        [UseFiltering(FilterType = typeof(ProductFilterType))]
        [UseSelection]
        public IQueryable<ProductDto> GetProducts([Service] IProductQueries productQueries)
        {
            return productQueries.GetProducts(CurrentUser);
        }

        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLName("returnable")]
        [UseSingleOrDefault]
        [UseSelection]
        public IQueryable<ReturnableDto> GetReturnable(Guid input, [Service] IReturnableQueries returnableQueries)
        {
            return returnableQueries.GetReturnable(input, CurrentUser);
        }

        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLName("returnables")]
        [UsePaging]
        [UseSorting(SortType = typeof(ReturnableSortType))]
        [UseFiltering(FilterType = typeof(ReturnableFilterType))]
        [UseSelection]
        public IQueryable<ReturnableDto> GetReturnables([Service] IReturnableQueries returnableQueries)
        {
            return returnableQueries.GetReturnables(CurrentUser);
        }

        [Authorize(Policy = Policies.REGISTERED)]
        [GraphQLName("job")]
        [UseSingleOrDefault]
        [UseSelection]
        public IQueryable<JobDto> GetJob(Guid input, [Service] IJobQueries jobQueries)
        {
            return jobQueries.GetJob(input, CurrentUser);
        }

        [Authorize(Policy = Policies.REGISTERED)]
        [GraphQLName("jobs")]
        [UsePaging]
        [UseSorting(SortType = typeof(JobSortType))]
        [UseFiltering(FilterType = typeof(JobFilterType))]
        [UseSelection]
        public IQueryable<JobDto> GetJobs([Service] IJobQueries jobQueries)
        {
            return jobQueries.GetJobs(CurrentUser);
        }

        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLName("delivery")]
        [UseSingleOrDefault]
        [UseSelection]
        public IQueryable<DeliveryModeDto> GetDelivery(Guid input, [Service] IDeliveryQueries deliveryQueries)
        {
            return deliveryQueries.GetDelivery(input, CurrentUser);
        }

        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLName("deliveries")]
        [UsePaging]
        [UseSorting(SortType = typeof(DeliveryModeSortType))]
        [UseFiltering(FilterType = typeof(DeliveryModeFilterType))]
        [UseSelection]
        public IQueryable<DeliveryModeDto> GetDeliveries([Service] IDeliveryQueries deliveryQueries)
        {
            return deliveryQueries.GetDeliveries(CurrentUser);
        }

        [Authorize(Policy = Policies.REGISTERED)]
        [GraphQLName("purchaseOrder")]
        [UseSingleOrDefault]
        [UseSelection]
        public IQueryable<PurchaseOrderDto> GetPurchaseOrder(Guid input, [Service] IPurchaseOrderQueries purchaseOrderQueries)
        {
            return purchaseOrderQueries.GetPurchaseOrder(input, CurrentUser);
        }

        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLName("purchaseOrders")]
        [UsePaging]
        [UseSorting(SortType = typeof(PurchaseOrderSortType))]
        [UseFiltering(FilterType = typeof(PurchaseOrderFilterType))]
        [UseSelection]
        public IQueryable<PurchaseOrderDto> GetPurchaseOrders([Service] IPurchaseOrderQueries purchaseOrderQueries)
        {
            return purchaseOrderQueries.GetPurchaseOrders(CurrentUser);
        }

        [Authorize(Policy = Policies.REGISTERED)]
        [GraphQLName("myOrders")]
        [UsePaging]
        [UseSorting(SortType = typeof(PurchaseOrderSortType))]
        [UseFiltering(FilterType = typeof(PurchaseOrderFilterType))]
        [UseSelection]
        public IQueryable<PurchaseOrderDto> GetMyOrders([Service] IPurchaseOrderQueries purchaseOrderQueries)
        {
            return purchaseOrderQueries.MyPurchaseOrders(CurrentUser);
        }

        [Authorize(Policy = Policies.OWNER)]
        [GraphQLName("myBusiness")]
        [UseSingleOrDefault]
        [UseSelection]
        public IQueryable<BusinessProfileDto> GetMyBusiness([Service] IBusinessQueries businessQueries)
        {
            return businessQueries.GetMyProfile(CurrentUser);
        }
    }
}