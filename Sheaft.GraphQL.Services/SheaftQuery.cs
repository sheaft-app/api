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
using HotChocolate;

namespace Sheaft.GraphQL.Services
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

        public SheaftQuery(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> GetFreshdeskTokenAsync([Service] IUserQueries userQueries)
        {
            return await userQueries.GetFreshdeskTokenAsync(CurrentUser, Token);
        }

        public async Task<RankInformationDto> GetMyRankInformationAsync([Service] ILeaderboardQueries leaderboardQueries)
        {
            return await leaderboardQueries.UserRankInformationAsync(CurrentUser.Id, CurrentUser, Token);
        }

        public async Task<UserPositionDto> GetMyPositionInDepartment([Service] ILeaderboardQueries leaderboardQueries)
        {
            return await leaderboardQueries.UserPositionInDepartmentAsync(CurrentUser.Id, CurrentUser, Token);
        }

        public async Task<UserPositionDto> GetMyPositionInRegion([Service] ILeaderboardQueries leaderboardQueries)
        {
            return await leaderboardQueries.UserPositionInRegionAsync(CurrentUser.Id, CurrentUser, Token);
        }

        public async Task<UserPositionDto> GetMyPositionAsync([Service] ILeaderboardQueries leaderboardQueries)
        {
            return await leaderboardQueries.UserPositionInCountryAsync(CurrentUser.Id, CurrentUser, Token);
        }

        public async Task<bool> HasProductsImportsInProgressAsync([Service] IJobQueries jobQueries)
        {
            return await jobQueries.HasProductsImportsInProgressAsync(CurrentUser.Id, CurrentUser, Token);
        }

        public async Task<bool> HasPickingOrdersExportsInProgressAsync([Service] IJobQueries jobQueries)
        {
            return await jobQueries.HasPickingOrdersExportsInProgressAsync(CurrentUser.Id, CurrentUser, Token);
        }

        public async Task<IEnumerable<ProducerDeliveriesDto>> GetStoreDeliveriesForProducersAsync(SearchProducersDeliveriesInput input, [Service] IDeliveryQueries deliveryQueries)
        {
            return await deliveryQueries.GetStoreDeliveriesForProducersAsync(CurrentUser.Id, input.Ids, input.Kinds, DateTimeOffset.UtcNow, CurrentUser, Token);
        }

        public async Task<IEnumerable<ProducerDeliveriesDto>> GetProducersDeliveriesAsync(SearchProducersDeliveriesInput input, [Service] IDeliveryQueries deliveryQueries)
        {
            return await deliveryQueries.GetProducersDeliveriesAsync(input.Ids, input.Kinds, DateTimeOffset.UtcNow, CurrentUser, Token);
        }

        public async Task<SirenBusinessDto> SearchBusinessWithSiretAsync(string input, [Service] IBusinessQueries businessQueries)
        {
            return await businessQueries.RetrieveSiretInfosAsync(input, CurrentUser, Token);
        }

        public async Task<ProducersSearchDto> SearchProducersAsync(SearchTermsInput input, [Service] IBusinessQueries businessQueries)
        {
            return await businessQueries.SearchProducersAsync(CurrentUser.Id, input, CurrentUser, Token);
        }

        public async Task<StoresSearchDto> SearchStoresAsync(SearchTermsInput input, [Service] IBusinessQueries businessQueries)
        {
            return await businessQueries.SearchStoresAsync(CurrentUser.Id, input, CurrentUser, Token);
        }

        public async Task<ProductsSearchDto> SearchProductsAsync(SearchTermsInput input, [Service] IProductQueries productQueries)
        {
            return await productQueries.SearchAsync(input, CurrentUser, Token);
        }

        public IQueryable<UserProfileDto> GetMyUserProfile([Service] IUserQueries userQueries)
        {
            return userQueries.GetUserProfile(CurrentUser.Id, CurrentUser);
        }

        public IQueryable<ProductDto> GetStoreProducts([Service] IProductQueries productQueries)
        {
            return productQueries.GetStoreProducts(CurrentUser.Id, CurrentUser);
        }

        public IQueryable<ProducerDto> GetProducer(Guid input, [Service] IBusinessQueries businessQueries)
        {
            return businessQueries.GetProducer(input, CurrentUser);
        }

        public IQueryable<ConsumerDto> GetConsumer(Guid input, [Service] IConsumerQueries consumerQueries)
        {
            return consumerQueries.GetConsumer(input, CurrentUser);
        }

        public IQueryable<StoreDto> GetStore(Guid input, [Service] IBusinessQueries businessQueries)
        {
            return businessQueries.GetStore(input, CurrentUser);
        }

        public IQueryable<ConsumerLegalDto> GetConsumerLegals([Service] ILegalQueries legalQueries)
        {
            return legalQueries.GetMyConsumerLegals(CurrentUser);
        }

        public IQueryable<BusinessLegalDto> GetBusinessLegals([Service] ILegalQueries legalQueries)
        {
            return legalQueries.GetMyBusinessLegals(CurrentUser);
        }

        public IQueryable<WebPayinTransactionDto> GetWebPayinTransaction(string input, [Service] ITransactionQueries transactionQueries)
        {
            return transactionQueries.GetWebPayinTransaction(input, CurrentUser);
        }

        public IQueryable<CountryPointsDto> GetCountryPoints(Guid? input, [Service] ILeaderboardQueries leaderboardQueries)
        {
            return leaderboardQueries.CountriesPoints(input, CurrentUser);
        }

        public IQueryable<RegionPointsDto> GetRegionsPoints(Guid? input, [Service] ILeaderboardQueries leaderboardQueries)
        {
            return leaderboardQueries.RegionsPoints(input, CurrentUser);
        }

        public IQueryable<DepartmentPointsDto> GetDepartmentsPoints(Guid? input, [Service] ILeaderboardQueries leaderboardQueries)
        {
            return leaderboardQueries.DepartmentsPoints(input, CurrentUser);
        }

        public IQueryable<CountryUserPointsDto> GetCountryUsersPoints(Guid? input, [Service] ILeaderboardQueries leaderboardQueries)
        {
            return leaderboardQueries.CountryUsersPoints(input, CurrentUser);
        }

        public IQueryable<RegionUserPointsDto> GetRegionUsersPoints(Guid? input, [Service] ILeaderboardQueries leaderboardQueries)
        {
            return leaderboardQueries.RegionUsersPoints(input, CurrentUser);
        }

        public IQueryable<DepartmentUserPointsDto> GetDepartmentUsersPoints(Guid? input, [Service] ILeaderboardQueries leaderboardQueries)
        {
            return leaderboardQueries.DepartmentUsersPoints(input, CurrentUser);
        }

        public IQueryable<QuickOrderDto> GetMyDefaultQuickOrder([Service] IQuickOrderQueries quickOrderQueries)
        {
            return quickOrderQueries.GetUserDefaultQuickOrder(CurrentUser.Id, CurrentUser);
        }

        public IQueryable<QuickOrderDto> GetQuickOrder(Guid input, [Service] IQuickOrderQueries quickOrderQueries)
        {
            return quickOrderQueries.GetQuickOrder(input, CurrentUser);
        }

        public IQueryable<QuickOrderDto> GetQuickOrders([Service] IQuickOrderQueries quickOrderQueries)
        {
            return quickOrderQueries.GetQuickOrders(CurrentUser);
        }

        public IQueryable<DocumentDto> GetDocuments([Service] IDocumentQueries documentQueries)
        {
            return documentQueries.GetDocuments(CurrentUser);
        }

        public IQueryable<DepartmentDto> GetDepartments([Service] IDepartmentQueries departmentQueries)
        {
            return departmentQueries.GetDepartments(CurrentUser);
        }

        public IQueryable<RegionDto> GetRegions([Service] IRegionQueries regionQueries)
        {
            return regionQueries.GetRegions(CurrentUser);
        }

        public IQueryable<NationalityDto> GetNationalities([Service] INationalityQueries nationalityQueries)
        {
            return nationalityQueries.GetNationalities(CurrentUser);
        }

        public IQueryable<CountryDto> GetCountries([Service] ICountryQueries countryQueries)
        {
            return countryQueries.GetCountries(CurrentUser);
        }

        public IQueryable<NotificationDto> GetNotifications([Service] INotificationQueries notificationQueries)
        {
            return notificationQueries.GetNotifications(CurrentUser);
        }

        public IQueryable<TagDto> GetTags([Service] ITagQueries tagQueries)
        {
            return tagQueries.GetTags(CurrentUser);
        }

        public IQueryable<OrderDto> GetOrder(Guid input, [Service] IOrderQueries orderQueries)
        {
            return orderQueries.GetOrder(input, CurrentUser);
        }

        public IQueryable<OrderDto> GetOrders([Service] IOrderQueries orderQueries)
        {
            return orderQueries.GetOrders(CurrentUser);
        }

        public IQueryable<AgreementDto> GetAgreement(Guid input, [Service] IAgreementQueries agreementQueries)
        {
            return agreementQueries.GetAgreement(input, CurrentUser);
        }

        public IQueryable<AgreementDto> GetAgreements([Service] IAgreementQueries agreementQueries)
        {
            return agreementQueries.GetAgreements(CurrentUser);
        }

        public IQueryable<AgreementDto> GetStoreAgreements(Guid input, [Service] IAgreementQueries agreementQueries)
        {
            return agreementQueries.GetStoreAgreements(input, CurrentUser);
        }

        public IQueryable<AgreementDto> GetProducerAgreements(Guid input, [Service] IAgreementQueries agreementQueries)
        {
            return agreementQueries.GetProducerAgreements(input, CurrentUser);
        }

        public IQueryable<ProductDto> GetProduct(Guid input, [Service] IProductQueries productQueries)
        {
            return productQueries.GetProduct(input, CurrentUser);
        }

        public IQueryable<ProductDto> GetProducerProducts(Guid input, [Service] IProductQueries productQueries)
        {
            return productQueries.GetProducerProducts(input, CurrentUser);
        }

        public IQueryable<ProductDto> GetProducts([Service] IProductQueries productQueries)
        {
            return productQueries.GetProducts(CurrentUser);
        }

        public IQueryable<ReturnableDto> GetReturnable(Guid input, [Service] IReturnableQueries returnableQueries)
        {
            return returnableQueries.GetReturnable(input, CurrentUser);
        }

        public IQueryable<ReturnableDto> GetReturnables([Service] IReturnableQueries returnableQueries)
        {
            return returnableQueries.GetReturnables(CurrentUser);
        }

        public IQueryable<JobDto> GetJob(Guid input, [Service] IJobQueries jobQueries)
        {
            return jobQueries.GetJob(input, CurrentUser);
        }

        public IQueryable<JobDto> GetJobs([Service] IJobQueries jobQueries)
        {
            return jobQueries.GetJobs(CurrentUser);
        }

        public IQueryable<DeliveryModeDto> GetDelivery(Guid input, [Service] IDeliveryQueries deliveryQueries)
        {
            return deliveryQueries.GetDelivery(input, CurrentUser);
        }

        public IQueryable<DeliveryModeDto> GetDeliveries([Service] IDeliveryQueries deliveryQueries)
        {
            return deliveryQueries.GetDeliveries(CurrentUser);
        }

        public IQueryable<PurchaseOrderDto> GetPurchaseOrder(Guid input, [Service] IPurchaseOrderQueries purchaseOrderQueries)
        {
            return purchaseOrderQueries.GetPurchaseOrder(input, CurrentUser);
        }

        public IQueryable<PurchaseOrderDto> GetPurchaseOrders([Service] IPurchaseOrderQueries purchaseOrderQueries)
        {
            return purchaseOrderQueries.GetPurchaseOrders(CurrentUser);
        }

        public IQueryable<PurchaseOrderDto> GetMyPurchaseOrders([Service] IPurchaseOrderQueries purchaseOrderQueries)
        {
            return purchaseOrderQueries.MyPurchaseOrders(CurrentUser);
        }

        public IQueryable<BusinessProfileDto> GetMyBusiness([Service] IBusinessQueries businessQueries)
        {
            return businessQueries.GetMyProfile(CurrentUser);
        }
    }
}