using System;

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