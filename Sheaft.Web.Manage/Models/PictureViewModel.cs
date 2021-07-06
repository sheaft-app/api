using System;

namespace Sheaft.Web.Manage.Models
{
    public class PictureViewModel
    {
        public Guid Id { get; set; }
        public string Url { get; set; }
        public int Position { get; set; }
        public bool Remove { get; set; }
    }
}