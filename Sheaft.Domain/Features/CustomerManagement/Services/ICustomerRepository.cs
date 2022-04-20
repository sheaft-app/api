namespace Sheaft.Domain.CustomerManagement;

public interface ICustomerRepository : IRepository<Customer, CustomerId>
{
    Task<Result<Customer>> Get(AccountId identifier, CancellationToken token);
    Task<Result<IEnumerable<CustomerInformation>>> GetInfo(IEnumerable<CustomerId> customerIdentifiers, CancellationToken token);
}