using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using cookout_orderAPI.Models;

namespace cookout_orderAPI.DAL
{
    /// <summary>
    /// A SQL Dao for user objects.
    /// </summary>
    public class UserSqlDAO : IUserDAO
    {
        private readonly string connectionString;
        private const string _getLastIdSQL = "SELECT CAST(SCOPE_IDENTITY() as int);";

        /// <summary>
        /// Creates a new sql dao for user objects.
        /// </summary>
        /// <param name="connectionString">the database connection string</param>
        public UserSqlDAO(string connectionString)
        {
            this.connectionString = connectionString;
        }

        #region User Login

        /// <summary>
        /// Saves the user to the database.
        /// </summary>
        /// <param name="user"></param>
        public int CreateUser(User user)
        {
            int newUserId = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO users (app_user_name, email, user_hash, user_salt, role_id) VALUES (@username, @email, @user_hash, @salt, @role)" + _getLastIdSQL, conn);
                    cmd.Parameters.AddWithValue("@username", user.Username);
                    cmd.Parameters.AddWithValue("@email", user.Email);
                    cmd.Parameters.AddWithValue("@user_hash", user.Password);
                    cmd.Parameters.AddWithValue("@salt", user.Salt);
                    cmd.Parameters.AddWithValue("@role", user.Role);

                    newUserId = (int)cmd.ExecuteScalar();

                    return newUserId;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Deletes the user from the database.
        /// </summary>
        /// <param name="user"></param>
        public void DeleteUser(User user)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM users WHERE app_user_id = @id;", conn);
                    cmd.Parameters.AddWithValue("@id", user.Id);

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
        /// Gets the user from the database.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public User GetUser(int id)
        {
            User result = new User();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM USERS WHERE app_user_id = @id;", conn);
                    cmd.Parameters.AddWithValue("@id", id);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        result = MapRowToUser(reader);

                    }
                }
                return result;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Updates the user role in the database.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newRoleId"></param>
        public void UpdateUser(int userId, int newRoleId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("UPDATE users SET role_id = @role WHERE app_user_id = @id;", conn);
                    cmd.Parameters.AddWithValue("@role", newRoleId);
                    cmd.Parameters.AddWithValue("@id", userId);

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
        /// Returns a User Via provided Email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public User GetUserByEmail(string email)
        {
            User user = new User();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM USERS WHERE email = @email;", conn);
                    cmd.Parameters.AddWithValue("@email", email);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        user = MapRowToUser(reader);

                    }
                }
                return user;
            }
            catch (SqlException ex)
            {
                throw ex;
            }

        }
        /// <summary>
        /// Gets all user info by user name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public User GetUserByUsername(string name)
        {
            User user = new User();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM USERS WHERE app_user_name = @name;", conn);
                    cmd.Parameters.AddWithValue("@name", name);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        user = MapRowToUser(reader);

                    }
                }
                return user;
            }
            catch (SqlException ex)
            {
                throw ex;
            }

        }

        private User MapRowToUser(SqlDataReader reader)
        {
            return new User()
            {
                Id = Convert.ToInt32(reader["app_user_id"]),
                Username = Convert.ToString(reader["app_user_name"]),
                Password = Convert.ToString(reader["user_hash"]),
                Salt = Convert.ToString(reader["user_salt"]),
                Email = Convert.ToString(reader["email"]),
                Role = Convert.ToString(reader["role_id"])
            };
        }
        #endregion

        /// <summary>
        /// Gets all events associated with a single user Id where that user is a guest
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public List<Event> GetEventByUser(string userName)
        {
            List<Event> result = new List<Event>();

            const string sql = "Select event_name, event_description, event_date, app_host_id, event_table.event_id from event_table " +
                                "join guest on guest.event_id = event_table.event_id " +
                                "join users on users.app_user_id = guest.app_user_id " +
                                "where users.app_user_name = @name;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@name", userName);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(GetEventFromReader(reader));
                }
                return result;
            }

        }

        /// <summary>
        /// Gets all events where user is host by host id of them events.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<Event> GetHostEventByHostId(int id, string name)
        {
            List<Event> result = new List<Event>();

            const string sql = "Select distinct event_name, event_description, event_date, app_host_id, event_table.event_id from event_table " +
                                "join guest on guest.event_id = event_table.event_id " +
                                "join users on users.app_user_id = guest.app_user_id " +
                                "where event_table.app_host_id = @id or users.app_user_name = @name;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@name", name);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(GetEventFromReader(reader));
                }
                return result;
            }
        }

        public List<Event> GetEventByHostId(int id)
        {
            List<Event> result = new List<Event>();

            const string sql = "SELECT * FROM event_table WHERE event_table.app_host_id = @id;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(GetEventFromReader(reader));
                }
                return result;
            }
        }

        private Event GetEventFromReader(SqlDataReader reader)
        {
            Event item = new Event();
            {
                item.Name = Convert.ToString(reader["event_name"]);
                item.Description = Convert.ToString(reader["event_description"]);
                item.HostId = Convert.ToInt32(reader["app_host_id"]);
                item.Date = Convert.ToDateTime(reader["event_date"]);
                item.Id = Convert.ToInt32(reader["event_id"]);
            }
            return item;
        }

        /// <summary>
        /// Gets list of all registered users
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public List<User> GetUsers()
        {
            List<User> result = new List<User>();
            const string sql = "SELECT * FROM Users";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(MapRowToUser(reader));
                }
                return result;
            }
        }

        public EventGuestVM GetGuestsByEventId(int eventId)
        {
            EventGuestVM result = new EventGuestVM();
            const string sql = "select app_user_name from users " +
                               "join guest on guest.app_user_id = users.app_user_id " +
                               "where event_id = @event_id; ";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@event_id", eventId);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    result.GuestName.Add(AddGuestFromReader(reader));
                }
                return result;
            }
        }

        private string AddGuestFromReader(SqlDataReader reader)
        {
            string item;
            {
                item = Convert.ToString(reader["app_user_name"]);
                
            }
            return item;
        }


        /// <summary>
        /// retreives a list of all orders that are incomplete by event ID
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        public List<OrderScreen> GetOrdersByEventID(int eventId)
        {

            Dictionary<int, OrderScreen> result = new Dictionary<int, OrderScreen>();

            const string sql = "select users.app_user_id, users.app_user_name, orders.order_id, item.item_name, order_items.item_quantity, orders.completed, item.item_name, item.item_description, item.item_id from item " +
                               "join order_items on item.item_id = order_items.item_id " +
                               "join orders on orders.order_id = order_items.order_id " +
                               "join users on users.app_user_id = orders.app_user_id " +
                               "where event_id = @eventId and orders.completed = 0 " +
                               "group by item.item_name, orders.completed, orders.order_id, item.item_name, item.item_description, item.item_id, order_items.item_quantity, users.app_user_id, users.app_user_name;";


            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@eventId", eventId);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int orderId = Convert.ToInt32(reader["order_id"]);
                    if(!result.ContainsKey(orderId))
                    {
                        var orderScreen = GetOrderScreenFromReader(reader);
                        orderScreen.Items.Add(GetOrderItemFromReader(reader));
                        result.Add(orderId, orderScreen);
                    }
                    else
                    {
                        var orderScreen = result[orderId];
                        orderScreen.Items.Add(GetOrderItemFromReader(reader));
                    }
                }                
            }

            return result.Values.ToList();
        }

        private OrderItem GetOrderItemFromReader(SqlDataReader reader)
        {
            OrderItem oi = new OrderItem();
            oi.ItemID = Convert.ToInt32(reader["item_id"]);
            oi.ItemName = Convert.ToString(reader["item_name"]);
            oi.ItemDescription = Convert.ToString(reader["item_description"]);
            oi.ItemQty = Convert.ToInt32(reader["item_quantity"]);

            return oi;
        }

        private OrderScreen GetOrderScreenFromReader(SqlDataReader reader)
        {
            OrderScreen item = new OrderScreen();
            item.OrderId = Convert.ToInt32(reader["order_id"]);
            item.Complete = Convert.ToBoolean(reader["completed"]);
            item.UserId = Convert.ToInt32(reader["app_user_id"]);
            item.UserName = Convert.ToString(reader["app_user_name"]);

            return item;
        }

        public List<int> GetOrderIdsByEventId(int eventId)
        {
            List<int> result = new List<int>();
            const string sql = "select orders.order_id from orders " +
                               "where event_id = @eventId and orders.completed = 0; ";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@eventId", eventId);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(GetOrderIdFromReader(reader));
                }
                return result;
            }
        }

        private int GetOrderIdFromReader(SqlDataReader reader)
        {
            int item; 
   
            {
                item = Convert.ToInt32(reader["order_id"]);
            }
            return item;
        }


        

        public string GetHostNameForEvent(int eventID)
        {
            String Host = "";
            const string sql = "select app_user_name from users where app_user_id = @eventID";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                Host = Convert.ToString(reader["app_user_name"]);

                return Host;
            }
        }

        public List<User> GetGuestListByEvent(int eventId)
        {
            List<User> result = new List<User>();
            const string sql = "Select users.app_user_name from users " +
            "JOIN guest on guest.app_user_id = users.app_user_id where event_id = @id;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", eventId);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(MapRowToUser(reader));
                }
                return result;
            }

        }

        /// <summary>
        /// Returns the Host-Id for the Given Event-Id
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        public int GetHostIdByEvent(int eventId)
        {
            int hostId = 0;
            const string sql = "select host_id from event_table where event_id = @eventId";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@eventId", eventId);
                hostId = (int)cmd.ExecuteScalar();
                return hostId;
            }
        }

        /// <summary>
        /// Updates the Host of the Given Event-Id to the Given Host-Id
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="eventId"></param>
        /// <returns>Int Host-ID</returns>
        public int AddHost(int userId, int eventId)
        {
            int hostId = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("UPDATE event_table SET app_host_id = @userId WHERE event_id = @eventId;", conn);
                    cmd.Parameters.AddWithValue("@eventId", eventId);
                    cmd.Parameters.AddWithValue("@id", userId);

                    hostId = (int)cmd.ExecuteScalar();

                    return hostId;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }

        }


        public int TestMakingEvent()
        {
            int newEventId = 0;
            //    using (SqlConnection conn = new SqlConnection(connectionString))
            //    {
            //        conn.Open();
            //        SqlCommand cmd = new SqlCommand("INSERT INTO event_table(" +
            //            "event_name, event_description, event_date) VALUES" +
            //            "('testEvent', 'testDescription', 12-25-00)", conn);
            //        newEventId = (int)cmd.ExecuteScalar();
            return newEventId;
            //    }
        }

        private Event MapRowToEvent(SqlDataReader reader)
        {
            return new Event()
            {
                Id = Convert.ToInt32(reader["event_id"]),
                HostId = Convert.ToInt32(reader["app_host_id"]),
                Name = Convert.ToString(reader["event_name"]),
                Description = Convert.ToString(reader["event_description"]),
                Date = Convert.ToDateTime(reader["event_date"])

            };
        }


    }
}
