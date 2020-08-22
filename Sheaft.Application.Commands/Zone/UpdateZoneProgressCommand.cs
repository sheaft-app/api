using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class UpdateZoneProgressCommand : Command<bool>
    {
        public UpdateZoneProgressCommand(RequestUser user) : base(user)
        {
        }
    }
}
