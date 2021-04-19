using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.Mediatr.BusinessClosing.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class UpdateOrCreateBusinessClosingsInputType : SheaftInputType<UpdateOrCreateBusinessClosingsCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateOrCreateBusinessClosingsCommand> descriptor)
        {
            descriptor.Name("UpdateOrCreateBusinessClosingsInput");
            descriptor.Field(c => c.UserId)
                .Name("Id")
                .Type<NonNullType<IdType>>();
            descriptor.Field(c => c.Closings)
                .Type<NonNullType<ListType<UpdateOrCreateClosingInputType>>>();
        }
    }
}