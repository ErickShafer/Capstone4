using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cookout_orderAPI.Models
{
    public class OrderItem
    {
        /// <summary>
        /// Unique ID for the menu item
        /// </summary>
        public int ItemID { get; set; }

        /// <summary>
        /// Name of the menu itemdd
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// Description for the menu item
        /// </summary>
        public string ItemDescription { get; set; }

        public int ItemQty { get; set; } = 0;


    }
}
