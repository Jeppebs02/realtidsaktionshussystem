using AuctionHouse.ClassLibrary.Model;
using AuctionHouse.DataAccessLayer.Interfaces;
using Dapper;
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

        public async Task<List<T>> GetAllAsync<T>()
        {
            const string sql = @"SELECT
                                b.BidId,
                                b.AuctionId,
                                b.Amount,
                                b.TimeStamp,
                                b.UserId        AS Bid_UserId,
                                u.UserId        AS User_UserId,
                                u.UserName,
                                u.FirstName,
                                u.LastName,
                                u.Email,
                                u.PhoneNumber,
                                u.[Address]     
                            FROM dbo.Bid       b
                            JOIN dbo.[User]  u
                                ON b.UserId = u.UserId";

            var bids = await _dbConnection.QueryAsync<T>(sql);

            return bids.ToList();
        }

         

        public async Task<T?> GetByIdAsync<T>(int id)
        {
            const string sql = @"SELECT
                                b.BidId,
                                b.AuctionId,
                                b.Amount,
                                b.TimeStamp,
                                b.UserId        AS Bid_UserId,
                                u.UserId        AS User_UserId,
                                u.UserName,
                                u.FirstName,
                                u.LastName,
                                u.Email,
                                u.PhoneNumber,
                                u.[Address]     
                            FROM dbo.Bid       b
                            JOIN dbo.[User]  u
                                ON b.UserId = u.UserId
                            WHERE b.BidId = @BidId;";

            var bid = await _dbConnection.QuerySingleOrDefaultAsync<T>(sql, new { BidId = id });

            return bid;
        }

        public async Task<Bid> GetLatestByAuctionId(int auctionId)
        {
            const string sql = "SELECT * FROM Bid WHERE AuctionId = @AuctionId ORDER BY Amount DESC LIMIT 1";

            var bid = await _dbConnection.QuerySingleOrDefaultAsync<Bid>(sql, new { AuctionId = auctionId });

            return bid;
        }

        public async Task<int> InsertAsync(Bid entity)
        {
            const string sql = "INSERT INTO Bid (AuctionId, Amount, TimeStamp, UserId) VALUES (@AuctionId, @Amount, @TimeStamp, @UserId);" +
                "SELECT CAST(SCOPE_IDENTITY() as int);";

            //TODO

            return 0;
        }

        public Task<bool> UpdateAsync(Bid entity)
        {
            throw new NotImplementedException();
        }
    }
}
