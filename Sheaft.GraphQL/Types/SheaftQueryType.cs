using HotChocolate.Types;
using Sheaft.GraphQL.Types;

namespace Sheaft.GraphQL
{
    public class SheaftQueryType : ObjectType<SheaftQuery>
    {
        protected override void Configure(IObjectTypeDescriptor<SheaftQuery> descriptor)
        {
            descriptor.Field(c => c.GetAgreement(default, default)).Argument("input", c => c.Type<NonNullType<IdType>>());
            descriptor.Field(c => c.GetAgreements(default));
            descriptor.Field(c => c.GetStoreAgreements(default, default)).Argument("input", c => c.Type<NonNullType<IdType>>());
            descriptor.Field(c => c.GetProducerAgreements(default, default)).Argument("input", c => c.Type<NonNullType<IdType>>());
            descriptor.Field(c => c.GetCountryPoints(default, default)).Argument("input", c => c.Type<IdType>());
            descriptor.Field(c => c.GetCountryUsersPoints(default, default)).Argument("input", c => c.Type<IdType>());
            descriptor.Field(c => c.GetDeliveries(default));
            descriptor.Field(c => c.GetDelivery(default, default)).Argument("input", c => c.Type<NonNullType<IdType>>());
            descriptor.Field(c => c.GetDepartments(default));
            descriptor.Field(c => c.GetNationalities(default));
            descriptor.Field(c => c.GetCountries(default));
            descriptor.Field(c => c.GetDepartmentsPoints(default, default)).Argument("input", c => c.Type<IdType>());
            descriptor.Field(c => c.GetDepartmentUsersPoints(default, default)).Argument("input", c => c.Type<IdType>());
            descriptor.Field(c => c.GetFreshdeskTokenAsync(default));
            descriptor.Field(c => c.GetJob(default, default)).Argument("input", c => c.Type<NonNullType<IdType>>());
            descriptor.Field(c => c.GetJobs(default));
            descriptor.Field(c => c.GetMyCompany(default));
            descriptor.Field(c => c.GetMyDefaultQuickOrder(default));
            descriptor.Field(c => c.GetMyOrders(default));
            descriptor.Field(c => c.GetMyPositionAsync(default));
            descriptor.Field(c => c.GetMyPositionInDepartment(default));
            descriptor.Field(c => c.GetMyPositionInRegion(default));
            descriptor.Field(c => c.GetMyRankInformationAsync(default));
            descriptor.Field(c => c.GetNotifications(default));
            descriptor.Field(c => c.GetReturnable(default, default)).Argument("input", c => c.Type<NonNullType<IdType>>());
            descriptor.Field(c => c.GetReturnables(default));
            descriptor.Field(c => c.GetProducer(default, default)).Argument("input", c => c.Type<NonNullType<IdType>>());
            descriptor.Field(c => c.GetProducersDeliveriesAsync(default, default)).Argument("input", c => c.Type<SearchProducersDeliveriesInputType>());
            descriptor.Field(c => c.GetProduct(default, default)).Argument("input", c => c.Type<NonNullType<IdType>>());
            descriptor.Field(c => c.GetProducerProducts(default, default)).Argument("input", c => c.Type<NonNullType<IdType>>());
            descriptor.Field(c => c.GetProducts(default));
            descriptor.Field(c => c.GetPurchaseOrder(default, default)).Argument("input", c => c.Type<NonNullType<IdType>>());
            descriptor.Field(c => c.GetPurchaseOrders(default));
            descriptor.Field(c => c.GetQuickOrder(default, default)).Argument("input", c => c.Type<NonNullType<IdType>>());
            descriptor.Field(c => c.GetQuickOrders(default));
            descriptor.Field(c => c.GetRegions(default));
            descriptor.Field(c => c.GetRegionsPoints(default, default)).Argument("input", c => c.Type<IdType>());
            descriptor.Field(c => c.GetRegionUsersPoints(default, default)).Argument("input", c => c.Type<IdType>());
            descriptor.Field(c => c.GetStore(default, default)).Argument("input", c => c.Type<NonNullType<IdType>>());
            descriptor.Field(c => c.GetStoreDeliveriesForProducersAsync(default, default));
            descriptor.Field(c => c.GetStoreProducts(default));
            descriptor.Field(c => c.GetTags(default));
            descriptor.Field(c => c.HasPickingOrdersExportsInProgressAsync(default));
            descriptor.Field(c => c.HasProductsImportsInProgressAsync(default));
            descriptor.Field(c => c.Me(default));
            descriptor.Field(c => c.RetrieveSiretCompanyInfosAsync(default, default)).Argument("input", c => c.Type<NonNullType<StringType>>());
            descriptor.Field(c => c.SearchProducersAsync(default, default)).Argument("input", c => c.Type<SearchTermsInputType>());
            descriptor.Field(c => c.SearchProductsAsync(default, default)).Argument("input", c => c.Type<SearchTermsInputType>());
            descriptor.Field(c => c.SearchStoresAsync(default, default)).Argument("input", c => c.Type<SearchTermsInputType>());
        }
    }
}
