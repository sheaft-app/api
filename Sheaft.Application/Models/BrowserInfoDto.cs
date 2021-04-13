namespace Sheaft.Application.Models
{
    public class BrowserInfoDto
    {
        public string AcceptHeader { get; set; }
        public bool JavaEnabled { get; set; }
        public string Language { get; set; }
        public int ColorDepth { get; set; }
        public int ScreenHeight { get; set; }
        public int ScreenWidth { get; set; }
        public string TimeZoneOffset { get; set; }
        public string UserAgent { get; set; }
        public bool JavascriptEnabled { get; set; }
    }
}