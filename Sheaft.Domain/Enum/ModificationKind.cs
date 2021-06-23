namespace Sheaft.Domain.Enum
{
    public enum ModificationKind
    {
        Order,
        Preparation,
        Deliver,
        Missing,
        Broken,
        Improper
    }

    public static class ModificationKindOverrides
    {
        public static string ToEnumString(this ModificationKind kind)
        {
            switch (kind)
            {
                case ModificationKind.Broken:
                    return "cassé";
                case ModificationKind.Deliver:
                    return "livré";
                case ModificationKind.Improper:
                    return "non conforme";
                case ModificationKind.Missing:
                    return "manquant";
                case ModificationKind.Order:
                    return "commandé";
                case ModificationKind.Preparation:
                    return "préparé";
                default:
                    return "";
            }
        }
    }
}