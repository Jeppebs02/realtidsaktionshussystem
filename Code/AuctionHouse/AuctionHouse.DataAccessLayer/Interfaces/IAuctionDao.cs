using AuctionHouse.ClassLibrary.Enum;
using AuctionHouse.ClassLibrary.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionHouse.DataAccessLayer.Interfaces
{
    public interface IAuctionDao : IGenericDao<Auction>
    {
        Task<IEnumerable<Auction>> GetWithinDateRangeAsync(DateTime startDate, DateTime endDate);

        /// <summary>
        /// All auctions that a user owns/created (he is selling an item)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IEnumerable<Auction>> GetAllByUserIDAsync(int userId);

        /// <summary>
        /// All auctions that a given user has made a bid on.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IEnumerable<Auction>> GetAllByBidsAsync(int userId);

        /// <summary>
        /// Gets all auctions that has "ACTIVE" in AuctionStatus
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Auction>> GetAllActiveAsync();

        Task<bool> UpdateAuctionOptimistically(int auctionId, byte[] expectedVersion, IDbTransaction transaction = null, int newBids=1);

        Task<bool> UpdateAuctionStatusOptimisticallyAsync(int auctionId, byte[] expectedVersion, AuctionStatus newStatus, IDbTransaction transaction = null);
    }
}
