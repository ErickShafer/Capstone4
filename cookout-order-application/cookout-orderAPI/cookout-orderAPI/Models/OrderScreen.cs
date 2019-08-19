using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cookout_orderAPI.Models
{
    /// <summary>
    /// Model that holds all information that will be viewed by the chef on the order screen
    /// </summary>
    public class OrderScreen
    {
        /// <summary>
        /// Order Id associated with each menu item
        /// </summary>
        public int OrderId { get; set; }

        public int UserId { get; set; }

        public string UserName { get; set; }

        /// <summary>
        /// A list of all orderitems for an order
        /// </summary>
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();

        /// <summary>
        /// Determines if order has been complete
        /// </summary>
        public bool Complete { get; set; }

    }
}
