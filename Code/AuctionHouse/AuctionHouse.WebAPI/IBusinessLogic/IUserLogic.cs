using AuctionHouse.ClassLibrary.Model;
namespace AuctionHouse.WebAPI.IBusinessLogic
{
    public interface IUserLogic
    {
        Task<bool> CreateUserAsync(User user);
        Task<bool> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(int userId);
        Task<User> GetUserByIdAsync(int userId);
        Task<List<User>> GetAllUsersAsync();
        Task<bool> UserExistsAsync(string username);
    }
}
