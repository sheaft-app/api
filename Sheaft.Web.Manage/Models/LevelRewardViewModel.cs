﻿using System;

namespace Sheaft.Web.Manage.Models
{

    public class LevelRewardViewModel
    {
        public Guid Id { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? RemovedOn { get; set; }
        public string Name { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Url { get; set; }
        public string DepartmentName { get; set; }

    }
}
