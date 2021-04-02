using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using HotChocolate;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Queries;
using Sheaft.Application.Models;
using Sheaft.Domain;
using BusinessLegalDto = Sheaft.Application.Models.BusinessLegalDto;
using ConsumerDto = Sheaft.Application.Models.ConsumerDto;
using ConsumerLegalDto = Sheaft.Application.Models.ConsumerLegalDto;
using OrderDto = Sheaft.Application.Models.OrderDto;
using QuickOrderDto = Sheaft.Application.Models.QuickOrderDto;

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
            SetLogTransaction();
            return await userQueries.GetFreshdeskTokenAsync(CurrentUser, Token);
        }

        public async Task<RankInformationDto> GetMyRankInformationAsync([Service] ILeaderboardQueries leaderboardQueries)
        {
            SetLogTransaction();
            return await leaderboardQueries.UserRankInformationAsync(CurrentUser.Id, CurrentUser, Token);
        }

        public async Task<UserPositionDto> GetMyPositionInDepartment([Service] ILeaderboardQueries leaderboardQueries)
        {
            SetLogTransaction();
            return await leaderboardQueries.UserPositionInDepartmentAsync(CurrentUser.Id, CurrentUser, Token);
        }

        public async Task<UserPositionDto> GetMyPositionInRegion([Service] ILeaderboardQueries leaderboardQueries)
        {
            SetLogTransaction();
            return await leaderboardQueries.UserPositionInRegionAsync(CurrentUser.Id, CurrentUser, Token);
        }

        public async Task<UserPositionDto> GetMyPositionAsync([Service] ILeaderboardQueries leaderboardQueries)
        {
            SetLogTransaction();
            return await leaderboardQueries.UserPositionInCountryAsync(CurrentUser.Id, CurrentUser, Token);
        }

        public async Task<bool> HasProductsImportsInProgressAsync([Service] IJobQueries jobQueries)
        {
            SetLogTransaction();
            return await jobQueries.HasProductsImportsInProgressAsync(CurrentUser.Id, CurrentUser, Token);
        }

        public async Task<bool> HasPickingOrdersExportsInProgressAsync([Service] IJobQueries jobQueries)
        {
            SetLogTransaction();
            return await jobQueries.HasPickingOrdersExportsInProgressAsync(CurrentUser.Id, CurrentUser, Token);
        }

        public async Task<IEnumerable<ProducerDeliveriesDto>> GetStoreDeliveriesForProducersAsync(SearchProducersDeliveriesDto input, [Service] IDeliveryQueries deliveryQueries)
        {
            SetLogTransaction(input);
            return await deliveryQueries.GetStoreDeliveriesForProducersAsync(CurrentUser.Id, input.Ids, input.Kinds, DateTimeOffset.UtcNow, CurrentUser, Token);
        }

        public async Task<IEnumerable<ProducerDeliveriesDto>> GetProducersDeliveriesAsync(SearchProducersDeliveriesDto input, [Service] IDeliveryQueries deliveryQueries)
        {
            SetLogTransaction(input);
            return await deliveryQueries.GetProducersDeliveriesAsync(input.Ids, input.Kinds, DateTimeOffset.UtcNow, CurrentUser, Token);
        }

        public async Task<SirenBusinessDto> SearchBusinessWithSiretAsync(string input, [Service] IUserQueries userQueries)
        {
            SetLogTransaction(input);
            return await userQueries.RetrieveSiretInfosAsync(input, CurrentUser, Token);
        }

        public async Task<ProducersSearchDto> SearchProducersAsync(SearchTermsDto input, [Service] IProducerQueries producerQueries)
        {
            SetLogTransaction(input);
            return await producerQueries.SearchProducersAsync(CurrentUser.Id, input, CurrentUser, Token);
        }

        public async Task<StoresSearchDto> SearchStoresAsync(SearchTermsDto input, [Service] IStoreQueries storeQueries)
        {
            SetLogTransaction(input);
            return await storeQueries.SearchStoresAsync(CurrentUser.Id, input, CurrentUser, Token);
        }

        public async Task<ProductsSearchDto> SearchProductsAsync(SearchProductsDto input, [Service] IProductQueries productQueries)
        {
            SetLogTransaction(input);
            return await productQueries.SearchAsync(input, CurrentUser, Token);
        }

        public async Task<IEnumerable<SuggestProducerDto>> SuggestProducersAsync(SearchTermsDto input, [Service] IProducerQueries producerQueries)
        {
            SetLogTransaction(input);
            return await producerQueries.SuggestProducersAsync(input, CurrentUser, Token);
        }

        public IQueryable<UserDto> GetMyUserProfile([Service] IUserQueries userQueries)
        {
            SetLogTransaction();
            return userQueries.GetUser(CurrentUser.Id, CurrentUser);
        }

        public IQueryable<ProductDto> GetStoreProducts([Service] IProductQueries productQueries)
        {
            SetLogTransaction();
            return productQueries.GetProducts(CurrentUser);
        }

        public IQueryable<ProducerDto> GetProducer(Guid input, [Service] IProducerQueries producerQueries)
        {
            SetLogTransaction(input);
            return producerQueries.GetProducer(input, CurrentUser);
        }

        public IQueryable<ProducerDto> GetProducers([Service] IProducerQueries producerQueries)
        {
            SetLogTransaction();
            return producerQueries.GetProducers(CurrentUser);
        }


        public IQueryable<ConsumerDto> GetConsumer(Guid input, [Service] IConsumerQueries consumerQueries)
        {
            SetLogTransaction(input);
            return consumerQueries.GetConsumer(input, CurrentUser);
        }

        public IQueryable<StoreDto> GetStore(Guid input, [Service] IStoreQueries storeQueries)
        {
            SetLogTransaction(input);
            return storeQueries.GetStore(input, CurrentUser);
        }

        public IQueryable<ConsumerLegalDto> GetConsumerLegals([Service] ILegalQueries legalQueries)
        {
            SetLogTransaction();
            return legalQueries.GetMyConsumerLegals(CurrentUser);
        }

        public IQueryable<BusinessLegalDto> GetBusinessLegals([Service] ILegalQueries legalQueries)
        {
            SetLogTransaction();
            return legalQueries.GetMyBusinessLegals(CurrentUser);
        }

        public IQueryable<WebPayinDto> GetWebPayinTransaction(string input, [Service] IPayinQueries payinQueries)
        {
            SetLogTransaction(input);
            return payinQueries.GetWebPayinTransaction(input, CurrentUser);
        }

        public IQueryable<CountryPointsDto> GetCountryPoints(Guid? input, [Service] ILeaderboardQueries leaderboardQueries)
        {
            SetLogTransaction(input);
            return leaderboardQueries.CountriesPoints(input, CurrentUser);
        }

        public IQueryable<RegionPointsDto> GetRegionsPoints(Guid? input, [Service] ILeaderboardQueries leaderboardQueries)
        {
            SetLogTransaction(input);
            return leaderboardQueries.RegionsPoints(input, CurrentUser);
        }

        public IQueryable<DepartmentPointsDto> GetDepartmentsPoints(Guid? input, [Service] ILeaderboardQueries leaderboardQueries)
        {
            SetLogTransaction(input);
            return leaderboardQueries.DepartmentsPoints(input, CurrentUser);
        }

        public IQueryable<CountryUserPointsDto> GetCountryUsersPoints(Guid? input, [Service] ILeaderboardQueries leaderboardQueries)
        {
            SetLogTransaction(input);
            return leaderboardQueries.CountryUsersPoints(input, CurrentUser);
        }

        public IQueryable<RegionUserPointsDto> GetRegionUsersPoints(Guid? input, [Service] ILeaderboardQueries leaderboardQueries)
        {
            SetLogTransaction(input);
            return leaderboardQueries.RegionUsersPoints(input, CurrentUser);
        }

        public IQueryable<DepartmentUserPointsDto> GetDepartmentUsersPoints(Guid? input, [Service] ILeaderboardQueries leaderboardQueries)
        {
            SetLogTransaction(input);
            return leaderboardQueries.DepartmentUsersPoints(input, CurrentUser);
        }

        public IQueryable<QuickOrderDto> GetMyDefaultQuickOrder([Service] IQuickOrderQueries quickOrderQueries)
        {
            SetLogTransaction();
            return quickOrderQueries.GetUserDefaultQuickOrder(CurrentUser.Id, CurrentUser);
        }

        public IQueryable<QuickOrderDto> GetQuickOrder(Guid input, [Service] IQuickOrderQueries quickOrderQueries)
        {
            SetLogTransaction(input);
            return quickOrderQueries.GetQuickOrder(input, CurrentUser);
        }

        public IQueryable<QuickOrderDto> GetQuickOrders([Service] IQuickOrderQueries quickOrderQueries)
        {
            SetLogTransaction();
            return quickOrderQueries.GetQuickOrders(CurrentUser);
        }

        public IQueryable<DocumentDto> GetDocuments([Service] IDocumentQueries documentQueries)
        {
            SetLogTransaction();
            return documentQueries.GetDocuments(CurrentUser);
        }

        public IQueryable<DepartmentDto> GetDepartments([Service] IDepartmentQueries departmentQueries)
        {
            SetLogTransaction();
            return departmentQueries.GetDepartments(CurrentUser);
        }

        public IQueryable<RegionDto> GetRegions([Service] IRegionQueries regionQueries)
        {
            SetLogTransaction();
            return regionQueries.GetRegions(CurrentUser);
        }

        public IQueryable<NationalityDto> GetNationalities([Service] INationalityQueries nationalityQueries)
        {
            SetLogTransaction();
            return nationalityQueries.GetNationalities(CurrentUser);
        }

        public IQueryable<CountryDto> GetCountries([Service] ICountryQueries countryQueries)
        {
            SetLogTransaction();
            return countryQueries.GetCountries(CurrentUser);
        }

        public IQueryable<NotificationDto> GetNotifications([Service] INotificationQueries notificationQueries)
        {
            SetLogTransaction();
            return notificationQueries.GetNotifications(CurrentUser);
        }

        public async Task<int> GetUnreadNotificationsCount([Service] INotificationQueries notificationQueries)
        {
            SetLogTransaction();
            return await notificationQueries.GetUnreadNotificationsCount(CurrentUser, Token);
        }

        public IQueryable<TagDto> GetTags([Service] ITagQueries tagQueries)
        {
            SetLogTransaction();
            return tagQueries.GetTags(CurrentUser);
        }

        public IQueryable<OrderDto> GetCurrentOrder([Service] IOrderQueries orderQueries)
        {
            SetLogTransaction();
            return orderQueries.GetCurrentOrder(CurrentUser);
        }
        
        public IQueryable<PayoutDto> GetPayout(Guid input, [Service] IPayoutQueries payoutQueries)
        {
            SetLogTransaction();
            return payoutQueries.GetPayout(input, CurrentUser);
        }
        
        public IQueryable<PayoutDto> GetPayouts([Service] IPayoutQueries payoutQueries)
        {
            SetLogTransaction();
            return payoutQueries.GetPayouts(CurrentUser);
        }
        
        public IQueryable<DonationDto> GetDonation(Guid input, [Service] IDonationQueries donationQueries)
        {
            SetLogTransaction();
            return donationQueries.GetDonation(input, CurrentUser);
        }
        
        public IQueryable<DonationDto> GetDonations([Service] IDonationQueries donationQueries)
        {
            SetLogTransaction();
            return donationQueries.GetDonations(CurrentUser);
        }
        
        public IQueryable<WithholdingDto> GetWithholding(Guid input, [Service] IWithholdingQueries withholdingQueries)
        {
            SetLogTransaction();
            return withholdingQueries.GetWithholding(input, CurrentUser);
        }
        
        public IQueryable<WithholdingDto> GetWithholdings([Service] IWithholdingQueries withholdingQueries)
        {
            SetLogTransaction();
            return withholdingQueries.GetWithholdings(CurrentUser);
        }

        public IQueryable<OrderDto> GetOrder(Guid input, [Service] IOrderQueries orderQueries)
        {
            SetLogTransaction(input);
            return orderQueries.GetOrder(input, CurrentUser);
        }

        public IQueryable<OrderDto> GetOrders([Service] IOrderQueries orderQueries)
        {
            SetLogTransaction();
            return orderQueries.GetOrders(CurrentUser);
        }

        public IQueryable<AgreementDto> GetAgreement(Guid input, [Service] IAgreementQueries agreementQueries)
        {
            SetLogTransaction(input);
            return agreementQueries.GetAgreement(input, CurrentUser);
        }

        public IQueryable<AgreementDto> GetAgreements([Service] IAgreementQueries agreementQueries)
        {
            SetLogTransaction();
            return agreementQueries.GetAgreements(CurrentUser);
        }

        public IQueryable<AgreementDto> GetStoreAgreements(Guid input, [Service] IAgreementQueries agreementQueries)
        {
            SetLogTransaction(input);
            return agreementQueries.GetStoreAgreements(input, CurrentUser);
        }

        public IQueryable<AgreementDto> GetProducerAgreements(Guid input, [Service] IAgreementQueries agreementQueries)
        {
            SetLogTransaction(input);
            return agreementQueries.GetProducerAgreements(input, CurrentUser);
        }

        public IQueryable<ProductDto> GetProduct(Guid input, [Service] IProductQueries productQueries)
        {
            SetLogTransaction(input);
            return productQueries.GetProduct(input, CurrentUser);
        }

        public IQueryable<ProductDto> GetProducerProducts(Guid input, [Service] IProductQueries productQueries)
        {
            SetLogTransaction(input);
            return productQueries.GetProducerProducts(input, CurrentUser);
        }

        public IQueryable<ProductDto> GetProducts([Service] IProductQueries productQueries)
        {
            SetLogTransaction();
            return productQueries.GetProducts(CurrentUser);
        }

        public IQueryable<ReturnableDto> GetReturnable(Guid input, [Service] IReturnableQueries returnableQueries)
        {
            SetLogTransaction(input);
            return returnableQueries.GetReturnable(input, CurrentUser);
        }

        public IQueryable<ReturnableDto> GetReturnables([Service] IReturnableQueries returnableQueries)
        {
            SetLogTransaction();
            return returnableQueries.GetReturnables(CurrentUser);
        }

        public IQueryable<JobDto> GetJob(Guid input, [Service] IJobQueries jobQueries)
        {
            SetLogTransaction(input);
            return jobQueries.GetJob(input, CurrentUser);
        }

        public IQueryable<JobDto> GetJobs([Service] IJobQueries jobQueries)
        {
            SetLogTransaction();
            return jobQueries.GetJobs(CurrentUser);
        }

        public IQueryable<DeliveryModeDto> GetDelivery(Guid input, [Service] IDeliveryQueries deliveryQueries)
        {
            SetLogTransaction(input);
            return deliveryQueries.GetDelivery(input, CurrentUser);
        }

        public IQueryable<DeliveryModeDto> GetDeliveries([Service] IDeliveryQueries deliveryQueries)
        {
            SetLogTransaction();
            return deliveryQueries.GetDeliveries(CurrentUser);
        }

        public IQueryable<PurchaseOrderDto> GetPurchaseOrder(Guid input, [Service] IPurchaseOrderQueries purchaseOrderQueries)
        {
            SetLogTransaction(input);
            return purchaseOrderQueries.GetPurchaseOrder(input, CurrentUser);
        }

        public IQueryable<PurchaseOrderDto> GetPurchaseOrders([Service] IPurchaseOrderQueries purchaseOrderQueries)
        {
            SetLogTransaction();
            return purchaseOrderQueries.GetPurchaseOrders(CurrentUser);
        }

        public IQueryable<PurchaseOrderDto> GetMyPurchaseOrders([Service] IPurchaseOrderQueries purchaseOrderQueries)
        {
            SetLogTransaction();
            return purchaseOrderQueries.MyPurchaseOrders(CurrentUser);
        }

        public IQueryable<ClosingDto> GetBusinessClosings([Service] IBusinessClosingQueries closingQueries)
        {
            SetLogTransaction();
            return closingQueries.GetClosings(CurrentUser);
        }

        public IQueryable<ClosingDto> GetDeliveryClosings(Guid? input, [Service] IDeliveryClosingQueries closingQueries)
        {
            SetLogTransaction();
            return closingQueries.GetClosings(input, CurrentUser);
        }

        public IQueryable<CatalogDto> GetCatalog(Guid input, [Service] ICatalogQueries catalogQueries)
        {
            SetLogTransaction();
            return catalogQueries.GetCatalog(input, CurrentUser);
        }

        public IQueryable<CatalogDto> GetCatalogs([Service] ICatalogQueries catalogQueries)
        {
            SetLogTransaction();
            return catalogQueries.GetCatalogs(CurrentUser);
        }

        public IQueryable<CatalogProductDto> GetCatalogProducts(Guid input, [Service] ICatalogQueries catalogQueries)
        {
            SetLogTransaction();
            return catalogQueries.GetCatalogProducts(input, CurrentUser);
        }

        private void SetLogTransaction(object input = null, [CallerMemberName] string name = "")
        {
            NewRelic.Api.Agent.NewRelic.SetTransactionName("GraphQL", name);

            var currentTransaction = NewRelic.Api.Agent.NewRelic.GetAgent().CurrentTransaction;
            currentTransaction.AddCustomAttribute("RequestId", _httpContextAccessor.HttpContext.TraceIdentifier);
            currentTransaction.AddCustomAttribute("UserIdentifier", CurrentUser.Id.ToString("N"));
            currentTransaction.AddCustomAttribute("IsAuthenticated", CurrentUser.IsAuthenticated.ToString());
            currentTransaction.AddCustomAttribute("Roles", string.Join(";", CurrentUser.Roles));
            currentTransaction.AddCustomAttribute("GraphQL", name);
            
            if(input != null)
                currentTransaction.AddCustomAttribute("Input", JsonConvert.SerializeObject(input));
        }
    }
}
