using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.Application.AgreementManagement;

public record AgreementDetailsDto
{
    public string Id { get; set; }
    public AgreementStatus Status { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public DateTimeOffset UpdatedOn { get; set; }
    public AgreementProfileDto Target { get; set; }
    public AgreementCatalogDto? Catalog { get; set; }
    public IEnumerable<DayOfWeek>? DeliveryDays { get; set; }
    public int? LimitOrderHourOffset { get; set; }
    public AddressDto DeliveryAddress { get; set; }
    public string OwnerId { get; set; }
    public AgreementOwner Owner { get; set; }
    public bool CanBeAcceptedOrRefused { get; set; }
    public bool CanBeCancelled { get; set; }
    public bool CanBeRevoked { get; set; }
}