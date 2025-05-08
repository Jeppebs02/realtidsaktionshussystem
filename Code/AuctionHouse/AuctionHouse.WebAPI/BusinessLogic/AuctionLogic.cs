using AuctionHouse.ClassLibrary.Enum;
using AuctionHouse.ClassLibrary.Model;
using AuctionHouse.DataAccessLayer.Interfaces;
using AuctionHouse.WebAPI.IBusinessLogic;
using System.Data;

namespace AuctionHouse.WebAPI.BusinessLogic
{
    public class AuctionLogic : IAuctionLogic
    {
        private readonly Func<IDbConnection> _connectionFactory;
        private readonly IAuctionDao _auctionDao;

        public AuctionLogic(Func<IDbConnection> connectionFactory, IAuctionDao auctionDao)
        {
            _connectionFactory = connectionFactory;
            _auctionDao = auctionDao;
        }
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
            return await _auctionDao.UpdateAuctionOptimistically(auctionId, expectedVersion, transaction, newBids);
        }

        public Task<bool> UpdateAuctionStatusOptimistically(int auctionId, byte[] expectedVersion, AuctionStatus newStatus, IDbTransaction transaction = null)
        {
            throw new NotImplementedException();
        }
    }
}
