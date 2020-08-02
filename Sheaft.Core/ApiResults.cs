using System.Collections.Generic;

namespace Sheaft.Core
{
    public class ApiResults<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int? Count { get; set; }
        public string NextPage { get; set; }
        public string PreviousPage { get; set; }
    }
}