using AuctionHouse.ClassLibrary.Model;
namespace AuctionHouse.WebAPI.IBusinessLogic
{
    public interface IUserLogic
    {
        Task<int> CreateUserAsync(User user);
        Task<bool> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(User user);
        Task<User> GetUserByIdAsync(int userId);
        Task<List<User>> GetAllUsersAsync();
    }
}
