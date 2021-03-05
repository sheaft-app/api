using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using Microsoft.AspNetCore.Http;
using Sheaft.Application.Common.Interfaces.Queries;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Application.Common.Models.Inputs;
using Sheaft.Domain;

namespace Sheaft.GraphQL
{
    public class SheaftQuery
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private CancellationToken Token => _httpContextAccessor.HttpContext.RequestAborted;
        private RequestUser CurrentUser => _currentUserService.GetCurrentUserInfo().Data;

        public SheaftQuery(
            ICurrentUserService currentUserService,
            IHttpContextAccessor httpContextAccessor)
        {
            _currentUserService = currentUserService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> GetFreshdeskTokenAsync([Service] IUserQueries userQueries)
        {
            SetLogTransaction(nameof(GetFreshdeskTokenAsync));
            return await userQueries.GetFreshdeskTokenAsync(CurrentUser, Token);
        }

        public async Task<RankInformationDto> GetMyRankInformationAsync([Service] ILeaderboardQueries leaderboardQueries)
        {
            SetLogTransaction(nameof(GetMyRankInformationAsync));
            return await leaderboardQueries.UserRankInformationAsync(CurrentUser.Id, CurrentUser, Token);
        }

        public async Task<UserPositionDto> GetMyPositionInDepartment([Service] ILeaderboardQueries leaderboardQueries)
        {
            SetLogTransaction(nameof(GetMyPositionInDepartment));
            return await leaderboardQueries.UserPositionInDepartmentAsync(CurrentUser.Id, CurrentUser, Token);
        }

        public async Task<UserPositionDto> GetMyPositionInRegion([Service] ILeaderboardQueries leaderboardQueries)
        {
            SetLogTransaction(nameof(GetMyPositionInRegion));
            return await leaderboardQueries.UserPositionInRegionAsync(CurrentUser.Id, CurrentUser, Token);
        }

        public async Task<UserPositionDto> GetMyPositionAsync([Service] ILeaderboardQueries leaderboardQueries)
        {
            SetLogTransaction(nameof(GetMyPositionAsync));
            return await leaderboardQueries.UserPositionInCountryAsync(CurrentUser.Id, CurrentUser, Token);
        }

        public async Task<bool> HasProductsImportsInProgressAsync([Service] IJobQueries jobQueries)
        {
            SetLogTransaction(nameof(HasProductsImportsInProgressAsync));
            return await jobQueries.HasProductsImportsInProgressAsync(CurrentUser.Id, CurrentUser, Token);
        }

        public async Task<bool> HasPickingOrdersExportsInProgressAsync([Service] IJobQueries jobQueries)
        {
            SetLogTransaction(nameof(HasPickingOrdersExportsInProgressAsync));
            return await jobQueries.HasPickingOrdersExportsInProgressAsync(CurrentUser.Id, CurrentUser, Token);
        }

        public async Task<IEnumerable<ProducerDeliveriesDto>> GetStoreDeliveriesForProducersAsync(SearchProducersDeliveriesInput input, [Service] IDeliveryQueries deliveryQueries)
        {
            SetLogTransaction(nameof(GetStoreDeliveriesForProducersAsync), input);
            return await deliveryQueries.GetStoreDeliveriesForProducersAsync(CurrentUser.Id, input.Ids, input.Kinds, DateTimeOffset.UtcNow, CurrentUser, Token);
        }

        public async Task<IEnumerable<ProducerDeliveriesDto>> GetProducersDeliveriesAsync(SearchProducersDeliveriesInput input, [Service] IDeliveryQueries deliveryQueries)
        {
            SetLogTransaction(nameof(GetProducersDeliveriesAsync), input);
            return await deliveryQueries.GetProducersDeliveriesAsync(input.Ids, input.Kinds, DateTimeOffset.UtcNow, CurrentUser, Token);
        }

        public async Task<SirenBusinessDto> SearchBusinessWithSiretAsync(string input, [Service] IUserQueries userQueries)
        {
            SetLogTransaction(nameof(SearchBusinessWithSiretAsync), input);
            return await userQueries.RetrieveSiretInfosAsync(input, CurrentUser, Token);
        }

        public async Task<ProducersSearchDto> SearchProducersAsync(SearchTermsInput input, [Service] IProducerQueries producerQueries)
        {
            SetLogTransaction(nameof(SearchProducersAsync), input);
            return await producerQueries.SearchProducersAsync(CurrentUser.Id, input, CurrentUser, Token);
        }

        public async Task<StoresSearchDto> SearchStoresAsync(SearchTermsInput input, [Service] IStoreQueries storeQueries)
        {
            SetLogTransaction(nameof(SearchStoresAsync), input);
            return await storeQueries.SearchStoresAsync(CurrentUser.Id, input, CurrentUser, Token);
        }

        public async Task<ProductsSearchDto> SearchProductsAsync(SearchProductsInput input, [Service] IProductQueries productQueries)
        {
            SetLogTransaction(nameof(SearchProductsAsync), input);
            return await productQueries.SearchAsync(input, CurrentUser, Token);
        }

        public async Task<IEnumerable<SuggestProducerDto>> SuggestProducersAsync(SearchTermsInput input, [Service] IProducerQueries producerQueries)
        {
            SetLogTransaction(nameof(SuggestProducersAsync), input);
            return await producerQueries.SuggestProducersAsync(input, CurrentUser, Token);
        }

        public IQueryable<UserDto> GetMyUserProfile([Service] IUserQueries userQueries)
        {
            SetLogTransaction(nameof(GetMyUserProfile));
            return userQueries.GetUser(CurrentUser.Id, CurrentUser);
        }

        public IQueryable<ProductDto> GetStoreProducts([Service] IProductQueries productQueries)
        {
            SetLogTransaction(nameof(GetStoreProducts));
            return productQueries.GetProducts(CurrentUser);
        }

        public IQueryable<ProducerDto> GetProducer(Guid input, [Service] IProducerQueries producerQueries)
        {
            SetLogTransaction(nameof(GetProducer), input);
            return producerQueries.GetProducer(input, CurrentUser);
        }

        public IQueryable<ProducerDto> GetProducers([Service] IProducerQueries producerQueries)
        {
            SetLogTransaction(nameof(GetProducers));
            return producerQueries.GetProducers(CurrentUser);
        }


        public IQueryable<ConsumerDto> GetConsumer(Guid input, [Service] IConsumerQueries consumerQueries)
        {
            SetLogTransaction(nameof(GetConsumer), input);
            return consumerQueries.GetConsumer(input, CurrentUser);
        }

        public IQueryable<StoreDto> GetStore(Guid input, [Service] IStoreQueries storeQueries)
        {
            SetLogTransaction(nameof(GetStore), input);
            return storeQueries.GetStore(input, CurrentUser);
        }

        public IQueryable<ConsumerLegalDto> GetConsumerLegals([Service] ILegalQueries legalQueries)
        {
            SetLogTransaction(nameof(GetConsumerLegals));
            return legalQueries.GetMyConsumerLegals(CurrentUser);
        }

        public IQueryable<BusinessLegalDto> GetBusinessLegals([Service] ILegalQueries legalQueries)
        {
            SetLogTransaction(nameof(GetBusinessLegals));
            return legalQueries.GetMyBusinessLegals(CurrentUser);
        }

        public IQueryable<WebPayinDto> GetWebPayinTransaction(string input, [Service] IPayinQueries payinQueries)
        {
            SetLogTransaction(nameof(GetWebPayinTransaction), input);
            return payinQueries.GetWebPayinTransaction(input, CurrentUser);
        }

        public IQueryable<CountryPointsDto> GetCountryPoints(Guid? input, [Service] ILeaderboardQueries leaderboardQueries)
        {
            SetLogTransaction(nameof(GetCountryPoints), input);
            return leaderboardQueries.CountriesPoints(input, CurrentUser);
        }

        public IQueryable<RegionPointsDto> GetRegionsPoints(Guid? input, [Service] ILeaderboardQueries leaderboardQueries)
        {
            SetLogTransaction(nameof(GetRegionsPoints), input);
            return leaderboardQueries.RegionsPoints(input, CurrentUser);
        }

        public IQueryable<DepartmentPointsDto> GetDepartmentsPoints(Guid? input, [Service] ILeaderboardQueries leaderboardQueries)
        {
            SetLogTransaction(nameof(GetDepartmentsPoints), input);
            return leaderboardQueries.DepartmentsPoints(input, CurrentUser);
        }

        public IQueryable<CountryUserPointsDto> GetCountryUsersPoints(Guid? input, [Service] ILeaderboardQueries leaderboardQueries)
        {
            SetLogTransaction(nameof(GetCountryUsersPoints), input);
            return leaderboardQueries.CountryUsersPoints(input, CurrentUser);
        }

        public IQueryable<RegionUserPointsDto> GetRegionUsersPoints(Guid? input, [Service] ILeaderboardQueries leaderboardQueries)
        {
            SetLogTransaction(nameof(GetRegionUsersPoints), input);
            return leaderboardQueries.RegionUsersPoints(input, CurrentUser);
        }

        public IQueryable<DepartmentUserPointsDto> GetDepartmentUsersPoints(Guid? input, [Service] ILeaderboardQueries leaderboardQueries)
        {
            SetLogTransaction(nameof(GetDepartmentUsersPoints), input);
            return leaderboardQueries.DepartmentUsersPoints(input, CurrentUser);
        }

        public IQueryable<QuickOrderDto> GetMyDefaultQuickOrder([Service] IQuickOrderQueries quickOrderQueries)
        {
            SetLogTransaction(nameof(GetMyDefaultQuickOrder));
            return quickOrderQueries.GetUserDefaultQuickOrder(CurrentUser.Id, CurrentUser);
        }

        public IQueryable<QuickOrderDto> GetQuickOrder(Guid input, [Service] IQuickOrderQueries quickOrderQueries)
        {
            SetLogTransaction(nameof(GetQuickOrder), input);
            return quickOrderQueries.GetQuickOrder(input, CurrentUser);
        }

        public IQueryable<QuickOrderDto> GetQuickOrders([Service] IQuickOrderQueries quickOrderQueries)
        {
            SetLogTransaction(nameof(GetQuickOrders));
            return quickOrderQueries.GetQuickOrders(CurrentUser);
        }

        public IQueryable<DocumentDto> GetDocuments([Service] IDocumentQueries documentQueries)
        {
            SetLogTransaction(nameof(GetDocuments));
            return documentQueries.GetDocuments(CurrentUser);
        }

        public IQueryable<DepartmentDto> GetDepartments([Service] IDepartmentQueries departmentQueries)
        {
            SetLogTransaction(nameof(GetDepartments));
            return departmentQueries.GetDepartments(CurrentUser);
        }

        public IQueryable<RegionDto> GetRegions([Service] IRegionQueries regionQueries)
        {
            SetLogTransaction(nameof(GetRegions));
            return regionQueries.GetRegions(CurrentUser);
        }

        public IQueryable<NationalityDto> GetNationalities([Service] INationalityQueries nationalityQueries)
        {
            SetLogTransaction(nameof(GetNationalities));
            return nationalityQueries.GetNationalities(CurrentUser);
        }

        public IQueryable<CountryDto> GetCountries([Service] ICountryQueries countryQueries)
        {
            SetLogTransaction(nameof(GetCountries));
            return countryQueries.GetCountries(CurrentUser);
        }

        public IQueryable<NotificationDto> GetNotifications([Service] INotificationQueries notificationQueries)
        {
            SetLogTransaction(nameof(GetNotifications));
            return notificationQueries.GetNotifications(CurrentUser);
        }

        public async Task<int> GetUnreadNotificationsCount([Service] INotificationQueries notificationQueries)
        {
            SetLogTransaction(nameof(GetUnreadNotificationsCount));
            return await notificationQueries.GetUnreadNotificationsCount(CurrentUser, Token);
        }

        public IQueryable<TagDto> GetTags([Service] ITagQueries tagQueries)
        {
            SetLogTransaction(nameof(GetTags));
            return tagQueries.GetTags(CurrentUser);
        }

        public IQueryable<OrderDto> GetCurrentOrder([Service] IOrderQueries orderQueries)
        {
            SetLogTransaction(nameof(GetOrder));
            return orderQueries.GetCurrentOrder(CurrentUser);
        }
        
        public IQueryable<TransferDto> GetTransfer(Guid input, [Service] ITransferQueries transferQueries)
        {
            SetLogTransaction(nameof(GetTransfer));
            return transferQueries.GetTransfer(input, CurrentUser);
        }

        public IQueryable<TransferDto> GetTransfers([Service] ITransferQueries transferQueries)
        {
            SetLogTransaction(nameof(GetTransfers));
            return transferQueries.GetTransfers(CurrentUser);
        }

        public IQueryable<OrderDto> GetOrder(Guid input, [Service] IOrderQueries orderQueries)
        {
            SetLogTransaction(nameof(GetOrder), input);
            return orderQueries.GetOrder(input, CurrentUser);
        }

        public IQueryable<OrderDto> GetOrders([Service] IOrderQueries orderQueries)
        {
            SetLogTransaction(nameof(GetOrders));
            return orderQueries.GetOrders(CurrentUser);
        }

        public IQueryable<AgreementDto> GetAgreement(Guid input, [Service] IAgreementQueries agreementQueries)
        {
            SetLogTransaction(nameof(GetAgreement), input);
            return agreementQueries.GetAgreement(input, CurrentUser);
        }

        public IQueryable<AgreementDto> GetAgreements([Service] IAgreementQueries agreementQueries)
        {
            SetLogTransaction(nameof(GetAgreements));
            return agreementQueries.GetAgreements(CurrentUser);
        }

        public IQueryable<AgreementDto> GetStoreAgreements(Guid input, [Service] IAgreementQueries agreementQueries)
        {
            SetLogTransaction(nameof(GetStoreAgreements), input);
            return agreementQueries.GetStoreAgreements(input, CurrentUser);
        }

        public IQueryable<AgreementDto> GetProducerAgreements(Guid input, [Service] IAgreementQueries agreementQueries)
        {
            SetLogTransaction(nameof(GetProducerAgreements), input);
            return agreementQueries.GetProducerAgreements(input, CurrentUser);
        }

        public IQueryable<ProductDto> GetProduct(Guid input, [Service] IProductQueries productQueries)
        {
            SetLogTransaction(nameof(GetProduct), input);
            return productQueries.GetProduct(input, CurrentUser);
        }

        public IQueryable<ProductDto> GetProducerProducts(Guid input, [Service] IProductQueries productQueries)
        {
            SetLogTransaction(nameof(GetProducerProducts), input);
            return productQueries.GetProducerProducts(input, CurrentUser);
        }

        public IQueryable<ProductDto> GetProducts([Service] IProductQueries productQueries)
        {
            SetLogTransaction(nameof(GetProducts));
            return productQueries.GetProducts(CurrentUser);
        }

        public IQueryable<ReturnableDto> GetReturnable(Guid input, [Service] IReturnableQueries returnableQueries)
        {
            SetLogTransaction(nameof(GetReturnable), input);
            return returnableQueries.GetReturnable(input, CurrentUser);
        }

        public IQueryable<ReturnableDto> GetReturnables([Service] IReturnableQueries returnableQueries)
        {
            SetLogTransaction(nameof(GetReturnables));
            return returnableQueries.GetReturnables(CurrentUser);
        }

        public IQueryable<JobDto> GetJob(Guid input, [Service] IJobQueries jobQueries)
        {
            SetLogTransaction(nameof(GetJob), input);
            return jobQueries.GetJob(input, CurrentUser);
        }

        public IQueryable<JobDto> GetJobs([Service] IJobQueries jobQueries)
        {
            SetLogTransaction(nameof(GetJobs));
            return jobQueries.GetJobs(CurrentUser);
        }

        public IQueryable<DeliveryModeDto> GetDelivery(Guid input, [Service] IDeliveryQueries deliveryQueries)
        {
            SetLogTransaction(nameof(GetDelivery), input);
            return deliveryQueries.GetDelivery(input, CurrentUser);
        }

        public IQueryable<DeliveryModeDto> GetDeliveries([Service] IDeliveryQueries deliveryQueries)
        {
            SetLogTransaction(nameof(GetDeliveries));
            return deliveryQueries.GetDeliveries(CurrentUser);
        }

        public IQueryable<PurchaseOrderDto> GetPurchaseOrder(Guid input, [Service] IPurchaseOrderQueries purchaseOrderQueries)
        {
            SetLogTransaction(nameof(GetPurchaseOrder), input);
            return purchaseOrderQueries.GetPurchaseOrder(input, CurrentUser);
        }

        public IQueryable<PurchaseOrderDto> GetPurchaseOrders([Service] IPurchaseOrderQueries purchaseOrderQueries)
        {
            SetLogTransaction(nameof(GetPurchaseOrders));
            return purchaseOrderQueries.GetPurchaseOrders(CurrentUser);
        }

        public IQueryable<PurchaseOrderDto> GetMyPurchaseOrders([Service] IPurchaseOrderQueries purchaseOrderQueries)
        {
            SetLogTransaction(nameof(GetMyPurchaseOrders));
            return purchaseOrderQueries.MyPurchaseOrders(CurrentUser);
        }

        private void SetLogTransaction(string name, object input = null)
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
