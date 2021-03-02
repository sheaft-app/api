﻿using HotChocolate.Types;
using Sheaft.Domain.Enum;

namespace Sheaft.GraphQL.Enums
{
    public class DocumentStatusEnumType : EnumType<DocumentStatus>
    {
        protected override void Configure(IEnumTypeDescriptor<DocumentStatus> descriptor)
        {
            descriptor.Value(DocumentStatus.NotSpecified).Name("NOT_SPECIFIED");
            descriptor.Value(DocumentStatus.Created).Name("CREATED");
            descriptor.Value(DocumentStatus.OutOfDate).Name("OUT_OF_DATE");
            descriptor.Value(DocumentStatus.Refused).Name("REFUSED");
            descriptor.Value(DocumentStatus.Validated).Name("VALIDATED");
            descriptor.Value(DocumentStatus.ValidationAsked).Name("VALIDATION_ASKED");
            descriptor.Value(DocumentStatus.UnLocked).Name("UNLOCKED");
            descriptor.Value(DocumentStatus.Locked).Name("LOCKED");
        }
    }
}