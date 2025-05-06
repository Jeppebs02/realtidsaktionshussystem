using AuctionHouse.ClassLibrary.Model;
using AuctionHouse.WebAPI.IBusinessLogic;

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

        public Task<bool> UpdateAuctionOptimistically(Auction auction)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAuctionStatusOptimistically(Auction auction)
        {
            throw new NotImplementedException();
        }
    }
}
