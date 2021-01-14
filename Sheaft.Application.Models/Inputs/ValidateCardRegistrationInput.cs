using System;

namespace Sheaft.Application.Models
{
    public class ValidateCardRegistrationInput
    {
        public Guid CardId { get; set; }
        public string RegistrationData { get; set; }
        public bool? Remember { get; set; }
    }
}