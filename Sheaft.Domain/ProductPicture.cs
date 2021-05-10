using System;

namespace Sheaft.Domain
{
    public class ProductPicture : Picture
    {
        protected ProductPicture()
        {
        }
        
        public ProductPicture(Guid id, string url) : base(id, url)
        {
        }

        public Guid ProductId { get; private set; }
    }
}