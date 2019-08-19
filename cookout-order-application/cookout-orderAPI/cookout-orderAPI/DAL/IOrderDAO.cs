using cookout_orderAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cookout_orderAPI.DAL
{
    public interface IOrderDAO
    {

        /// <summary>
        /// retrieves an order from the database by orderID.
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        Order getOrder(int orderId);

        /// <summary>
        /// creates a new order.
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        int createOrder(Order order);


        /// <summary>
        /// changes the comleted status of an order.
        /// </summary>
        void completeOrder(int orderId);

        int PopulateOrder_Items(int orderId, int itemId, int quantity);

        List<List<OrderItem>> GetOrderItems(List<int> orderIds);

    }
}
