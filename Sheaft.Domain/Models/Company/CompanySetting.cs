using System;

namespace Sheaft.Domain
{
    public class CompanySetting
    {
        protected CompanySetting(){}
        
        internal CompanySetting(Guid companyId, Setting setting, string value)
        {
            CompanyId = companyId;
            Setting = setting;
            SettingId = setting.Id;
            Value = value;
        }

        public Guid CompanyId { get; set; }
        public string Value { get; set; }
        public Guid SettingId { get; private set; }
        public Setting Setting { get; private set; }
    }
}