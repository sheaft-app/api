namespace Sheaft.Mailing
{
    public class DeliveryUserMailerModel
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DeliveryAddressMailerModel Address { get; set; }
        public string Siret { get; set; }
    }
}