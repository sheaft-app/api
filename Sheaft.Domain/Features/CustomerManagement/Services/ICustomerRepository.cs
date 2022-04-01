namespace Sheaft.Domain;

public interface ICustomerRepository : IRepository<Customer, CustomerId>
{
    Task<Result<Customer>> Get(AccountId identifier, CancellationToken token);
}