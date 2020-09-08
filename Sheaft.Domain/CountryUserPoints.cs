using Sheaft.Interop.Enums;
using System;

namespace Sheaft.Domain.Views
{
    public class CountryUserPoints
    {
        public Guid UserId { get; set; }
        public ProfileKind Kind { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public int? Points { get; set; }
        public long Position { get; set; }        
    }
}