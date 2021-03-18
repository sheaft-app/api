using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class SirenAddressType : ObjectType<SirenAddressDto>
    {
        protected override void Configure(IObjectTypeDescriptor<SirenAddressDto> descriptor)
        {
            descriptor.Field("line1").Resolver(c =>
            {
                var source = c.Parent<SirenAddressDto>();
                return $"{source.NumeroVoieEtablissement} {source.TypeVoieEtablissement} {source.LibelleVoieEtablissement}";
            });

            descriptor.Field(c => c.ComplementAdresseEtablissement)
                .Name("line2");

            descriptor.Field(c => c.CodePostalEtablissement)
                .Name("zipcode");

            descriptor.Field(c => c.LibelleCommuneEtablissement)
                .Name("city");
        }
    }
}
