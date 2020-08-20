using System;

namespace Sheaft.Manage.Models
{
    [Serializable]
    public class EntityViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}
