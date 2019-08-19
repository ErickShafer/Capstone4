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
    public interface IEventDAO
    {
        /// <summary>
        /// Retrieves an event from the system by event id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Event GetEvent(int id);

        /// <summary>
        /// Creates a new event.
        /// </summary>
        /// <param name="cookoutEvent"></param>
        int CreateEvent(Event cookoutEvent);

        /// <summary>
        /// Returns a List of all menu items in Database.
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        List<OrderItem> GetItems();

        /// <summary>
        /// Adds list of selected order items to an event id to create a Menu.
        /// </summary>
        /// <param name="Menu"></param>
        /// <param name="eventId"></param>
        void AddItems(MenuVM menu);

        /// <summary>
        /// Returns a list of all guests in Database.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<User> GetGuests(int eventId);

        /// <summary>
        /// Adds a list of selected users to an event id to create a GuestList.
        /// </summary>
        void AddGuests(GuestListVM invite);

        /// <summary>
        /// Return list of menu items by event ID
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        List<EventMenuItem> GetMenuByEventId(int eventId);

        /// <summary>
        /// Return event details by event id
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        Event GetEventByEventId(int eventId);

        /// <summary>
        /// After event is created, add the host of the party to the guest list.
        /// </summary>
        /// <param name="hostId"></param>
        /// <param name="eventId"></param>
        void AddHostasAGuest(int hostId, int eventId);

        bool UpdateMenu(MenuVM newMenu);

        bool UpdateGuests(GuestListVM invite);

        bool UpdateInfo(Event updatedEvent);
    }
}
