namespace Sheaft.Application.Models
{
    public class PreAuthorizedPayinDto : PayinDto
    {
        public PreAuthorizationDto PreAuthorization { get; set; }
    }
}