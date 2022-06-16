using Sheaft.Domain;

namespace Sheaft.Application.AgreementManagement;

public record AgreementDto(string Id, AgreementStatus Status, DateTimeOffset UpdatedOn, string SupplierName, string CustomerName);