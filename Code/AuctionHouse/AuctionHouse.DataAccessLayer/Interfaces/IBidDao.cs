using AuctionHouse.ClassLibrary.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionHouse.DataAccessLayer.Interfaces
{
    public interface IBidDao : IGenericDao<Bid>
    {

        /// <summary>
        /// Gets the latest (And thus highest) bid on a given auction.
        /// </summary>
        /// <param name="auctionId"></param>
        /// <returns></returns>
        Task<Bid> GetLatestByAuctionId(int auctionId);

        Task<int> InsertBidAsync(Bid entity, IDbTransaction transaction = null);

        Task<List<Bid>> GetAllByAuctionIdAsync(int auctionId);


    }
}
