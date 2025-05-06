using AuctionHouse.ClassLibrary.Model;
using AuctionHouse.ClassLibrary.Enum;
namespace AuctionHouse.WebAPI.IBusinessLogic
{
    public interface IItemLogic
    {
        Task<bool> CreateItemAsync(Item item);
        //Task<bool> UpdateItemAsync(Item item);
        //Task<bool> DeleteItemAsync(Item item);
        //Task<Item> GetItemByIdAsync(int itemId);
        //Task<List<Item>> GetAllItemsAsync();
        //Task<List<Item>> GetItemsByCategoryAsync(Category category);
        //Task<List<Item>> GetItemsByAuctionIdAsync(int auctionId);
        //Task<List<Item>> GetItemsByUserIdAsync(int userId);
        //Task<List<Item>> GetItemsByStatusAsync(ItemStatus status);
    }
}
