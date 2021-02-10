using System;

namespace Sheaft.Application.Common.Models.ViewModels
{
    public class ProducerRequiredDeclarationViewModel
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Name { get; set; }
        public decimal Total { get; set; }
    }
}
