using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cookout_orderAPI.Models
{
    /// <summary>
    /// Model holding information to create a menu
    /// </summary>
    public class MenuVM
    {
        /// <summary>
        /// Selection of Items For the given Event
        /// </summary>
        public List<int> itemIds { get; set; }

        /// <summary>
        /// The Event the Items are For
        /// </summary>
        public int eventId { get; set; }

    }
}
