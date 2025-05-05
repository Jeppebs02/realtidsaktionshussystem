using AuctionHouse.ClassLibrary.Model;
using AuctionHouse.DataAccessLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionHouse.DataAccessLayer.DAO
{
    public class BidDAO : IBidDao
    {

        private readonly IDbConnection _dbConnection;

        public BidDAO(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public Task<bool> DeleteAsync(Bid entity)
        {
            throw new NotImplementedException();
        }

        public Task<List<T>> GetAllAsync<T>()
        {
            throw new NotImplementedException();
        }

        public Task<T?> GetByIdAsync<T>(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Bid> GetLatestByAuctionId(int auctionId)
        {
            throw new NotImplementedException();
        }

        public Task<int> InsertAsync(Bid entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(Bid entity)
        {
            throw new NotImplementedException();
        }
    }
}
