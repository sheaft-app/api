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
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

namespace Sheaft.GraphQL.Services
{
    public class SheaftQuery
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<SheaftQuery> _logger;

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
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor, 
            ILogger<SheaftQuery> logger)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<string> GetFreshdeskTokenAsync([Service] IUserQueries userQueries)
        {
            SetLogTransaction("GraphQL", nameof(GetFreshdeskTokenAsync));
            return await userQueries.GetFreshdeskTokenAsync(CurrentUser, Token);
        }

        public async Task<RankInformationDto> GetMyRankInformationAsync([Service] ILeaderboardQueries leaderboardQueries)
        {
            SetLogTransaction("GraphQL", nameof(GetMyRankInformationAsync));
            return await leaderboardQueries.UserRankInformationAsync(CurrentUser.Id, CurrentUser, Token);
        }

        public async Task<UserPositionDto> GetMyPositionInDepartment([Service] ILeaderboardQueries leaderboardQueries)
        {
            SetLogTransaction("GraphQL", nameof(GetMyPositionInDepartment));
            return await leaderboardQueries.UserPositionInDepartmentAsync(CurrentUser.Id, CurrentUser, Token);
        }

        public async Task<UserPositionDto> GetMyPositionInRegion([Service] ILeaderboardQueries leaderboardQueries)
        {
            SetLogTransaction("GraphQL", nameof(GetMyPositionInRegion));
            return await leaderboardQueries.UserPositionInRegionAsync(CurrentUser.Id, CurrentUser, Token);
        }

        public async Task<UserPositionDto> GetMyPositionAsync([Service] ILeaderboardQueries leaderboardQueries)
        {
            SetLogTransaction("GraphQL", nameof(GetMyPositionAsync));
            return await leaderboardQueries.UserPositionInCountryAsync(CurrentUser.Id, CurrentUser, Token);
        }

        public async Task<bool> HasProductsImportsInProgressAsync([Service] IJobQueries jobQueries)
        {
            SetLogTransaction("GraphQL", nameof(HasProductsImportsInProgressAsync));
            return await jobQueries.HasProductsImportsInProgressAsync(CurrentUser.Id, CurrentUser, Token);
        }

        public async Task<bool> HasPickingOrdersExportsInProgressAsync([Service] IJobQueries jobQueries)
        {
            SetLogTransaction("GraphQL", nameof(HasPickingOrdersExportsInProgressAsync));
            return await jobQueries.HasPickingOrdersExportsInProgressAsync(CurrentUser.Id, CurrentUser, Token);
        }

        public async Task<IEnumerable<ProducerDeliveriesDto>> GetStoreDeliveriesForProducersAsync(SearchProducersDeliveriesInput input, [Service] IDeliveryQueries deliveryQueries)
        {
            SetLogTransaction("GraphQL", nameof(GetStoreDeliveriesForProducersAsync), input);
            return await deliveryQueries.GetStoreDeliveriesForProducersAsync(CurrentUser.Id, input.Ids, input.Kinds, DateTimeOffset.UtcNow, CurrentUser, Token);
        }

        public async Task<IEnumerable<ProducerDeliveriesDto>> GetProducersDeliveriesAsync(SearchProducersDeliveriesInput input, [Service] IDeliveryQueries deliveryQueries)
        {
            SetLogTransaction("GraphQL", nameof(GetProducersDeliveriesAsync), input);
            return await deliveryQueries.GetProducersDeliveriesAsync(input.Ids, input.Kinds, DateTimeOffset.UtcNow, CurrentUser, Token);
        }

        public async Task<SirenBusinessDto> SearchBusinessWithSiretAsync(string input, [Service] IBusinessQueries businessQueries)
        {
            SetLogTransaction("GraphQL", nameof(SearchBusinessWithSiretAsync), input);
            return await businessQueries.RetrieveSiretInfosAsync(input, CurrentUser, Token);
        }

        public async Task<ProducersSearchDto> SearchProducersAsync(SearchTermsInput input, [Service] IBusinessQueries businessQueries)
        {
            SetLogTransaction("GraphQL", nameof(SearchProducersAsync), input);
            return await businessQueries.SearchProducersAsync(CurrentUser.Id, input, CurrentUser, Token);
        }

        public async Task<StoresSearchDto> SearchStoresAsync(SearchTermsInput input, [Service] IBusinessQueries businessQueries)
        {
            SetLogTransaction("GraphQL", nameof(SearchStoresAsync), input);
            return await businessQueries.SearchStoresAsync(CurrentUser.Id, input, CurrentUser, Token);
        }

        public async Task<ProductsSearchDto> SearchProductsAsync(SearchProductsInput input, [Service] IProductQueries productQueries)
        {
            SetLogTransaction("GraphQL", nameof(SearchProductsAsync), input);
            return await productQueries.SearchAsync(input, CurrentUser, Token);
        }

        public IQueryable<UserProfileDto> GetMyUserProfile([Service] IUserQueries userQueries)
        {
            SetLogTransaction("GraphQL", nameof(GetMyUserProfile));
            return userQueries.GetUserProfile(CurrentUser.Id, CurrentUser);
        }

        public IQueryable<ProductDto> GetStoreProducts([Service] IProductQueries productQueries)
        {
            SetLogTransaction("GraphQL", nameof(GetStoreProducts));
            return productQueries.GetStoreProducts(CurrentUser.Id, CurrentUser);
        }

        public IQueryable<ProducerDto> GetProducer(Guid input, [Service] IBusinessQueries businessQueries)
        {
            SetLogTransaction("GraphQL", nameof(GetProducer), input);
            return businessQueries.GetProducer(input, CurrentUser);
        }

        public IQueryable<ConsumerDto> GetConsumer(Guid input, [Service] IConsumerQueries consumerQueries)
        {
            SetLogTransaction("GraphQL", nameof(GetConsumer), input);
            return consumerQueries.GetConsumer(input, CurrentUser);
        }

        public IQueryable<StoreDto> GetStore(Guid input, [Service] IBusinessQueries businessQueries)
        {
            SetLogTransaction("GraphQL", nameof(GetStore), input);
            return businessQueries.GetStore(input, CurrentUser);
        }

        public IQueryable<ConsumerLegalDto> GetConsumerLegals([Service] ILegalQueries legalQueries)
        {
            SetLogTransaction("GraphQL", nameof(GetConsumerLegals));
            return legalQueries.GetMyConsumerLegals(CurrentUser);
        }

        public IQueryable<BusinessLegalDto> GetBusinessLegals([Service] ILegalQueries legalQueries)
        {
            SetLogTransaction("GraphQL", nameof(GetBusinessLegals));
            return legalQueries.GetMyBusinessLegals(CurrentUser);
        }

        public IQueryable<WebPayinDto> GetWebPayinTransaction(string input, [Service] ITransactionQueries transactionQueries)
        {
            SetLogTransaction("GraphQL", nameof(GetWebPayinTransaction), input);
            return transactionQueries.GetWebPayinTransaction(input, CurrentUser);
        }

        public IQueryable<CountryPointsDto> GetCountryPoints(Guid? input, [Service] ILeaderboardQueries leaderboardQueries)
        {
            SetLogTransaction("GraphQL", nameof(GetCountryPoints), input);
            return leaderboardQueries.CountriesPoints(input, CurrentUser);
        }

        public IQueryable<RegionPointsDto> GetRegionsPoints(Guid? input, [Service] ILeaderboardQueries leaderboardQueries)
        {
            SetLogTransaction("GraphQL", nameof(GetRegionsPoints), input);
            return leaderboardQueries.RegionsPoints(input, CurrentUser);
        }

        public IQueryable<DepartmentPointsDto> GetDepartmentsPoints(Guid? input, [Service] ILeaderboardQueries leaderboardQueries)
        {
            SetLogTransaction("GraphQL", nameof(GetDepartmentsPoints), input);
            return leaderboardQueries.DepartmentsPoints(input, CurrentUser);
        }

        public IQueryable<CountryUserPointsDto> GetCountryUsersPoints(Guid? input, [Service] ILeaderboardQueries leaderboardQueries)
        {
            SetLogTransaction("GraphQL", nameof(GetCountryUsersPoints), input);
            return leaderboardQueries.CountryUsersPoints(input, CurrentUser);
        }

        public IQueryable<RegionUserPointsDto> GetRegionUsersPoints(Guid? input, [Service] ILeaderboardQueries leaderboardQueries)
        {
            SetLogTransaction("GraphQL", nameof(GetRegionUsersPoints), input);
            return leaderboardQueries.RegionUsersPoints(input, CurrentUser);
        }

        public IQueryable<DepartmentUserPointsDto> GetDepartmentUsersPoints(Guid? input, [Service] ILeaderboardQueries leaderboardQueries)
        {
            SetLogTransaction("GraphQL", nameof(GetDepartmentUsersPoints), input);
            return leaderboardQueries.DepartmentUsersPoints(input, CurrentUser);
        }

        public IQueryable<QuickOrderDto> GetMyDefaultQuickOrder([Service] IQuickOrderQueries quickOrderQueries)
        {
            SetLogTransaction("GraphQL", nameof(GetMyDefaultQuickOrder));
            return quickOrderQueries.GetUserDefaultQuickOrder(CurrentUser.Id, CurrentUser);
        }

        public IQueryable<QuickOrderDto> GetQuickOrder(Guid input, [Service] IQuickOrderQueries quickOrderQueries)
        {
            SetLogTransaction("GraphQL", nameof(GetQuickOrder), input);
            return quickOrderQueries.GetQuickOrder(input, CurrentUser);
        }

        public IQueryable<QuickOrderDto> GetQuickOrders([Service] IQuickOrderQueries quickOrderQueries)
        {
            SetLogTransaction("GraphQL", nameof(GetQuickOrders));
            return quickOrderQueries.GetQuickOrders(CurrentUser);
        }

        public IQueryable<DocumentDto> GetDocuments([Service] IDocumentQueries documentQueries)
        {
            SetLogTransaction("GraphQL", nameof(GetDocuments));
            return documentQueries.GetDocuments(CurrentUser);
        }

        public IQueryable<DepartmentDto> GetDepartments([Service] IDepartmentQueries departmentQueries)
        {
            SetLogTransaction("GraphQL", nameof(GetDepartments));
            return departmentQueries.GetDepartments(CurrentUser);
        }

        public IQueryable<RegionDto> GetRegions([Service] IRegionQueries regionQueries)
        {
            SetLogTransaction("GraphQL", nameof(GetRegions));
            return regionQueries.GetRegions(CurrentUser);
        }

        public IQueryable<NationalityDto> GetNationalities([Service] INationalityQueries nationalityQueries)
        {
            SetLogTransaction("GraphQL", nameof(GetNationalities));
            return nationalityQueries.GetNationalities(CurrentUser);
        }

        public IQueryable<CountryDto> GetCountries([Service] ICountryQueries countryQueries)
        {
            SetLogTransaction("GraphQL", nameof(GetCountries));
            return countryQueries.GetCountries(CurrentUser);
        }

        public IQueryable<NotificationDto> GetNotifications([Service] INotificationQueries notificationQueries)
        {
            SetLogTransaction("GraphQL", nameof(GetNotifications));
            return notificationQueries.GetNotifications(CurrentUser);
        }

        public async Task<int> GetUnreadNotificationsCount([Service] INotificationQueries notificationQueries)
        {
            SetLogTransaction("GraphQL", nameof(GetUnreadNotificationsCount));
            return await notificationQueries.GetUnreadNotificationsCount(CurrentUser, Token);
        }

        public IQueryable<TagDto> GetTags([Service] ITagQueries tagQueries)
        {
            SetLogTransaction("GraphQL", nameof(GetTags));
            return tagQueries.GetTags(CurrentUser);
        }

        public IQueryable<OrderDto> GetOrder(Guid input, [Service] IOrderQueries orderQueries)
        {
            SetLogTransaction("GraphQL", nameof(GetOrder), input);
            return orderQueries.GetOrder(input, CurrentUser);
        }

        public IQueryable<OrderDto> GetOrders([Service] IOrderQueries orderQueries)
        {
            SetLogTransaction("GraphQL", nameof(GetOrders));
            return orderQueries.GetOrders(CurrentUser);
        }

        public IQueryable<AgreementDto> GetAgreement(Guid input, [Service] IAgreementQueries agreementQueries)
        {
            SetLogTransaction("GraphQL", nameof(GetAgreement), input);
            return agreementQueries.GetAgreement(input, CurrentUser);
        }

        public IQueryable<AgreementDto> GetAgreements([Service] IAgreementQueries agreementQueries)
        {
            SetLogTransaction("GraphQL", nameof(GetAgreements));
            return agreementQueries.GetAgreements(CurrentUser);
        }

        public IQueryable<AgreementDto> GetStoreAgreements(Guid input, [Service] IAgreementQueries agreementQueries)
        {
            SetLogTransaction("GraphQL", nameof(GetStoreAgreements), input);
            return agreementQueries.GetStoreAgreements(input, CurrentUser);
        }

        public IQueryable<AgreementDto> GetProducerAgreements(Guid input, [Service] IAgreementQueries agreementQueries)
        {
            SetLogTransaction("GraphQL", nameof(GetProducerAgreements), input);
            return agreementQueries.GetProducerAgreements(input, CurrentUser);
        }

        public IQueryable<ProductDto> GetProduct(Guid input, [Service] IProductQueries productQueries)
        {
            SetLogTransaction("GraphQL", nameof(GetProduct), input);
            return productQueries.GetProduct(input, CurrentUser);
        }

        public IQueryable<ProductDto> GetProducerProducts(Guid input, [Service] IProductQueries productQueries)
        {
            SetLogTransaction("GraphQL", nameof(GetProducerProducts), input);
            return productQueries.GetProducerProductsForStores(input, CurrentUser);
        }

        public IQueryable<ProductDto> GetProducts([Service] IProductQueries productQueries)
        {
            SetLogTransaction("GraphQL", nameof(GetProducts));
            return productQueries.GetProducts(CurrentUser);
        }

        public IQueryable<ReturnableDto> GetReturnable(Guid input, [Service] IReturnableQueries returnableQueries)
        {
            SetLogTransaction("GraphQL", nameof(GetReturnable), input);
            return returnableQueries.GetReturnable(input, CurrentUser);
        }

        public IQueryable<ReturnableDto> GetReturnables([Service] IReturnableQueries returnableQueries)
        {
            SetLogTransaction("GraphQL", nameof(GetReturnables));
            return returnableQueries.GetReturnables(CurrentUser);
        }

        public IQueryable<JobDto> GetJob(Guid input, [Service] IJobQueries jobQueries)
        {
            SetLogTransaction("GraphQL", nameof(GetJob), input);
            return jobQueries.GetJob(input, CurrentUser);
        }

        public IQueryable<JobDto> GetJobs([Service] IJobQueries jobQueries)
        {
            SetLogTransaction("GraphQL", nameof(GetJobs));
            return jobQueries.GetJobs(CurrentUser);
        }

        public IQueryable<DeliveryModeDto> GetDelivery(Guid input, [Service] IDeliveryQueries deliveryQueries)
        {
            SetLogTransaction("GraphQL", nameof(GetDelivery), input);
            return deliveryQueries.GetDelivery(input, CurrentUser);
        }

        public IQueryable<DeliveryModeDto> GetDeliveries([Service] IDeliveryQueries deliveryQueries)
        {
            SetLogTransaction("GraphQL", nameof(GetDeliveries));
            return deliveryQueries.GetDeliveries(CurrentUser);
        }

        public IQueryable<PurchaseOrderDto> GetPurchaseOrder(Guid input, [Service] IPurchaseOrderQueries purchaseOrderQueries)
        {
            SetLogTransaction("GraphQL", nameof(GetPurchaseOrder), input);
            return purchaseOrderQueries.GetPurchaseOrder(input, CurrentUser);
        }

        public IQueryable<PurchaseOrderDto> GetPurchaseOrders([Service] IPurchaseOrderQueries purchaseOrderQueries)
        {
            SetLogTransaction("GraphQL", nameof(GetPurchaseOrders));
            return purchaseOrderQueries.GetPurchaseOrders(CurrentUser);
        }

        public IQueryable<PurchaseOrderDto> GetMyPurchaseOrders([Service] IPurchaseOrderQueries purchaseOrderQueries)
        {
            SetLogTransaction("GraphQL", nameof(GetMyPurchaseOrders));
            return purchaseOrderQueries.MyPurchaseOrders(CurrentUser);
        }

        public IQueryable<BusinessProfileDto> GetMyBusiness([Service] IBusinessQueries businessQueries)
        {
            SetLogTransaction("GraphQL", nameof(GetMyBusiness));
            return businessQueries.GetMyProfile(CurrentUser);
        }

        private void SetLogTransaction(string category, string name, object input = null)
        {
            NewRelic.Api.Agent.NewRelic.SetTransactionName(category, name);

            var currentTransaction = NewRelic.Api.Agent.NewRelic.GetAgent().CurrentTransaction;
            currentTransaction.AddCustomAttribute("RequestId", _httpContextAccessor.HttpContext.TraceIdentifier);
            currentTransaction.AddCustomAttribute("UserIdentifier", CurrentUser.Id.ToString("N"));
            currentTransaction.AddCustomAttribute("IsAuthenticated", CurrentUser.IsAuthenticated.ToString());
            currentTransaction.AddCustomAttribute("Roles", string.Join(";", CurrentUser.Roles));
            currentTransaction.AddCustomAttribute("GraphQL", name);

            using (var scope = _logger.BeginScope(new Dictionary<string, object>
            {
                ["UserIdentifier"] = CurrentUser.Id.ToString("N"),
                ["Roles"] = string.Join(';', CurrentUser.Roles),
                ["IsAuthenticated"] = CurrentUser.IsAuthenticated.ToString(),
                ["GraphQL"] = name,
                ["Datas"] = _configuration.GetValue<bool?>("NEW_RELIC_LOG_DATA_QUERY") ?? true ? JsonConvert.SerializeObject(input) : null
            }))
            {
                _logger.LogInformation($"Querying {name}");
            }
        }
    }
}