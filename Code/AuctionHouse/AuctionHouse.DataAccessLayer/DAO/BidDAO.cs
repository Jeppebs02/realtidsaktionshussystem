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
                                b.UserId AS Bid_UserId,
                                u.UserId AS User_UserId,
                                u.UserName,
                                u.FirstName,
                                u.LastName,
                                u.Email,
                                u.PhoneNumber,
                                u.Address
                            FROM dbo.Bid b
                            JOIN dbo.[User] u ON b.UserId = u.UserId;
                            ";

            // Await the result of QueryAsync and then convert it to a list

            var bids = (await _dbConnection.QueryAsync<T, User, T>
                (sql, (bid, user) =>
                {
                    // Assuming T is Bid
                    if (bid is Bid b)
                    {
                        b.User = user;
                    }
                    return bid;
                }, splitOn: "Bid_UserId")).ToList();

            return bids;
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
        }

        public Task<bool> UpdateAsync(Bid entity)
        {
            throw new NotImplementedException();
        }
    }
}
