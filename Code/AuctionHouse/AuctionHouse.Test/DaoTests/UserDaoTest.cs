using AuctionHouse.ClassLibrary.Model;
using AuctionHouse.DataAccessLayer.Interfaces;
using AuctionHouse.DataAccessLayer.DAO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionHouse.Test.DaoTests
{

    [Collection("Sequential")]
    public class UserDaoTest
    {
        #region Fields
        private readonly IUserDao _userDao;
        private readonly IDbConnection _connection;
        private readonly IWalletDao _walletDao;
        #endregion

        #region Constructor
        public UserDaoTest()
        {
            string connectionString = Environment.GetEnvironmentVariable("DatabaseConnectionString");

            _connection = new SqlConnection(connectionString);

            TransactionDAO transactionDAO = new TransactionDAO(_connection);
            _walletDao = new WalletDAO(_connection, transactionDAO);
            _userDao = new UserDAO(_connection, _walletDao);

            //Clean tables
            CleanAndBuild.CleanDB();
            //Generate test data
            CleanAndBuild.GenerateFreshTestDB();
        }
        #endregion

        #region Build up and tear down methods
        // Clean up after each test
        public void Dispose()
        {
            CleanAndBuild.CleanDB();
        }
        #endregion

        #region Test
        [Fact]
        public async Task GetById_ShouldReturnUser_WhenUserIdExists()
        {
            // Arrange  
            int userId = 1;
            // Act  
            User user = await _userDao.GetByIdAsync(userId);
            // Assert  
            Assert.NotNull(user);
            Assert.Equal(userId, user.UserId); // Assuming User has a userId property  
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnTrue_WhenUserIsUpdated()
        {
            // Arrange  
            User user = await _userDao.GetByIdAsync(1);   // Assuming user with ID 1 exists
            Console.WriteLine(user.Password);
            user.FirstName = "Zac"; // Update the user's name

            // Act  
            bool result = await _userDao.UpdateAsync(user);

            // Assert  
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnTrue_WhenUserIsDeleted()
        {
            // Arrange  
            User user = await _userDao.GetByIdAsync(1);   // Assuming user with ID 1 exists

            // Act  
            bool result = await _userDao.DeleteAsync(user);

            // Assert  
            Assert.True(result);
        }

        [Fact]
        public async Task InsertAsync_ShouldReturnId_WhenUserIsInserted()
        {
            // Arrange
            User user = new User("carlCool", "$2a$12$QG55SQJOa36iv/Efl6RDiuXTBNUYLKorvZ.M/xVuHZbKLhCZrDaSm", "carl", "carlsen", "hej@.com", "12345678", "carl street", null);

            // Act
            int UserId = await _userDao.InsertAsync(user);
            Wallet wallet = await _walletDao.GetByUserId(UserId);
            // Assert
            Assert.Equal(UserId, wallet.UserId);
        }

        [Fact]
        public async Task GetAll_ShouldReturnListOfUsers_WhenUsersExist()
        {
            // Act  
            List<User> users = await _userDao.GetAllAsync();

            // Assert  
            Assert.NotNull(users);
            Assert.True(users.Count > 0);
        }
        #endregion
    }
}
