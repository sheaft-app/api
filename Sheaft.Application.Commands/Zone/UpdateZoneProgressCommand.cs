using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class UpdateZoneProgressCommand : Command<bool>
    {
        public UpdateZoneProgressCommand(RequestUser user) : base(user)
        {
        }
    }
    public class GenerateZonesFileCommand : Command<bool>
    {
        public GenerateZonesFileCommand(RequestUser user) : base(user)
        {
        }
    }
}
