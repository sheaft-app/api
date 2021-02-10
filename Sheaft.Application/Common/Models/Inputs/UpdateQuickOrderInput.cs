using System;

namespace Sheaft.Application.Common.Models.Inputs
{
    public class UpdateQuickOrderInput : QuickOrderInput
    {
        public Guid Id { get; set; }
    }
}