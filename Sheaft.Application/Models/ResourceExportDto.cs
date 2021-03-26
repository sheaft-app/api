namespace Sheaft.Application.Models
{
    public class ResourceExportDto
    {
        public string MimeType { get; set; }
        public string Extension { get; set; }
        public byte[] Data { get; set; }
    }
}