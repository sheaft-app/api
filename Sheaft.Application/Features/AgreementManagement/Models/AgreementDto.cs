using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.Application.AgreementManagement;

public record AgreementDto
{
    public string Id { get; set; }
    public AgreementStatus Status { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public DateTimeOffset UpdatedOn { get; set; }
    public AgreementProfileDto Supplier { get; set; }
    public AgreementProfileDto Customer { get; set; }
    public AgreementCatalogDto? Catalog { get; set; }
    public IEnumerable<DayOfWeek>? DeliveryDays { get; set; }
    public AddressDto DeliveryAddress { get; set; }
}

public record AgreementProfileDto(string Id, string Name, string Email, string Phone);
public record AgreementCatalogDto(string Id, string Name);