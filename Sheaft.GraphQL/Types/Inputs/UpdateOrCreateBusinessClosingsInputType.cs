using HotChocolate.Types;
using Sheaft.Mediatr.Business.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class UpdateOrCreateBusinessClosingsInputType : SheaftInputType<UpdateOrCreateBusinessClosingsCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateOrCreateBusinessClosingsCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("UpdateOrCreateBusinessClosingsInput");
            
            descriptor
                .Field(c => c.Closings)
                .Name("closings")
                .Type<NonNullType<ListType<BusinessClosingInputType>>>();
        }
    }
}