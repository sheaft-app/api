using Sheaft.Application.Models;

namespace Sheaft.Application.SupplierManagement;

public record AvailableSupplierDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public AddressDto ShippingAddress { get; set; }
}