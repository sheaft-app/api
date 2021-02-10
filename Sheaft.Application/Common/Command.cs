using Sheaft.Application.Common.Models;
using Sheaft.Domain;

namespace Sheaft.Application.Common
{
    public abstract class Command : ICommand
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
