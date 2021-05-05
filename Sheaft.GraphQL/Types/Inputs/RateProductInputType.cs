using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.Domain;
using Sheaft.Mediatr.Product.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class RateProductInputType : SheaftInputType<RateProductCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<RateProductCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("RateProductInput");
            
            descriptor
                .Field(c => c.Value)
                .Name("value");
                
            descriptor
                .Field(c => c.Comment)
                .Name("comment");
                
            descriptor.Field(c => c.ProductId)
                .Name("id")
                .ID(nameof(Product));
        }
    }
}
