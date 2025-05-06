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
        private readonly IBidDao _bidDao;

        public AuctionDAO(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public Task<bool> DeleteAsync(Auction entity)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Auction>> GetAllActiveAsync()
        {
            const string sql = @"SELECT
                            AuctionId,
                            AuctionName,
                            Description,
                            StartPrice,
                            StartDate,
                            EndDate,
                            AmountOfBids,
                            AuctionStatus,
                            UserId
                        FROM dbo.Auction
                        WHERE AuctionStatus = ACTIVE";

            var auctions = await _dbConnection.QueryAsync<Auction>(sql);

            foreach(var auction in auctions)
            {
                auction.Bids = await _bidDao.GetAllByAuctionIdAsync(auction.AuctionID.Value);
            }

            return auctions;

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

        public async Task<bool> UpdateAuctionOptimistically(int auctionId, byte[] expectedVersion, IDbTransaction transaction = null, int newBids = 1)
        {
            const string sql = @"UPDATE Auction
                                 SET AmountOfBids = AmountOfBids + @AmountOfBids,
                                 WHERE AuctionId = @AuctionId
                                 AND Version = @ExpectedVersion";
            var parameters = new DynamicParameters();
            parameters.Add("AuctionId", auctionId);
            parameters.Add("AmountOfBids", newBids);
            parameters.Add("ExpectedVersion", expectedVersion);

            int rowsAffected = await _dbConnection.ExecuteAsync(sql, parameters, transaction: transaction);
            return rowsAffected > 0;
        }

        public async Task<bool> UpdateAuctionStatusAsync(int auctionId, AuctionStatus newStatus)
        {
            throw new NotImplementedException();
        }
    }

}

