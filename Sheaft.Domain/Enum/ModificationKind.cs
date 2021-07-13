namespace Sheaft.Domain.Enum
{
    public enum ModificationKind
    {
        ToDeliver,
        Missing,
        Broken,
        Improper,
        Excess,
    }

    public static class ModificationKindOverrides
    {
        public static string ToEnumString(this ModificationKind kind)
        {
            switch (kind)
            {
                case ModificationKind.ToDeliver:
                    return "à livrer";
                case ModificationKind.Broken:
                    return "cassé";
                case ModificationKind.Improper:
                    return "non conforme";
                case ModificationKind.Missing:
                    return "manquant";
                case ModificationKind.Excess:
                    return "en surplu";
                default:
                    return "";
            }
        }
    }
}