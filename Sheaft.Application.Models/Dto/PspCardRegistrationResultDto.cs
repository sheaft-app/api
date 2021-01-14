using Sheaft.Domain.Enums;

namespace Sheaft.Application.Models
{
    public class PspCardRegistrationResultDto
    {
        public string UserId { get; set; }
        public string AccessKey { get; set; }
        public string PreregistrationData { get; set; }
        public string CardRegistrationURL { get; set; }
        public string CardId { get; set; }
        public string RegistrationData { get; set; }
        public string ResultCode { get; set; }
        public CardStatus Status { get; set; }
    }
}