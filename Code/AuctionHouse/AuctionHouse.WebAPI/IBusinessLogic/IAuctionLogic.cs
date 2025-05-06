using AuctionHouse.ClassLibrary.Model;
namespace AuctionHouse.WebAPI.IBusinessLogic
{
    public interface IAuctionLogic
    {
        Task<List<T>> GetAllAuctionsAsync<T>();
        Task<IEnumerable<Auction>> GetAllActiveAuctionsAsync();

        Task<bool> CreateAuctionAsync(Auction auction);
        Task<bool> UpdateAuctionAsync(Auction auction);
    }
}
