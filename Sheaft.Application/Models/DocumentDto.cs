﻿using System;
using System.Collections.Generic;
using Sheaft.Domain.Enum;

namespace Sheaft.Application.Models
{
    public class DocumentDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DocumentStatus Status { get; set; }
        public string ReasonCode { get; set; }
        public string ReasonMessage { get; set; }
        public IEnumerable<PageDto> Pages { get; set; }
    }
}