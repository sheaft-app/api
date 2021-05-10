using HotChocolate.Types;

namespace Sheaft.GraphQL.Types
{
    public abstract class SheaftInputType<T> : InputObjectType<T>
    {
        protected override void Configure(IInputObjectTypeDescriptor<T> descriptor)
        {
            base.Configure(descriptor);
            descriptor.BindFieldsExplicitly();
        }
    }
}
