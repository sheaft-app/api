using System;
using Newtonsoft.Json;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class UpdateCompanyPictureCommand : Command<bool>
    {
        [JsonConstructor]
        public UpdateCompanyPictureCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
        public string Picture { get; set; }
    }
}
