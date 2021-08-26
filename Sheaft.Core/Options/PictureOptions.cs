namespace Sheaft.Core.Options
{
    public class PictureOptions
    {
        public const string SETTING = "Pictures";

        public EntitySize User { get; set; } = new EntitySize
        {
            Small = new SizeInfos { Height = 128, Width = 128, Quality = 100 },
            Medium = new SizeInfos { Height = 620, Width = 620, Quality = 80 },
            Large = new SizeInfos { Height = 1280, Width = 1280, Quality = 70 }
        };

        public EntitySize Product { get; set; } = new EntitySize
        {
            Small = new SizeInfos { Height = 128, Width = 128, Quality = 100 },
            Medium = new SizeInfos { Height = 256, Width = 620, Quality = 80 },
            Large = new SizeInfos { Height = 512, Width = 1280, Quality = 70 }
        };

        public EntitySize Tag { get; set; } = new EntitySize
        {
            Small = new SizeInfos { Height = 128, Width = 128, Quality = 100 },
            Medium = new SizeInfos { Height = 256, Width = 256, Quality = 80 },
            Large = new SizeInfos { Height = 512, Width = 512, Quality = 70 }
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
