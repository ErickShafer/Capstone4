using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cookout_orderAPI.Models
{
    public class Order
    {
        /// <summary>
        ///Order ID number that identifies it uniquely in the database
        /// </summary>
        public int OrderID { get; set; }

        /// <summary>
        /// Name of the person that made the order
        /// </summary>
        public int OrderSubmitter { get; set; }

        /// <summary>
        /// List of items in the order
        /// </summary>
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        /// <summary>
        /// Status for the order defaults to false
        /// </summary>
        public bool IsCompleted { get; set; } = false;

        /// <summary>
        /// event for the order
        /// </summary>
        public int EventId { get; set; }

    }
}
