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
        private readonly IItemDao _itemDao;

        public AuctionLogic(Func<IDbConnection> connectionFactory, IAuctionDao auctionDao, IItemDao itemDao)
        {
            _connectionFactory = connectionFactory;
            _auctionDao = auctionDao;
            _itemDao = itemDao;
        }
        public async Task<bool> CreateAuctionAsync(Auction auction)
        {
            int itemId = await _itemDao.InsertAsync(auction.item);
            Console.WriteLine($"Inserted Item ID: {itemId}");

            if (itemId == 0)
            {
                Console.WriteLine("Item insert failed.");
                return false;
            }

            auction.item.ItemId = itemId;

            int auctionId = await _auctionDao.InsertAsync(auction);
            Console.WriteLine($"Inserted Auction ID: {auctionId}");

            return auctionId != 0;
        }

        public Task<IEnumerable<Auction>> GetAllActiveAuctionsAsync()
        {
            return _auctionDao.GetAllActiveAsync();
        }

        public Task<List<T>> GetAllAuctionsAsync<T>()
        {
            throw new NotImplementedException();
        }

        public Task<Auction> GetAuctionByIdAsync(int id, IDbTransaction transaction)
        {
            return _auctionDao.GetByIdAsync(id, transaction);
        }

        public async Task<bool> UpdateAuctionOptimistically(int auctionId, byte[] expectedVersion, IDbTransaction transaction = null, int newBids = 1)
        {
            return await _auctionDao.UpdateAuctionOptimistically(auctionId, expectedVersion, transaction, newBids);
        }

        public async Task<bool> UpdateAuctionStatusOptimistically(int auctionId, byte[] expectedVersion, AuctionStatus newStatus, IDbTransaction transaction = null)
        {
            return await _auctionDao.UpdateAuctionStatusOptimisticallyAsync(auctionId, expectedVersion, newStatus, transaction);
        }
    }
}
