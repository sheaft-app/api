using Sheaft.Domain;

namespace Sheaft.Application.AgreementManagement;

public record AgreementDto
{
    public string Id { get; set; }
    public AgreementStatus Status { get; set; }
    public DateTimeOffset UpdatedOn { get; set; }
    public string SupplierName { get; set; }
    public string CustomerName { get; set; }
}