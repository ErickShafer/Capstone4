using cookout_orderAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cookout_orderAPI.DAL
{
    /// <summary>
    /// An interface for user data objects.
    /// </summary>
    public interface IUserDAO
    {
        /// <summary>
        /// Retrieves a user from the system by username.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        User GetUser(int id);

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="user"></param>
        int CreateUser(User user);

        /// <summary>
        /// Deletes a user from the system.
        /// </summary>
        /// <param name="user"></param>
        void DeleteUser(User user);

        /// <summary>
        /// Gets all events associated with a user ID where user is a guest
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<Event> GetEventByUser(string userName);

        /// <summary>
        /// Returns a List of each user in Database
        /// </summary>
        /// <returns></returns>
        List<User> GetUsers();

        /// <summary>
        /// Returns A User with a given Email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        User GetUserByEmail(string email);

        /// <summary>
        /// Returns all incomplete orders by eventId
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        List<OrderScreen> GetOrdersByEventID(int eventId);

        int TestMakingEvent();

        int AddHost(int HostId, int eventId);

        /// <summary>
        /// Gets all info of a single user by username
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        User GetUserByUsername(string name);

        /// <summary>
        /// List of events where user is host
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        List<Event> GetHostEventByHostId(int id, string name);

        List<Event> GetEventByHostId(int id);

        /// <summary>
        /// Updates user role by user ID
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newRoleId"></param>
        void UpdateUser(int userId, int newRoleId);

        List<int> GetOrderIdsByEventId(int eventId);

        EventGuestVM GetGuestsByEventId(int eventId);

    }
}
