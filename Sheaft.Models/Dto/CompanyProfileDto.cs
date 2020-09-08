namespace Sheaft.Models.Dto
{
    public class CompanyProfileDto : UserDto
    {
        public string Description { get; set; }
        public string VatIdentifier { get; set; }
        public string Siret { get; set; }
    }
}