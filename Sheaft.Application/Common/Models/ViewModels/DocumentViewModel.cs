﻿using System;
using System.Collections.Generic;

namespace Sheaft.Application.Common.Models.ViewModels
{
    public class DocumentViewModel : DocumentShortViewModel
    {
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset? UpdatedOn { get; private set; }
        public DateTimeOffset? RemovedOn { get; private set; }
        public IEnumerable<PageViewModel> Pages { get; set; }
    }
}