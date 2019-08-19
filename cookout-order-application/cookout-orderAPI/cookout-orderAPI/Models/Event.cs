using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cookout_orderAPI.Models
{
    /// <summary>
    /// Model for a single instance of an event
    /// </summary>
    public class Event
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
    }
}
