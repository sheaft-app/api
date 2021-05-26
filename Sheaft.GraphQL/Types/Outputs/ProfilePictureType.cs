using System;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Sheaft.Application.Extensions;
using Sheaft.Core;
using Sheaft.Domain;
using Sheaft.GraphQL.Users;

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
                .Field(c => c.Position)
                .Name("position");
            
            descriptor
                .Field(c => PictureExtensions.GetPictureUrl(Guid.Empty, c.Url, PictureSize.LARGE))
                .Name("large");
            descriptor
                .Field(c => PictureExtensions.GetPictureUrl(Guid.Empty, c.Url, PictureSize.MEDIUM))
                .Name("medium");
            descriptor
                .Field(c => PictureExtensions.GetPictureUrl(Guid.Empty, c.Url, PictureSize.SMALL))
                .Name("small");
        }
    }
}