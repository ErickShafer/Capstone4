using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cookout_orderAPI.DAL;
using cookout_orderAPI.Models;
using cookout_orderAPI.Models.Account;
using cookout_orderAPI.Providers.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace cookout_orderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private ITokenGenerator tokenGenerator;
        private IPasswordHasher passwordHasher;
        private IUserDAO userDao;
        private IEventDAO eventDAO;
        private IOrderDAO _orderDAO;

        /// <summary>
        /// Creates a new account controller.
        /// </summary>
        /// <param name="tokenGenerator">A token generator used when creating auth tokens.</param>
        /// <param name="passwordHasher">A password hasher used when hashing passwords.</param>
        /// <param name="userDao">A data access object to store user data.</param>
        public LoginController(ITokenGenerator tokenGenerator, IPasswordHasher passwordHasher, IUserDAO userDao, IEventDAO eventDAO, IOrderDAO orderDAO)
        {
            this.tokenGenerator = tokenGenerator;
            this.passwordHasher = passwordHasher;
            this.userDao = userDao;
            this.eventDAO = eventDAO;
            _orderDAO = orderDAO;
        }

        /// <summary>
        /// Registers a user and provides a bearer token.
        /// </summary>
        /// <param name="newUser"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public IActionResult Register(RegisterForm newUser)
        {
            var testUser = userDao.GetUserByEmail(newUser.Email);
            // Does user already exist
            if (testUser.Username != null)
            {
                return BadRequest(new
                {
                    Message = "Username has already been taken."
                });
            }

            // Generate a password hash
            var passwordHash = passwordHasher.ComputeHash(newUser.Password);

            // Create a user object
            var user = new User { Password = passwordHash.Password, Salt = passwordHash.Salt, Role = "1", Username = newUser.Username, Email = newUser.Email };

            // Save the user
            userDao.CreateUser(user);

            // Generate a token
            var token = tokenGenerator.GenerateToken(user.Username, user.Role);

            // Return the token
            return Ok(token);
        }


        /// <summary>
        /// Authenticates the user and provides a bearer token.
        /// </summary>
        /// <param name="model">An object including the user's credentials.</param> 
        /// <returns></returns>
        [HttpPost("login")]
        public IActionResult Login(AuthenticationModel model)
        {
            // Assume the user is not authorized
            IActionResult result = Unauthorized();

            // Get the user by username
            var user = userDao.GetUserByEmail(model.Email);

            // If we found a user and the password has matches
            if (user != null && passwordHasher.VerifyHashMatch(user.Password, model.Password, user.Salt))
            {
                // Create an authentication token
                var token = tokenGenerator.GenerateToken(user.Username, user.Role);

                // Switch to 200 OK
                result = Ok(token);
            }

            return result;
        }

        /// <summary>
        /// Gets complete user info for single user by username
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("userinfo")]
        public ActionResult<User> GetUser(string name)
        {
            User oneUser = userDao.GetUserByUsername(name);
            return oneUser;
        }

        /// <summary>
        /// Gets a List of all items in database to populate menu creation form
        /// </summary>
        /// <returns></returns>
        [HttpGet("items")]
        public ActionResult<List<OrderItem>> GetItems()
        {
            List<OrderItem> items = eventDAO.GetItems();
            return items;
        }

        [HttpGet("guests")]
        public ActionResult<List<User>> GetGuests()
        {
            List<User> guests = userDao.GetUsers();
            return guests;
        }

        /// <summary>
        /// Adds List of Users to Guestlist of given event ID
        /// </summary>
        /// <param name="invites">Object containing list of guests and event id.</param>
        /// <returns></returns>
        [HttpPost("invite")]
        public ActionResult<int> AddGuests(GuestListVM invites)
        {
            eventDAO.AddGuests(invites);
            return invites.guestIds.Count;
        }

        /// <summary>
        /// returns list of events where user is a guest
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpGet("home")]
        public ActionResult<List<Event>> GetEvents(string userName)
        {
            return userDao.GetEventByUser(userName);
        }


        /// <summary>
        /// adds a menu to for an event
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        [HttpPost("menu")]
        public ActionResult<int> AddMenu(MenuVM menu)
        {
            eventDAO.AddItems(menu);
            return 0;
        }

        /// <summary>
        /// returns list of events where user is a host
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("hostevents")]
        public ActionResult<List<HostHomeVM>> GetHostEvents(int id, string name)
        {
            var hostEvents = userDao.GetHostEventByHostId(id, name);
            List<HostHomeVM> vmList = new List<HostHomeVM>();
            foreach (Event party in hostEvents)
            {
                HostHomeVM vm = new HostHomeVM();
                vm.Name = party.Name;
                vm.HostId = party.HostId;
                vm.Description = party.Description;
                vm.Date = party.Date;
                vm.Id = party.Id;
                if (party.HostId == id)
                {
                    vm.Role = "Host";
                }
                else
                {
                    vm.Role = "Guest";
                }
                vmList.Add(vm);
            }
            return vmList;
        }


        [HttpGet("findId")]
        public int GetUserId(string userName)
        {
            int id = userDao.GetUserByUsername(userName).Id;
            return id;
        }


        // POST create event
        [HttpPost("create")]
        public ActionResult<int> CreateEvent(Event newEvent)
        {
            int newEventId = eventDAO.CreateEvent(newEvent);
            eventDAO.AddHostasAGuest(newEvent.HostId, newEventId);
            return newEventId;
        }

        [HttpPost("updateMenu")]
        public bool UpdateMenu(MenuVM menu)
        {
            return eventDAO.UpdateMenu(menu);
        }

        [HttpPost("updateGuests")]
        public bool UpdateGuests(GuestListVM invite)
        {
            return eventDAO.UpdateGuests(invite);
        }

        [HttpPost("updateInfo")]
        public bool UpdateInfo(Event uEvent)
        {
            return eventDAO.UpdateInfo(uEvent);
        }

        [HttpGet("findName")]
        public string FindName(int id)
        {
            Event Result = eventDAO.GetEventByEventId(id);
            return Result.Name;
        }

        [HttpPut("updateRole")]
        public IActionResult UpdateRole(int userId, int newRoleId)
        {
            IActionResult result = Unauthorized();
            userDao.UpdateUser(userId, newRoleId);
            var updatedUser = userDao.GetUser(userId);
            if (updatedUser.Role == newRoleId.ToString())
            {
                var token = tokenGenerator.GenerateToken(updatedUser.Username, updatedUser.Role);
                result = Ok(token);
            }
            return result;
        }

        /// <summary>
        /// Returns all information of a single event by event ID including menu
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("getAEvent")]
        public ActionResult<EventVM> GetEventById(int id)
        {
            EventVM vm = new EventVM();
            var aEvent = eventDAO.GetEventByEventId(id);
            var menu = eventDAO.GetMenuByEventId(id);
            vm.Name = aEvent.Name;
            vm.Description = aEvent.Description;
            vm.HostId = aEvent.HostId;
            vm.Id = id;
            foreach (EventMenuItem item in menu)
            {
                vm.Menu.Add(item);
            }
            return vm;

        }
        /// <summary>
        /// Submits order to database.
        /// </summary>
        /// <param name="userOrder"></param>
        /// <returns></returns>
        [HttpPost("postOrder")]
        public IActionResult postOrder(EventVM userOrder)
        {
            IActionResult result = Unauthorized();
            Order dbOrder = new Order();
            dbOrder.EventId = userOrder.Id;
            dbOrder.OrderSubmitter = userOrder.UserId;
            foreach (EventMenuItem item in userOrder.Menu)
            {
                if (item.ItemQty > 0)
                {
                    OrderItem menuItem = new OrderItem();
                    menuItem.ItemID = item.Id;
                    menuItem.ItemQty = item.ItemQty;
                    dbOrder.OrderItems.Add(menuItem);
                }
            }
            int newOrderId = _orderDAO.createOrder(dbOrder);
            if (newOrderId != 0)
            {
                foreach (OrderItem item in dbOrder.OrderItems)
                {
                    _orderDAO.PopulateOrder_Items(newOrderId, item.ItemID, item.ItemQty);
                }
                result = Ok();
            }

            return result;
        }

        [HttpGet("myEvents")]
        public ActionResult<List<Event>> GetHostedEvents(int id)
        {
            List<Event> result = new List<Event>();
            result = userDao.GetEventByHostId(id);
            return result;
        }

        [HttpGet("getOrders")]
        public ActionResult<List<OrderScreen>> GetOrders(int eventID)
        {
            return userDao.GetOrdersByEventID(eventID);
        }

        [HttpGet("getOrderids")]
        public ActionResult<List<int>> GetOrderIds(int eventID)
        {
            return userDao.GetOrderIdsByEventId(eventID);
        }

        [HttpGet("getOrderItems")]
        public ActionResult<List<List<OrderItem>>> GetOrderItems(List<int> orderIds)
        {
            return _orderDAO.GetOrderItems(orderIds);

        }

        [HttpGet("getGuests")]
        public ActionResult<EventGuestVM> GetGuests(int eventID)
        {
            return userDao.GetGuestsByEventId(eventID);

        }
        /// <summary>
        /// Updates order to mark it complete by Order ID
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpPut("markComplete")]
        public IActionResult MarkComplete(int orderId)
        {
            IActionResult result = Unauthorized();
            _orderDAO.completeOrder(orderId);
            var ordercheck = _orderDAO.getOrder(orderId);
            if (ordercheck.IsCompleted)
            {
                result = Ok();
            }
            return result;
        }
    }

}

    
