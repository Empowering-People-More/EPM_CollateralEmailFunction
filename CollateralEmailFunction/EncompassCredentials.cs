using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollateralEmailFunction
{
    public class EncompassCredentials
    {
        /// <summary>
        /// Encompass Instance
        /// </summary>
        public string? InstanceId { get; set; }

        /// <summary>
        /// Encompass Smart Client username
        /// </summary>
        public string? Username { get; set; }

        /// <summary>
        /// Encompass Smart Client username for the Disclosure Desk
        /// </summary>
        public string? DisclosureDeskUsername { get; set; }

        /// <summary>
        /// Encompass Smart Client password
        /// </summary>
        public string? Password { get; set; }

        /// <summary>
        /// Encompass Smart Client password for the Disclosure Desk
        /// </summary>
        public string? DisclosureDeskPassword { get; set; }

        /// <summary>
        /// Encompass API Client Id
        /// </summary>
        public string? ClientId { get; set; }

        /// <summary>
        /// Encompass API Client Secret
        /// </summary>
        public string? ClientSecret { get; set; }

        /// <summary>
        /// SendGrid API Key
        /// </summary>
        public string? SendGridKey { get; set; }
    }
}
