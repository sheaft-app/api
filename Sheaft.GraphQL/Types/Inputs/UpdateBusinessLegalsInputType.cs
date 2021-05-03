﻿using HotChocolate.Types;
using Sheaft.Mediatr.Legal.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class UpdateBusinessLegalsInputType : SheaftInputType<UpdateBusinessLegalCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateBusinessLegalCommand> descriptor)
        {
            descriptor.Name("UpdateBusinessLegalInput");
            descriptor.Field(c => c.VatIdentifier);

            descriptor.Field(c => c.Name)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Email)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.LegalId)
                .Name("id")
                .Type<NonNullType<IdType>>();

            descriptor.Field(c => c.Kind);

            descriptor.Field(c => c.Owner)
                .Type<NonNullType<CreateOwnerInputType>>();

            descriptor.Field(c => c.Address)
                .Type<NonNullType<AddressInputType>>();

            descriptor.Field(c => c.Siret)
                .Type<NonNullType<StringType>>();
        }
    }
}
