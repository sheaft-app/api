using System;

namespace Sheaft.Application.Common.Models.Inputs
{
    public class ExportTransactionsInput
    {
        public DateTimeOffset From { get; set; }
        public DateTimeOffset To { get; set; }
        public string Name { get; set; }
    }
}