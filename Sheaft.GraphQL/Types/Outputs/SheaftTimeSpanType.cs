using HotChocolate.Types;
using HotChocolate.Types.Relay;
using Sheaft.Application.Models;
using Sheaft.Application.Queries;
using Sheaft.GraphQL.Enums;
using Sheaft.GraphQL.Filters;
using Sheaft.GraphQL.Sorts;

namespace Sheaft.GraphQL.Types
{
    public class SheaftTimeSpanType : TimeSpanType
    {
        public SheaftTimeSpanType() : base("TimeSpan", null, TimeSpanFormat.DotNet)
        {
        }
    }
}
