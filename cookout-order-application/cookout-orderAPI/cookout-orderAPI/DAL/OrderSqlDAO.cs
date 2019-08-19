using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using cookout_orderAPI.Models;

namespace cookout_orderAPI.DAL
{
    public class OrderSqlDAO : IOrderDAO
    {
        private readonly string connectionString;
        private const string _getLastIdSQL = "SELECT CAST(SCOPE_IDENTITY() as int);";

        /// <summary>
        /// Creates a new sql dao for event objects.
        /// </summary>
        /// <param name="connectionString">the database connection string</param>
        public OrderSqlDAO(string connectionString)
        {
            this.connectionString = connectionString;
        }



        /// <summary>
        /// updates order to be completed
        /// </summary>
        /// <param name="orderId"></param>
        public void completeOrder(int orderId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("UPDATE orders SET completed = 1 WHERE order_id = @id;", conn);
                    cmd.Parameters.AddWithValue("@id", orderId);
                    cmd.ExecuteNonQuery();

                    return;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// maps new order to new row in  database
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private Order MapRowToOrder(SqlDataReader reader)
        {
            return new Order()
            {
                OrderID = Convert.ToInt32(reader["order_id"]),
                OrderSubmitter = Convert.ToInt32(reader["app_user_id"]),
                EventId = Convert.ToInt32(reader["event_id"]),
                IsCompleted = Convert.ToBoolean(reader["completed"])
            };
        }

        /// <summary>
        /// creates the order. I think.
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public int createOrder(Order order)
        {
            int newOrderId = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO orders (app_user_id, event_id, completed) VALUES (@user_id, @event_id, @completed)" + _getLastIdSQL, conn);
                    cmd.Parameters.AddWithValue("@user_id", order.OrderSubmitter);
                    cmd.Parameters.AddWithValue("@event_id", order.EventId);
                    cmd.Parameters.AddWithValue("@completed", order.IsCompleted);
                    newOrderId = (int)cmd.ExecuteScalar();

                    return newOrderId;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// When order is placed, adds quantity, item ID, and orderId to Order_Items table. This will enable access to get item names by order.
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="itemId"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public int PopulateOrder_Items(int orderId, int itemId, int quantity)
        {
            int newId = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO order_items (order_id, item_id, item_quantity) VALUES (@order, @item, @count)" + _getLastIdSQL, conn);
                    cmd.Parameters.AddWithValue("@order", orderId);
                    cmd.Parameters.AddWithValue("@item", itemId);
                    cmd.Parameters.AddWithValue("@count", quantity);
                    newId = (int)cmd.ExecuteScalar();

                    return newId;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// gets the order
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public Order getOrder(int orderId)
        {
            Order result = new Order();
            const string sql = "SELECT * FROM orders where order_id = @orderId";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@orderId", orderId);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    result = MapRowToOrder(reader);
                }
                return result;
            }

        }

        public List<List<OrderItem>> GetOrderItems(List<int> orderIds)
        {
            List<List<OrderItem>> result = new List<List<OrderItem>>();
            foreach (int orderId in orderIds)
            {
                List<OrderItem> items = new List<OrderItem>();
                const string sql = " Select orders.order_id, app_user_id, item_name From orders" +
                                    "join order_items on order_items.order_id = orders.order_id" +
                                    "join item on item.item_id = order_items.item_id" +
                                    "Where orders.order_id = @orderId; ";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@id", orderId);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        items.Add(MapRowToOrderItem(reader));
                    }
                    result.Add(items);
                }
                
            }
            return result;
        }

        private OrderItem MapRowToOrderItem(SqlDataReader reader)
        {
            return new OrderItem()
            {
                ItemID = Convert.ToInt32(reader["order_id"]),
                ItemName = Convert.ToString(reader["item_name"]),
                ItemDescription = Convert.ToString(reader["item_description"]),
                ItemQty = Convert.ToInt32(reader["item_quantity"])
            };
        }


    }
}
