using HotChocolate.Resolvers;
using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.Domain;
using Sheaft.GraphQL.Cards;
using Sheaft.GraphQL.Catalogs;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class CardType : SheaftOutputType<Card>
    {
        protected override void Configure(IObjectTypeDescriptor<Card> descriptor)
        {
            base.Configure(descriptor);

            descriptor
                .ImplementsNode()
                .IdField(c => c.Id)
                .ResolveNode((ctx, id) => 
                    ctx.DataLoader<CardsByIdBatchDataLoader>().LoadAsync(id, ctx.RequestAborted));

            descriptor
                .Field(c => c.Name)
                .Name("name");
            
            descriptor
                .Field(c => c.IsActive)
                .Name("isActive");
            
            descriptor
                .Field(c => c.CreatedOn)
                .Name("createdOn");
        }
    }
}