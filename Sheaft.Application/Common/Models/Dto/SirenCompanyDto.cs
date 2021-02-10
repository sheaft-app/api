namespace Sheaft.Application.Models
{
    public class SirenBusinessDto
    {
        public string Siren { get; set; }
        public string Nic { get; set; }
        public string Siret { get; set; }
        public SirenLegalsDto UniteLegale { get; set; }
        public SirenAddressDto AdresseEtablissement { get; set; }
    }
}
