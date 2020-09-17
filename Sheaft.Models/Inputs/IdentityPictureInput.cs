﻿using System;
using System.Collections.Generic;

namespace Sheaft.Models.Inputs
{

    public class IdentityPictureInput
    {
        public IdentityPictureInput(Guid id, string picture)
        {
            Id = id.ToString("N");
            Picture = picture;
        }

        public string Id { get; set; }
        public string Picture { get; set; }
    }
}