using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.Observation.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class UpdateObservationInputType : SheaftInputType<UpdateObservationCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateObservationCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("UpdateObservationInput");
            
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