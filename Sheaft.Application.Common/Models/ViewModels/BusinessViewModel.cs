using System;
using System.Collections.Generic;
using Sheaft.Domain.Enum;

namespace Sheaft.Application.Common.Models.ViewModels
{
    public class ProducerViewModel
    {
        public Guid Id { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public DateTimeOffset? RemovedOn { get; set; }
        public ProfileKind Kind { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Reason { get; set; }
        public string Picture { get; set; }
        public bool OpenForNewBusiness { get; set; }
        public AddressViewModel Address { get; set; }
        public IEnumerable<Guid> Tags { get; set; }
        public bool NotSubjectToVat { get; set; }
    }
}
