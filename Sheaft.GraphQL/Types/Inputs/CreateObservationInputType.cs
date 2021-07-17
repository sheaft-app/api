using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.Observation.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class CreateObservationInputType : SheaftInputType<CreateObservationCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreateObservationCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("CreateObservationInput");
            
            descriptor
                .Field(c => c.BatchIds)
                .ID(nameof(Batch))
                .Name("batchIds");
            
            descriptor
                .Field(c => c.ProductIds)
                .ID(nameof(Product))
                .Name("productIds");
            
            descriptor
                .Field(c => c.Comment)
                .Name("comment");
                
            descriptor
                .Field(c => c.VisibleToAll)
                .Name("visibleToAll");
        }
    }
}