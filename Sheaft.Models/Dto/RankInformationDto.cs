using System.Collections.Generic;
using System.Linq;

namespace Sheaft.Models.Dto
{
    public class RankInformationDto
    {
        public RankInformationDto(Dictionary<string, int> ranks, int points)
        {
            Points = points;
            var i = 0;
            var orderedRanks = ranks.OrderBy(r => r.Value).ToArray();
            foreach (var rank in orderedRanks)
            {
                if (points < rank.Value)
                {
                    Rank = rank.Key;
                    PointsToLevelUp = rank.Value - Points;
                    if (i + 1 < orderedRanks.Length)
                    {
                        NextRank = orderedRanks[i + 1].Key;
                    }
                    else
                    {
                        NextRank = null;
                    }
                    break;
                }

                i++;
            }
        }

        public int Points { get; set; }
        public string Rank { get; set; }
        public string NextRank { get; set; }
        public int? PointsToLevelUp { get; set; }
    }
}