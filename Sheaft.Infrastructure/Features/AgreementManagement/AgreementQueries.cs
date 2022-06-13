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

    public Task<Result<AgreementDetailsDto>> Get(AgreementId identifier, CancellationToken token)
    {
        return QueryAsync(async () =>
        {
            var agreementQuery =
                from agreement in _context.Agreements
                from supplier in _context.Suppliers.InnerJoin(c => c.Id == agreement.SupplierId)
                from customer in _context.Customers.InnerJoin(c => c.Id == agreement.CustomerId)
                from catalog in _context.Catalogs.LeftJoin(c => c.Id == agreement.CatalogId)
                where agreement.Id == identifier.Value
                select new AgreementDetailsDto
                {
                    Id = agreement.Id,
                    Status = (AgreementStatus) agreement.Status,
                    CreatedOn = agreement.CreatedOn,
                    UpdatedOn = agreement.UpdatedOn,
                    DeliveryDays = agreement.DeliveryDaysAgreementIds.Select(d => (DayOfWeek)d.DayOfWeek),
                    DeliveryAddress = new AddressDto(customer.DeliveryAddressStreet, customer.DeliveryAddressComplement, 
                        customer.DeliveryAddressPostcode, customer.DeliveryAddressCity),
                    Catalog = catalog != null && catalog.Id != null ? new AgreementCatalogDto(catalog.Id, catalog.Name) : null,
                    Supplier = new AgreementProfileDto(supplier.Id, supplier.TradeName, supplier.Email, supplier.Phone),
                    Customer = new AgreementProfileDto(customer.Id, customer.TradeName, customer.Email, customer.Phone)
                };

            var agreementResult = await agreementQuery.FirstOrDefaultAsync(token);
            return agreementResult == null
                ? Result.Failure<AgreementDetailsDto>(ErrorKind.NotFound, "agreement.notfound")
                : Result.Success(agreementResult);
        });
    }

    public Task<Result<PagedResult<AgreementDto>>> List(AccountId accountId, PageInfo pageInfo, CancellationToken token)
    {
        return QueryAsync(async () =>
        {
            var agreementsQuery =
                from agreement in _context.Agreements
                from supplier in _context.Suppliers.InnerJoin(c => c.Id == agreement.SupplierId)
                from customer in _context.Customers.InnerJoin(c => c.Id == agreement.CustomerId)
                where (agreement.Supplier.AccountId == accountId.Value || agreement.Customer.AccountId == accountId.Value) 
                    && (agreement.Status == (int)AgreementStatus.Active ||  agreement.Status == (int)AgreementStatus.Pending)
                select new AgreementDto
                {
                    Id = agreement.Id,
                    Status = (AgreementStatus) agreement.Status,
                    UpdatedOn = agreement.UpdatedOn,
                    CustomerName = customer.TradeName,
                    SupplierName = supplier.TradeName
                };

            var agreementsResults = await agreementsQuery
                .Skip(pageInfo.Skip)
                .Take(pageInfo.Take)
                .GroupBy(p => new {Total = agreementsQuery.Count()})
                .FirstOrDefaultAsync(token);

            return Result.Success(new PagedResult<AgreementDto>(agreementsResults?.Select(p => p), pageInfo,
                agreementsResults?.Key.Total ?? 0));
        });
    }

    public Task<Result<AvailableCustomerDto>> GetCustomer(CustomerId identifier, CancellationToken token)
    {
        return QueryAsync(async () =>
        {
            var customerQuery =
                from customer in _context.Customers
                where customer.Id == identifier.Value
                select new AvailableCustomerDto
                {
                    Id = customer.Id,
                    Email = customer.Email,
                    Name = customer.TradeName,
                    Phone = customer.Phone,
                    DeliveryAddress = new AddressDto(
                        customer.DeliveryAddressStreet, 
                        customer.DeliveryAddressComplement, 
                        customer.DeliveryAddressPostcode, 
                        customer.DeliveryAddressCity)
                };

            var customerResult = await customerQuery.FirstOrDefaultAsync(token);
            return customerResult == null
                ? Result.Failure<AvailableCustomerDto>(ErrorKind.NotFound, "customer.notfound")
                : Result.Success(customerResult);
        });
    }

    public Task<Result<AvailableSupplierDto>> GetSupplier(SupplierId identifier, CancellationToken token)
    {
        return QueryAsync(async () =>
        {
            var supplierQuery =
                from supplier in _context.Suppliers
                where supplier.Id == identifier.Value
                select new AvailableSupplierDto
                {
                    Id = supplier.Id,
                    Email = supplier.Email,
                    Name = supplier.TradeName,
                    Phone = supplier.Phone,
                    ShippingAddress = new AddressDto(
                        supplier.ShippingAddressStreet, 
                        supplier.ShippingAddressComplement, 
                        supplier.ShippingAddressPostcode, 
                        supplier.ShippingAddressCity)
                };

            var supplierResult = await supplierQuery.FirstOrDefaultAsync(token);
            return supplierResult == null
                ? Result.Failure<AvailableSupplierDto>(ErrorKind.NotFound, "supplier.notfound")
                : Result.Success(supplierResult);
        });
    }

    public Task<Result<PagedResult<AvailableCustomerDto>>> ListAvailableCustomersForSupplier(SupplierId supplierId, PageInfo pageInfo, CancellationToken token)
    {
        return QueryAsync(async () =>
        {
            var customersQuery =
                from customer in _context.Customers
                from agreement in _context.Agreements.LeftJoin(a => 
                    a.CustomerId == customer.Id && a.SupplierId == supplierId.Value && (a.Status == (int)AgreementStatus.Active || a.Status == (int)AgreementStatus.Pending))
                where agreement == null
                group customer by customer.Id into grouped
                select new AvailableCustomerDto
                {
                    Id = grouped.Key,
                    Name = grouped.First().TradeName,
                    Email = grouped.First().Email,
                    Phone = grouped.First().Phone,
                    DeliveryAddress = new AddressDto(
                        grouped.First().DeliveryAddressStreet, 
                        grouped.First().DeliveryAddressComplement, 
                        grouped.First().DeliveryAddressPostcode, 
                        grouped.First().DeliveryAddressCity)
                };

            var customersResults = await customersQuery
                .Skip(pageInfo.Skip)
                .Take(pageInfo.Take)
                .GroupBy(p => new {Total = customersQuery.Count()})
                .FirstOrDefaultAsync(token);

            return Result.Success(new PagedResult<AvailableCustomerDto>(customersResults?.Select(p => p), pageInfo,
                customersResults?.Key.Total ?? 0));
        });
    }

    public Task<Result<PagedResult<AvailableSupplierDto>>> ListAvailableSuppliersForCustomer(CustomerId customerId, PageInfo pageInfo, CancellationToken token)
    {
        return QueryAsync(async () =>
        {
            var suppliersQuery =
                from supplier in _context.Suppliers
                from agreement in _context.Agreements.LeftJoin(a => 
                    a.SupplierId == supplier.Id && a.CustomerId == customerId.Value && (a.Status == (int)AgreementStatus.Active || a.Status == (int)AgreementStatus.Pending))
                where agreement == null
                group supplier by supplier.Id into grouped
                select new AvailableSupplierDto
                {
                    Id = grouped.Key,
                    Name = grouped.First().TradeName,
                    Email = grouped.First().Email,
                    Phone = grouped.First().Phone,
                    ShippingAddress = new AddressDto(
                        grouped.First().ShippingAddressStreet, 
                        grouped.First().ShippingAddressComplement, 
                        grouped.First().ShippingAddressPostcode, 
                        grouped.First().ShippingAddressCity)
                };

            var suppliersResults = await suppliersQuery
                .Skip(pageInfo.Skip)
                .Take(pageInfo.Take)
                .GroupBy(p => new {Total = suppliersQuery.Count()})
                .FirstOrDefaultAsync(token);

            return Result.Success(new PagedResult<AvailableSupplierDto>(suppliersResults?.Select(p => p), pageInfo,
                suppliersResults?.Key.Total ?? 0));
        });
    }
}