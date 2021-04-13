namespace Sheaft.Application.Models
{
    public class PreAuthorizedPayinDto : TransactionDto
    {
        public PreAuthorizationDto PreAuthorization { get; set; }
    }
}