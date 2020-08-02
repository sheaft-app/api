namespace Sheaft.Models.Dto
{
    public class SirenCompanyDto
    {
        public string Siren { get; set; }
        public string Nic { get; set; }
        public string Siret { get; set; }
        public SirenLegalsDto UniteLegale { get; set; }
        public SirenAddressDto AdresseEtablissement { get; set; }
    }
}
