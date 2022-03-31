namespace Sheaft.Domain.AgreementManagement;

public class Agreement : AggregateRoot
{
    public Agreement()
    {
        Identifier = AgreementId.New();
    }

    public AgreementId Identifier { get; } 
}