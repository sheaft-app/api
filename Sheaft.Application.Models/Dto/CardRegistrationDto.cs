using System;
using Sheaft.Domain.Enums;

namespace Sheaft.Application.Models
{
    public class CardRegistrationDto
    {
        public Guid Id { get; set; }
        public string Identifier { get; set; }
        public string Url { get; set; }
        public string RegistrationData { get; set; }
        public string PreRegistrationData { get; set; }
        public string AccessKey { get; set; }
        public CardStatus Status { get; set; }
    }
}