using Sheaft.Core;
using System;

namespace Sheaft.Application.Commands
{

    public class UpdateDepartmentCommand : Command<bool>
    {
        public const string QUEUE_NAME = "command-departments-update";

        public UpdateDepartmentCommand(RequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
        public int Points { get; set; }
        public int Position { get; set; }
        public int Users { get; set; }
        public int Producers { get; set; }
        public int Stores { get; set; }
    }
}
