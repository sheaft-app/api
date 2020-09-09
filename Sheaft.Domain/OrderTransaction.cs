using System;
using Sheaft.Interop;
using Sheaft.Interop.Enums;

namespace Sheaft.Domain.Models
{

    public class OrderTransaction : Transaction
    {
        protected OrderTransaction()
        {
        }

        public OrderTransaction(Guid id)
            :base(id)
        {
        }
    }
}