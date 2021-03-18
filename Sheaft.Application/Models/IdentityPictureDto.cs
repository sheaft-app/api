using System;

namespace Sheaft.Application.Models
{

    public class IdentityPictureDto
    {
        public IdentityPictureDto(Guid id, string picture)
        {
            Id = id.ToString("N");
            Picture = picture;
        }

        public string Id { get; set; }
        public string Picture { get; set; }
    }
}