using HotChocolate.Types;
using Sheaft.Domain;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class BirthAddressType : SheaftOutputType<BirthAddress>
    {
        protected override void Configure(IObjectTypeDescriptor<BirthAddress> descriptor)
        {
            base.Configure(descriptor);
            
            descriptor
                .Field(c => c.City)
                .Name("city");
                
            descriptor
                .Field(c => c.Country)
                .Name("country");
        }
    }
}
