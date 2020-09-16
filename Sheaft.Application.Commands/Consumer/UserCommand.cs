using System;
using Newtonsoft.Json;
using Sheaft.Core;
using Sheaft.Models.Inputs;

namespace Sheaft.Application.Commands
{
    public abstract class ConsumerCommand<T> : Command<T>
    {
        [JsonConstructor]
        protected ConsumerCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Picture { get; set; }
        public bool Anonymous { get; set; }
        public Guid DepartmentId { get; set; }
    }
}
