namespace Sheaft.Application.Commands
{
    public class UpdateZoneProgressCommand : Command<bool>
    {
        public UpdateZoneProgressCommand(Interop.IRequestUser user) : base(user)
        {
        }
    }
}
