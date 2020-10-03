using System;

namespace Sheaft.Web.Manage.Models
{
    [Serializable]
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
