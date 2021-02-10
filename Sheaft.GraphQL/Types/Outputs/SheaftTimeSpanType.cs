using HotChocolate.Types;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class SheaftTimeSpanType : TimeSpanType
    {
        public SheaftTimeSpanType() : base("TimeSpan", null, TimeSpanFormat.DotNet)
        {
        }
    }
}
