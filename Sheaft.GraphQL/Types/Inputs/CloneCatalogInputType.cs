using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.Domain;
using Sheaft.Mediatr.Catalog.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class CloneCatalogInputType : SheaftInputType<CloneCatalogCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CloneCatalogCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("CloneCatalogInput");

            descriptor
                .Field(c => c.CatalogId)
                .Name("id")
                .ID(nameof(Catalog));
            
            descriptor
                .Field(c => c.Name)
                .Name("name");

            descriptor
                .Field(c => c.Percent)
                .Name("percent");
        }
    }
}