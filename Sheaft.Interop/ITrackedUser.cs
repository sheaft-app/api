using System;
using System.Collections.Generic;

namespace Sheaft.Interop
{
    public interface ITrackedUser
    {
        public IRequestUser RequestUser { get; }
    }
}