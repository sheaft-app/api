namespace Sheaft.Domain
{
    public class Role
    {
        protected Role()
        {
        }
        
        public Role(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}