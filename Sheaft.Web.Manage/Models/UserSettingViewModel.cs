using System;
using Sheaft.Domain.Enum;

namespace Sheaft.Web.Manage.Models
{
    public class UserSettingViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public SettingKind Kind { get; set; }
        public string Value { get; set; }
    }
}