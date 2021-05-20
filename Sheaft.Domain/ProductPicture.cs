using System;

namespace Sheaft.Domain
{
    public class ProductPicture : Picture
    {
        protected ProductPicture()
        {
        }
        
        public ProductPicture(Guid id, string url, int position) : base(id, url, position)
        {
        }

        public Guid ProductId { get; private set; }
    }
}