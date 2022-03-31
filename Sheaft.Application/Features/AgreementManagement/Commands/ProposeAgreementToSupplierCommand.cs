using Sheaft.Domain;

namespace Sheaft.Application.AgreementManagement;

public record ProposeAgreementToSupplierCommand() : ICommand<Result<string>>;

public class ProposeAgreementToSupplierHandler : ICommandHandler<ProposeAgreementToSupplierCommand, Result<string>>
{
    private readonly IUnitOfWork _uow;

    public ProposeAgreementToSupplierHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public Task<Result<string>> Handle(ProposeAgreementToSupplierCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}