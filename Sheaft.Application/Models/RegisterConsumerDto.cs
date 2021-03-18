namespace Sheaft.Application.Models
{
    public class RegisterConsumerDto 
    {
        public bool Anonymous { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Picture { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public string Website { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string Instagram { get; set; }
        public AddressDto Address { get; set; }
        public string SponsoringCode { get; set; }
    }
}