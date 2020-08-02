using System.Collections.Generic;

namespace Sheaft.Options
{
    public class ScoringOptions
    {
        public const string SETTING = "Scoring";
        public Dictionary<string, int> Ranks { get; set; }
        public Dictionary<string, int> Points { get; set; }
    }
}
