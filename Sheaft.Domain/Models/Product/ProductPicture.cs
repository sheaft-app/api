using System;
using Sheaft.Domain.BaseClass;

namespace Sheaft.Domain
{
    public class ProductPicture : Picture
    {
        protected ProductPicture()
        {
        }
        
        public ProductPicture(string url, int position) : base(url, position)
        {
        }

        public Guid ProductId { get; private set; }
    }
}