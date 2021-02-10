using System;
using System.Collections.Generic;
using Sheaft.Domain.Enum;

namespace Sheaft.Application.Common.Models.ViewModels
{
    public class DeclarationViewModel
    {
        public Guid Id { get; private set; }
        public DeclarationStatus Status { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public DateTimeOffset? ProcessedOn { get; set; }
        public string ReasonCode { get; set; }
        public string ReasonMessage { get; set; }
        public IEnumerable<UboViewModel> Ubos { get; set; }
    }
}
