using HotChocolate.Types;

namespace Sheaft.GraphQL.Types
{
    public abstract class SheaftOutputType<T> : ObjectType<T>
    {
        protected override void Configure(IObjectTypeDescriptor<T> descriptor)
        {
            base.Configure(descriptor);
            descriptor.BindFieldsExplicitly();
        }
    }
}
