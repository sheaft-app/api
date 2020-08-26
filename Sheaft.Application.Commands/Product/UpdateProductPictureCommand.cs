using System;
using Newtonsoft.Json;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class UpdateProductPictureCommand : Command<bool>
    {
        [JsonConstructor]
        public UpdateProductPictureCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
        public string Picture { get; set; }
    }
}
