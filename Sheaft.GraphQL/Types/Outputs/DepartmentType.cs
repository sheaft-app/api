﻿using HotChocolate.Resolvers;
using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.GraphQL.Departments;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class DepartmentType : SheaftOutputType<Department>
    {
        protected override void Configure(IObjectTypeDescriptor<Department> descriptor)
        {
            base.Configure(descriptor);

            descriptor
                .ImplementsNode()
                .IdField(c => c.Id)
                .ResolveNode((ctx, id) =>
                    ctx.DataLoader<DepartmentsByIdBatchDataLoader>().LoadAsync(id, ctx.RequestAborted));
            
            descriptor
                .Field(c => c.Name)
                .Name("name")
                .Type<NonNullType<StringType>>();

            descriptor
                .Field(c => c.Code)
                .Name("code")
                .Type<NonNullType<StringType>>();

            descriptor
                .Field(c => c.ConsumersCount)
                .Name("consumersCount");
            
            descriptor
                .Field(c => c.StoresCount)
                .Name("storesCount");
            
            descriptor
                .Field(c => c.ProducersCount)
                .Name("producersCount");
            
            descriptor
                .Field(c => c.RequiredProducers)
                .Name("requiredProducers");
        }
    }
}
