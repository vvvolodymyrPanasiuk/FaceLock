using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceLock.Authentication.DTO
{
    /// <summary>
    /// Data transfer object (DTO) for user metadata information.
    /// </summary>
    public class UserMetaDataDTO
    {
        /// <summary>
        /// Country of the user.
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        /// City of the user.
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// Device of the user.
        /// </summary>
        public string Device { get; set; }
    }
}
