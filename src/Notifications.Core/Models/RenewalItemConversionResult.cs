using System.Collections.Generic;

namespace Notifications.Core.Models
{
    /// <summary>
    /// Container to hold RenewalItem conversion validation result, and resulting IRenewalItem is valid.
    /// </summary>
    public class RenewalItemConversionResult
    {
        public Dictionary<int, string> Errors { get; internal set; }
        public IRenewalItem RenewalItem { get; internal set; }

        public RenewalItemConversionResult()
        {
            Errors = new Dictionary<int, string>();
        }
    }
}
