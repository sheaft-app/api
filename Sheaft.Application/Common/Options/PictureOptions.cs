namespace Sheaft.Options
{
    public class PictureOptions
    {
        public const string SETTING = "Pictures";

        public EntitySize User { get; set; } = new EntitySize
        {
            Small = new SizeInfos { Height = 64, Width = 64, Quality = 100 },
            Medium = new SizeInfos { Height = 128, Width = 128, Quality = 80 },
            Large = new SizeInfos { Height = 256, Width = 256, Quality = 60 }
        };

        public EntitySize Product { get; set; } = new EntitySize
        {
            Small = new SizeInfos { Height = 64, Width = 64, Quality = 100 },
            Medium = new SizeInfos { Height = 128, Width = 310, Quality = 80 },
            Large = new SizeInfos { Height = 256, Width = 620, Quality = 60 }
        };

        public EntitySize Tag { get; set; } = new EntitySize
        {
            Small = new SizeInfos { Height = 64, Width = 64, Quality = 100 },
            Medium = new SizeInfos { Height = 128, Width = 128, Quality = 80 },
            Large = new SizeInfos { Height = 256, Width = 256, Quality = 60 }
        };
    }

    public class EntitySize
    {
        public SizeInfos Small { get; set; }
        public SizeInfos Medium { get; set; }
        public SizeInfos Large { get; set; }
    }

    public class SizeInfos
    {
        public int Height { get; set; }
        public int Width { get; set; }
        public int Quality { get; set; }
    }
}
