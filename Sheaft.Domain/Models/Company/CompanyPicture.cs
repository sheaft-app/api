using System;
using Sheaft.Domain.BaseClass;

namespace Sheaft.Domain
{
    public class CompanyPicture : Picture
    {
        protected CompanyPicture()
        {
        }
        
        internal CompanyPicture(string url, int position) : base(url, position)
        {
        }
    }
}