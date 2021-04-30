using HotChocolate.Resolvers;
using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.GraphQL.ProfilePictures;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class ProfilePictureType : SheaftOutputType<ProfilePicture>
    {
        protected override void Configure(IObjectTypeDescriptor<ProfilePicture> descriptor)
        {
            base.Configure(descriptor);
            
            descriptor
                .ImplementsNode()
                .IdField(c => c.Id)
                .ResolveNode((ctx, id) => ctx.DataLoader<ProfilePicturesByIdBatchDataLoader>().LoadAsync(id, ctx.RequestAborted));
                
            descriptor
                .Field(c => c.Url)
                .Name("url");
        }
    }
}