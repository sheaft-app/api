using Sheaft.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Sheaft.Application.Models
{
    public class DocumentViewModel : DocumentShortViewModel
    {
        public LegalViewModel Legal { get; set; }
        public IEnumerable<PageViewModel> Pages { get; set; }
    }
}
