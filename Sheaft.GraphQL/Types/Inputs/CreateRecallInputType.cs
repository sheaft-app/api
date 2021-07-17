using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.Recall.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class CreateRecallInputType : SheaftInputType<CreateRecallCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreateRecallCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("CreateRecallInput");
            
            descriptor
                .Field(c => c.BatchIds)
                .ID(nameof(Batch))
                .Name("batchIds");
            
            descriptor
                .Field(c => c.ProductIds)
                .ID(nameof(Product))
                .Name("productIds");
            
            descriptor
                .Field(c => c.Name)
                .Name("name");
            
            descriptor
                .Field(c => c.Comment)
                .Name("comment");
            
            descriptor
                .Field(c => c.SaleStartedOn)
                .Name("saleStartedOn");
            
            descriptor
                .Field(c => c.SaleEndedOn)
                .Name("saleEndedOn");
        }
    }
}