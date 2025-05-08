using AuctionHouse.ClassLibrary.Enum;
using AuctionHouse.ClassLibrary.Model;
using AuctionHouse.WebAPI.IBusinessLogic;
using System.Data;

namespace AuctionHouse.WebAPI.BusinessLogic
{
    public class AuctionLogic : IAuctionLogic
    {
        public Task<bool> CreateAuctionAsync(Auction auction)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Auction>> GetAllActiveAuctionsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<T>> GetAllAuctionsAsync<T>()
        {
            throw new NotImplementedException();
        }

        public Task<Auction> GetAuctionByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateAuctionOptimistically(int auctionId, byte[] expectedVersion, IDbTransaction transaction = null, int newBids = 1)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAuctionStatusOptimistically(int auctionId, byte[] expectedVersion, AuctionStatus newStatus, IDbTransaction transaction = null)
        {
            throw new NotImplementedException();
        }
    }
}
