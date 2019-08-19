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
    public class EventSqlDAO : IEventDAO
    {
        private readonly string connectionString;
        private const string _getLastIdSQL = "SELECT CAST(SCOPE_IDENTITY() as int);";

        /// <summary>
        /// Creates a new sql dao for event objects.
        /// </summary>
        /// <param name="connectionString">the database connection string</param>
        public EventSqlDAO(string connectionString)
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
        /// Updates the user in the database.
        /// </summary>
        /// <param name="user"></param>
        public void UpdateUser(User user)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("UPDATE users SET user_hash = @password, role_id = @role WHERE app_user_id = @id;", conn);
                    cmd.Parameters.AddWithValue("@password", user.Password);
                    cmd.Parameters.AddWithValue("@role", user.Role);
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
        /// Gets list of all registered events
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Event GetEvent(int id)
        {
            Event result = new Event();
            const string sql = "SELECT * FROM Event_Table where app_user_id = @userId";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@userId", id);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    result = MapRowToEvent(reader);
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

        /// <summary>
        /// Saves the event to the database.
        /// </summary>
        /// <param name="cookoutEvent"></param>
        public int CreateEvent(Event cookoutEvent)
        {
            int newEventId = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO event_table (app_host_id, event_name, event_description, event_date) VALUES (@host_id, @event_name, @event_description, @event_date)" + _getLastIdSQL, conn);
                    cmd.Parameters.AddWithValue("@host_id", cookoutEvent.HostId);
                    cmd.Parameters.AddWithValue("@event_name", cookoutEvent.Name);
                    cmd.Parameters.AddWithValue("@event_description", cookoutEvent.Description);
                    cmd.Parameters.AddWithValue("@event_date", cookoutEvent.Date);

                    newEventId = (int)cmd.ExecuteScalar();

                    return newEventId;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Gets a list of all items in the database. 
        /// </summary>
        /// <param name="ItemID"></param>
        /// <returns></returns>
        public List<OrderItem> GetItems()
        {
            List<OrderItem> result = new List<OrderItem>();

            const string sql = "SELECT * FROM Item";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(GetItemFromReader(reader));
                }
                return result;
            }

        }

        private OrderItem GetItemFromReader(SqlDataReader reader)
        {
            OrderItem item = new OrderItem();
            {
                item.ItemName = Convert.ToString(reader["item_name"]);
                item.ItemDescription = Convert.ToString(reader["item_description"]);
                item.ItemID = Convert.ToInt32(reader["item_id"]);
            }
            return item;
        }


        /// <summary>
        /// Assigns selected list of items to an event id to create a menu.
        /// </summary>
        /// <returns></returns>
        public void AddItems(MenuVM menu)
        {
            
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    foreach (int item in menu.itemIds)
                    {
                        SqlCommand cmd = new SqlCommand("INSERT INTO event_item (item_id, event_id) VALUES (@item_id, @event_id)", conn);
                        cmd.Parameters.AddWithValue("@item_id", item);
                        cmd.Parameters.AddWithValue("@event_id", menu.eventId);
                        cmd.ExecuteNonQuery();
                    }

                    return;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }

        }

        



        /// <summary>
        /// Gets list of all registered users
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public List<User> GetGuests(int UserId)
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

       

        /// <summary>
        /// Adds a list of selected users to an event id to create a GuestList.
        /// </summary>
        /// <param name="Guests"></param>
        /// <param name="id"></param>
        public void AddGuests(GuestListVM invite)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    foreach (int user in invite.guestIds)
                    {
                        SqlCommand cmd = new SqlCommand("INSERT INTO guest (app_user_id, event_id) VALUES (@app_user_id, @event_id)" + _getLastIdSQL, conn);
                       
                        cmd.Parameters.AddWithValue("@app_user_id", user);
                        cmd.Parameters.AddWithValue("@event_id", invite.eventId);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public void AddHostasAGuest(int hostId, int eventId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO guest (app_user_id, event_id) VALUES (@app_user_id, @event_id)" + _getLastIdSQL, conn);
                    cmd.Parameters.AddWithValue("@app_user_id", hostId);
                    cmd.Parameters.AddWithValue("@event_id", eventId);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public int GetEventByEventName(string name)
        {
            int id = 0;
            const string sql = "SELECT * FROM Event_Table where event_name = @name"+_getLastIdSQL;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@name", name);
                id = (int)cmd.ExecuteScalar();

                return id;
            }
        }

        public Event GetEventByEventId(int eventId)
        {
            Event result = null;
            const string sql = "select * from event_table where event_id = @eventId";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@eventId", eventId);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    result = MapRowToEvent(reader);
                }
                return result;
            }
            
        }

        /// <summary>
        /// Gets menu for an event by event ID
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        public List<EventMenuItem> GetMenuByEventId(int eventId)
        {
            List<EventMenuItem> result = new List<EventMenuItem>();
            const string sql = "SELECT item.item_id, item.item_description, item.item_name FROM item " +
                               "join event_item on event_item.item_id = item.item_id " +
                               "where event_id = @id;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", eventId);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(GetMenuFromReader(reader));
                }
                return result;
            }
        }
        
        public bool UpdateInfo(Event updatedEvent)
        {
            bool ok = false;
            const string Sql = "UPDATE event_table"+
            " SET event_name = @event_name, event_description = @event_description, event_date = @event_date "+
            "WHERE event_id = @event_id;";
            
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(Sql, conn);
                    cmd.Parameters.AddWithValue("@event_id", updatedEvent.Id);
                    cmd.Parameters.AddWithValue("@event_name", updatedEvent.Name);
                    cmd.Parameters.AddWithValue("@event_description", updatedEvent.Description);
                    cmd.Parameters.AddWithValue("@event_date", updatedEvent.Date);
                    cmd.ExecuteNonQuery();
                    ok = true;
                    return ok;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }


            
        }

        public bool UpdateGuests(GuestListVM invite)
        {
            bool ok = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM guest WHERE event_id = @event_id", conn);
                    cmd.Parameters.AddWithValue("@event_id", invite.eventId);
                    cmd.ExecuteNonQuery();
                    AddGuests(invite);
                    ok = true;
                    return ok;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }



        }

        public bool UpdateMenu(MenuVM newMenu)
        {
            bool ok = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM event_item WHERE event_id = @event_id", conn);
                    cmd.Parameters.AddWithValue("@event_id", newMenu.eventId);
                    cmd.ExecuteNonQuery();
                    AddItems(newMenu);
                    ok = true;
                    return ok;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }


        }

        private EventMenuItem GetMenuFromReader(SqlDataReader reader)
        {
            return new EventMenuItem()
            {
                Id = Convert.ToInt32(reader["item_id"]),
                Description = Convert.ToString(reader["item_description"]),
                Name = Convert.ToString(reader["item_name"]),
            };
        }

    }
}
