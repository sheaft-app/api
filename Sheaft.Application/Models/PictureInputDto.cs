using System;

namespace Sheaft.Application.Models
{
    public class PictureInputDto
    {
        public Guid? Id { get; set; }
        public int Position { get; set; }
        public string Data { get; set; }
    }
}