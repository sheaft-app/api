using System.Collections.Generic;

namespace Sheaft.Application.Common.Models.Dto
{
    public class ProfileInformationDto
    {
        public string Summary { get; set; }
        public string Description { get; set; }
        public string Website { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string Instagram { get; set; }
        public string Banner { get; set; }
        public List<PictureDto> Pictures { get; set; }
    }
}