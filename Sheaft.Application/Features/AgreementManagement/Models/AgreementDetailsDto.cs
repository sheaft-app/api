using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.Application.AgreementManagement;

public record AgreementDetailsDto
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