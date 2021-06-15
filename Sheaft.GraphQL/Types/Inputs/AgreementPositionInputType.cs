using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class AgreementPositionInputType : SheaftInputType<AgreementPositionDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<AgreementPositionDto> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("AgreementPositionInput");

            descriptor
                .Field(c => c.Id)
                .Name("id")
                .ID(nameof(Agreement));

            descriptor
                .Field(c => c.Position)
                .Name("position");
        }
    }
}