using Sheaft.Application.Interop;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public abstract class Command<T> : ICommand<T>
    {
        protected Command(RequestUser requestUser)
        {
            RequestUser = requestUser;
        }

        public RequestUser RequestUser { get; private set; }

        public void RemoveUserInfos()
        {
            RequestUser = null;
        }
    }
}
