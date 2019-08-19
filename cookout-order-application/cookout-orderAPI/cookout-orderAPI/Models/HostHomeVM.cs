using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cookout_orderAPI.Models
{
    public class HostHomeVM
    {

        /// <summary>
        /// Name of the event
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Event description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Date of the event
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// The user ID of the event host
        /// </summary>
        public int HostId { get; set; }

        /// <summary>
        /// ID of the specific event
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Role for the party the host is attending
        /// </summary>
        public string Role { get; set; }
    }
}

