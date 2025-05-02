using AuctionHouse.ClassLibrary.Model;
using AuctionHouse.DataAccessLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionHouse.Test.DaoTests
{
    public class UserDaoTest
    {
        private readonly IUserDao _userDao;

        [Fact]
        public async Task GetById_ShouldReturnUser_WhenUserIdExists()
        {
            // Arrange  
            int userId = 1;
            // Act  
            User user = await _userDao.GetByIdAsync<User>(userId);
            // Assert  
            Assert.NotNull(user);
            Assert.Equal(userId, user.userId); // Assuming User has a userId property  
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnTrue_WhenUserIsUpdated()
        {
            // Arrange  
            User user = await _userDao.GetByIdAsync<User>(1);   // Assuming user with ID 1 exists
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
            User user = new User("carlCool", "123", "carl", "carlsen", "hej@.com", "12345678", "carl street", new Wallet(100, 0, 0));

            // Act
            int id = await _userDao.InsertAsync(user);

            // Assert
            Assert.True(id > 0);
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
