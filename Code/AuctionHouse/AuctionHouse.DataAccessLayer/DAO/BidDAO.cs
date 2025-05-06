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

        public async Task<bool> DeleteAsync(Bid entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<T>> GetAllAsync<T>()
        {
            return null;
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

            var bids = await _dbConnection.QueryAsync<T, User, T>(
                sql,
                map: (bidT, user) =>
                {
                    if (bidT != null)
                    {
                        //Runtime cast
                        if (bidT is Bid concreteBid)
                        {
                            concreteBid.User = user;
                        }

                    }
                    return bidT;
                },
                param: new { BidId = id },

                splitOn: "User_UserId"
            );

            return bids.FirstOrDefault();

        }

        public async Task<Bid> GetLatestByAuctionId(int auctionId)
        {
            
            const string sql = @"SELECT TOP 1
                            b.BidId, b.Amount, b.TimeStamp, b.UserId AS BidUserId, b.AuctionId,
                            u.UserId AS User_UserId,
                            u.CantBuy, u.CantSell, u.UserName, u.PasswordHash,
                            u.RegistrationDate, u.FirstName, u.LastName, u.Email,
                            u.PhoneNumber, u.[Address], u.IsDeleted
                        FROM dbo.Bid b
                        JOIN dbo.[User] u ON b.UserId = u.UserId
                        WHERE b.AuctionId = @AuctionId
                        ORDER BY b.Amount DESC, b.TimeStamp DESC;";

            var bids = await _dbConnection.QueryAsync<Bid, User, Bid>(
                sql,
                map: (bid, user) =>
                {
                    // add the user to the bid, this is similar to the static map function in ItemDAO.
                    //This is just with a lambda :)
                    bid.User = user;
                    return bid;
                },
                param: new { AuctionId = auctionId },
                
                splitOn: "User_UserId"
            );

            return bids.FirstOrDefault();
        }

        // Since bid is where our concurrency lies, we need to add an IDbTransaction.
        public async Task<int> InsertBidAsync(Bid entity, IDbTransaction transaction = null)
        {
            const string sql = @"
                                INSERT INTO Bid (AuctionId, Amount, TimeStamp, UserId)
                                VALUES (@AuctionId, @Amount, @TimeStamp, @UserId);
                                SELECT CAST(SCOPE_IDENTITY() as int);";

           
            var parameters = new
            {
                entity.AuctionId,
                entity.Amount,
                entity.TimeStamp,
                UserId = entity.User?.userId
            };
            if (parameters.UserId == null) throw new ArgumentException("Bid must have a User with a userId.");


            return await _dbConnection.ExecuteScalarAsync<int>(sql, parameters, transaction: transaction);
        }

        public async Task<bool> UpdateAsync(Bid entity)
        {
            const string sql = "UPDATE Bid SET AuctionId = @AuctionId, Amount = @Amount, TimeStamp = @TimeStamp WHERE BidId = @BidId;";
            var result = await _dbConnection.ExecuteAsync(sql, new { entity.AuctionId, entity.Amount, entity.TimeStamp, entity.BidId });
            return result > 0;
        }

        Task<int> IGenericDao<Bid>.InsertAsync(Bid entity)
        {
            throw new NotImplementedException("Use BidDAO.InsertBidAsync instead");
        }
    }
}
