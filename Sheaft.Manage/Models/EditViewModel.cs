using System;

namespace Sheaft.Manage.Models
{
    [Serializable]
    public class EditViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}
