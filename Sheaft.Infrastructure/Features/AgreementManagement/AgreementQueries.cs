using DataModel;
using LinqToDB;
using Sheaft.Application;
using Sheaft.Application.AgreementManagement;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.AgreementManagement;

internal class AgreementQueries : Queries, IAgreementQueries
{
    private readonly AppDb _context;

    public AgreementQueries(AppDb context)
    {
        _context = context;
    }

    public Task<Result<AgreementDto>> Get(AgreementId identifier, CancellationToken token)
    {
        return QueryAsync(async () =>
        {
            var agreementQuery =
                from agreement in _context.Agreements
                from supplier in _context.Suppliers.Where(c => c.Id == agreement.SupplierId)
                from customer in _context.Customers.Where(c => c.Id == agreement.CustomerId)
                from catalog in _context.Catalogs.Where(c => c.Id == agreement.CatalogId).DefaultIfEmpty()
                where agreement.Id == identifier.Value
                select GetAgreementDto(agreement, customer, catalog, supplier);

            var agreementResult = await agreementQuery.FirstOrDefaultAsync(token);
            return agreementResult == null
                ? Result.Failure<AgreementDto>(ErrorKind.NotFound, "agreement.notfound")
                : Result.Success(agreementResult);
        });
    }

    public Task<Result<PagedResult<AgreementDto>>> List(AccountId accountId, PageInfo pageInfo, CancellationToken token)
    {
        return QueryAsync(async () =>
        {
            var agreementsQuery =
                from agreement in _context.Agreements
                from supplier in _context.Suppliers.Where(c => c.Id == agreement.SupplierId)
                from customer in _context.Customers.Where(c => c.Id == agreement.CustomerId)
                from catalog in _context.Catalogs.Where(c => c.Id == agreement.CatalogId).DefaultIfEmpty()
                where supplier.AccountId == accountId.Value || customer.AccountId == accountId.Value
                select GetAgreementDto(agreement, customer, catalog, supplier);

            var agreementsResult = await agreementsQuery
                .Skip(pageInfo.Skip)
                .Take(pageInfo.Take)
                .GroupBy(p => new {Total = agreementsQuery.Count()})
                .FirstOrDefaultAsync(token);

            return Result.Success(new PagedResult<AgreementDto>(agreementsResult?.Select(p => p), pageInfo,
                agreementsResult?.Key.Total ?? 0));
        });
    }

    private static AgreementDto GetAgreementDto(Agreement p, Customer cu, Catalog ca, Supplier s)
    {
        return new AgreementDto
        {
            Id = p.Id,
            Status = (AgreementStatus) p.Status,
            CreatedOn = p.CreatedOn,
            UpdatedOn = p.UpdatedOn,
            DeliveryDays = p.DeliveryDaysAgreementIds.Select(d => (DayOfWeek)d.DayOfWeek),
            DeliveryAddress = new AddressDto(cu.DeliveryAddressStreet, cu.DeliveryAddressComplement, 
                cu.DeliveryAddressPostcode, cu.DeliveryAddressCity),
            Catalog = ca?.Id != null ? new AgreementCatalogDto(ca.Id, ca.Name) : null,
            Supplier = new AgreementProfileDto(s.Id, s.TradeName, s.Email, s.Phone),
            Customer = new AgreementProfileDto(cu.Id, cu.TradeName, cu.Email, cu.Phone)
        };
    }
}