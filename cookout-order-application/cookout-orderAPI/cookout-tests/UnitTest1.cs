using cookout_orderAPI.DAL;
using cookout_orderAPI.Models;
using cookout_orderAPI.Providers.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Transactions;

namespace cookout_tests
{
    [TestClass]
    public class UnitTest1
    {
        private TransactionScope _tran = null;
        private IUserDAO _db = null;
        private IPasswordHasher passwordHasher;
        [TestInitialize]
        public void Initialize()
        {
            _db = new UserSqlDAO("Data Source=.\\SQLEXPRESS;Initial Catalog=cookoutDatabase;Integrated Security=True");
            _tran = new TransactionScope();
            passwordHasher = new PasswordHasher();

        }

        [TestCleanup]
        public void Cleanup()
        {
            _tran.Dispose();
        }

        [TestMethod]
        public void TestCreateUser()
        {
            int beforeInsert = _db.GetUsers().Count;
            User test = new User();
            test.Username = "testUser";
            test.Email = "test@email.com";
            test.Password = "testPassword";
            test.Salt = "testSalt";
            test.Role = "1";
            int newUserId = _db.CreateUser(test);
            User newUser = _db.GetUser(newUserId);
            int afterInsert = _db.GetUsers().Count;

            Assert.AreEqual(1, (afterInsert - beforeInsert), "The CreateUser method did not Insert a New User");
            Assert.AreEqual("testUser", newUser.Username, "The Create Method did not instert a Name");
            Assert.AreEqual("test@email.com", newUser.Email, "The Create Method did not instert an Email");
            Assert.AreEqual("testPassword", newUser.Password, "The Create Method did not instert a Password");
            Assert.AreEqual("1", newUser.Role, "The Create Method did not instert a Role");
        }

        [TestMethod]
        public void TestDeleteUser()
        {

            User test = new User();
            test.Username = "testUser";
            test.Email = "test@email.com";
            test.Password = "testPassword";
            test.Salt = "testSalt";
            test.Role = "1";
            int newUserId = _db.CreateUser(test);
            int beforeDelete = _db.GetUsers().Count;
            User newUser = _db.GetUser(newUserId);
            _db.DeleteUser(newUser);
            User confirmUser = _db.GetUser(newUserId);
            int afterDelete = _db.GetUsers().Count;

            Assert.AreEqual(1, (beforeDelete - afterDelete), "The DeleteUser method did not delete a User");
            Assert.IsNull(confirmUser.Username, "The DeleteUser method did not delete the CORRECT User");
        }

        [TestMethod]
        public void UpdateUser()
        {
            User test = new User();
            test.Username = "testUser";
            test.Email = "test@email.com";
            test.Password = "testPassword";
            test.Salt = "testSalt";
            test.Role = "1";
            int newUserId = _db.CreateUser(test);
            User newUser = _db.GetUser(newUserId);
            User testUpdateUser = newUser;
            
            User testUpdate = _db.GetUser(testUpdateUser.Id);
            Assert.AreEqual("testUser", testUpdate.Username);
            Assert.AreEqual("1", testUpdate.Role);
        }

        [TestMethod]
        public void TestGetUserByEmail()
        {
            User test = new User();
            test.Username = "testUser";
            test.Email = "test@email.com";
            test.Password = "testPassword";
            test.Salt = "testSalt";
            test.Role = "1";
            int newUserId = _db.CreateUser(test);
            User newUser = _db.GetUser(newUserId);
            User validateUser = _db.GetUserByEmail(newUser.Email);
            Assert.AreEqual(newUser.Id, validateUser.Id, "The user does not match");
        }

        [TestMethod]
        public void TestCreateEvent()
        {

        }

        [TestMethod]
        public void TestGetEventByUser()
        {
            List<Event> testList = _db.GetEventByUser("testUser");

            Assert.AreEqual(1, testList.Count);

        }

        //[TestMethod]
        //public void TestCreateOrder()
        //{
        //    OrderScreen test = new OrderScreen();
        //    test.OrderId = 11;
        //    test.ItemName.Add("testItem");
        //    test.Count.Add(1);
        //    test.Complete = false;

        //}

        [TestMethod]
        public void Test()
        {

        }

    }
}
