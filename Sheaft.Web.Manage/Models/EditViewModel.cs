using System;

namespace Sheaft.Web.Manage.Models
{
    [Serializable]
    public class EntityViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}
