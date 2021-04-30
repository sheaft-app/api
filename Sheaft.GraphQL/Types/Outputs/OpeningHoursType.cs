using HotChocolate.Types;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class OpeningHoursType : ObjectType<Domain.OpeningHours>
    {
        protected override void Configure(IObjectTypeDescriptor<Domain.OpeningHours> descriptor)
        {
            base.Configure(descriptor);
            
            descriptor
                .Field(c => c.Day)
                .Name("day");
                
            descriptor
                .Field(c => c.From)
                .Name("from");
                
            descriptor
                .Field(c => c.To)
                .Name("to");
        }
    }
}