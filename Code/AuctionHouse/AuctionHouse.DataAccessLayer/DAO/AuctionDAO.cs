using AuctionHouse.DataAccessLayer.Interfaces;
using AuctionHouse.ClassLibrary.Model;
using AuctionHouse.ClassLibrary.Enum;
using System;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionHouse.DataAccessLayer.DAO
{
    public class AuctionDAO : IAuctionDao
    {

        private readonly IDbConnection _dbConnection;

        public AuctionDAO(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public Task<bool> DeleteAsync(Auction entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Auction>> GetAllActiveAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<T>> GetAllAsync<T>()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Auction>> GetAllByBidsAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Auction>> GetAllByUserIDAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<T?> GetByIdAsync<T>(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Auction>> GetWithinDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public Task<int> InsertAsync(Auction entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(Auction entity)
        {
            throw new NotImplementedException();
        }
    }

}

