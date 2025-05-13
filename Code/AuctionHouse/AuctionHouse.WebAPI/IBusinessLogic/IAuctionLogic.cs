using AuctionHouse.ClassLibrary.Enum;
using AuctionHouse.ClassLibrary.Model;
using System.Data;
namespace AuctionHouse.WebAPI.IBusinessLogic
{
    public interface IAuctionLogic
    {
        Task<List<T>> GetAllAuctionsAsync<T>();
        Task<IEnumerable<Auction>> GetAllActiveAuctionsAsync();

        Task<Auction> GetAuctionByIdAsync(int id, IDbTransaction transaction = null);

        Task<bool> CreateAuctionAsync(Auction auction);

        Task<bool> UpdateAuctionOptimistically(int auctionId, byte[] expectedVersion, IDbTransaction transaction=null, int newBids=1);

        Task<bool> UpdateAuctionStatusOptimistically(int auctionId, byte[] expectedVersion, AuctionStatus newStatus, IDbTransaction transaction = null);
    }
}
