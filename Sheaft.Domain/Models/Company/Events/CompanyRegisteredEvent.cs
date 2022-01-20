using System;
using Newtonsoft.Json;
using Sheaft.Domain.Common;

namespace Sheaft.Domain.Events.Company
{
    public class CompanyRegistered : DomainEvent
    {
        [JsonConstructor]
        public CompanyRegistered(Guid companyId)
        {
            CompanyId = companyId;
        }

        public Guid CompanyId { get; }
    }
}
