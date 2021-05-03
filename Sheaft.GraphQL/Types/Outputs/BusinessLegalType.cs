using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Models;
using Sheaft.Domain;
using Sheaft.GraphQL.Business;
using Sheaft.GraphQL.Catalogs;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class BusinessLegalType : SheaftOutputType<BusinessLegal>
    {
        protected override void Configure(IObjectTypeDescriptor<BusinessLegal> descriptor)
        {
            base.Configure(descriptor);

            descriptor
                .ImplementsNode()
                .IdField(c => c.Id)
                .ResolveNode((ctx, id) =>
                    ctx.DataLoader<BusinessLegalsByIdBatchDataLoader>().LoadAsync(id, ctx.RequestAborted));
            
            descriptor
                .Field(c => c.Kind)
                .Name("kind");
                
            descriptor
                .Field(c => c.Validation)
                .Name("validation");
                
            descriptor
                .Field(c => c.Email)
                .Name("email");
                
            descriptor
                .Field(c => c.Address)
                .Name("address");
                
            descriptor
                .Field(c => c.Owner)
                .Name("owner");
        }
    }
}
