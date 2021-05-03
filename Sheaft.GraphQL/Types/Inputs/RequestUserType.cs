using HotChocolate.Types;
using Sheaft.Domain;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class RequestUserType : SheaftInputType<RequestUser>
    {
        protected override void Configure(IInputObjectTypeDescriptor<RequestUser> descriptor)
        {
            base.Configure(descriptor);
        }
    }
}