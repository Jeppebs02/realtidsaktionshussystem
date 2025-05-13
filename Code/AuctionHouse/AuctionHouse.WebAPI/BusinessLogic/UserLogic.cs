using AuctionHouse.ClassLibrary.Model;
using AuctionHouse.DataAccessLayer.Interfaces;
using AuctionHouse.WebAPI.IBusinessLogic;

namespace AuctionHouse.WebAPI.BusinessLogic
{
    public class UserLogic : IUserLogic
    {

        private readonly IUserDao _userDao;

        public UserLogic(IUserDao userDao)
        {
            _userDao = userDao;
        }

        public async Task<int> CreateUserAsync(User user)
        {
            return await _userDao.InsertAsync(user);
        }

        public async Task<bool> DeleteUserAsync(User user)
        {
            return await _userDao.DeleteAsync(user);
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _userDao.GetAllAsync();
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _userDao.GetByIdAsync(userId);
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            return await _userDao.UpdateAsync(user);
        }
    }
}
