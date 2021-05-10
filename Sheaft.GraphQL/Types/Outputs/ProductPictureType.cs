using HotChocolate.Resolvers;
using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.GraphQL.Products;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class ProductPictureType : SheaftOutputType<ProductPicture>
    {
        protected override void Configure(IObjectTypeDescriptor<ProductPicture> descriptor)
        {
            base.Configure(descriptor);
            
            descriptor
                .ImplementsNode()
                .IdField(c => c.Id)
                .ResolveNode((ctx, id) => ctx.DataLoader<ProductPicturesByIdBatchDataLoader>().LoadAsync(id, ctx.RequestAborted));
                
            descriptor
                .Field(c => c.Url)
                .Name("url");
        }
    }
}