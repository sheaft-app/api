﻿using System;

namespace Sheaft.Application.Models.Mailer
{
    public class PickingOrderExportMailerModel
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public string JobUrl { get; set; }
        public string DownloadUrl { get; set; }
    }
}
