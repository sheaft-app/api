namespace Sheaft.Domain
{
    public class UserSetting
    {
        protected UserSetting(){}
        
        public UserSetting(Setting setting, string value)
        {
            Setting = setting;
            Value = value;
        }
        
        public string Value { get; private set; }
        public virtual Setting Setting { get; private set; }
    }
}