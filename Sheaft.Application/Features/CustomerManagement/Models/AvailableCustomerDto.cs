using Sheaft.Application.Models;

namespace Sheaft.Application.CustomerManagement;

public record AvailableCustomerDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public AddressDto DeliveryAddress { get; set; }
}