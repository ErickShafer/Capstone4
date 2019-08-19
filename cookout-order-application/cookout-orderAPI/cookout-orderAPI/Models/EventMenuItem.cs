using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cookout_orderAPI.Models
{
    /// <summary>
    /// Holds all information for a single menu item
    /// </summary>
    public class EventMenuItem
    {
        /// <summary>
        /// Id of menu item
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Description of menu item
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Name of menu item
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Count of menu item
        /// </summary>
        public int ItemQty { get; set; } = 0;
    }
}
