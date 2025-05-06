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
    public class UserDaoTest
    {
        private readonly IUserDao _userDao;
        private readonly IDbConnection _connection;
        private readonly IWalletDao _walletDao;

        public UserDaoTest()
        {
            string connectionString = Environment.GetEnvironmentVariable("DatabaseConnectionString");

            _connection = new SqlConnection(connectionString);

            TransactionDAO transactionDAO = new TransactionDAO(_connection);
            _walletDao = new WalletDAO(_connection, transactionDAO);
            _userDao = new UserDAO(_connection, _walletDao);
            

        }

        [Fact]
        public async Task GetById_ShouldReturnUser_WhenUserIdExists()
        {
            // Arrange  
            int userId = 1;
            // Act  
            User user = await _userDao.GetByIdAsync<User>(userId);
            // Assert  
            Assert.NotNull(user);
            Assert.Equal(userId, user.UserId); // Assuming User has a userId property  
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnTrue_WhenUserIsUpdated()
        {
            // Arrange  
            User user = await _userDao.GetByIdAsync<User>(1);   // Assuming user with ID 1 exists
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
            User user = await _userDao.GetByIdAsync<User>(1);   // Assuming user with ID 1 exists

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
            List<User> users = await _userDao.GetAllAsync<User>();

            // Assert  
            Assert.NotNull(users);
            Assert.True(users.Count > 0);
        }

    }
}
