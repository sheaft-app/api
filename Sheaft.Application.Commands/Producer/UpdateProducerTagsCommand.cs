using System;
using System.Collections.Generic;
using Sheaft.Domain.Enums;
using Sheaft.Application.Models;
using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Commands
{
    public class UpdateProducerTagsCommand : Command<bool>
    {
        [JsonConstructor]
        public UpdateProducerTagsCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid ProducerId { get; set; }
    }
}
