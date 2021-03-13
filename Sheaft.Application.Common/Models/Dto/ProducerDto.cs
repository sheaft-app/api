using System;
using System.Collections.Generic;

namespace Sheaft.Application.Common.Models.Dto
{
    public class ProducerDto : UserDto
    {
        public string VatIdentifier { get; set; }
        public string Siret { get; set; }
        public bool OpenForNewBusiness { get; set; }
        public bool NotSubjectToVat { get; set; }
        public IEnumerable<TagDto> Tags { get; set; }
        public IEnumerable<ClosingDto> Closings { get; set; }
    }
}