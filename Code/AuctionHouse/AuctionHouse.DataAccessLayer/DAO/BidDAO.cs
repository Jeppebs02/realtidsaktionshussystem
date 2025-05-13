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

        private readonly Func<IDbConnection> _connectionFactory;
        private readonly IUserDao _userDao;

        public BidDAO(Func<IDbConnection> connectionFactory, IUserDao userdao)
        {
            _connectionFactory = connectionFactory;
            _userDao = userdao;
        }

        public async Task<bool> DeleteAsync(Bid entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Bid>> GetAllAsync()
        {
            return null;
        }



        public async Task<Bid> GetByIdAsync(int id) // This one is called by AuctionDAO.GetByIdAsync's dependency chain
        {
            using var conn = _connectionFactory();
            const string sql = @"SELECT
                            b.BidId, b.AuctionId, b.Amount, b.TimeStamp, b.UserId,
                            -- User part
                            u.UserId AS User_UserId, u.UserName, u.FirstName, u.LastName, u.Email, u.PhoneNumber, u.Address, u.CantBuy, u.CantSell, u.RegistrationDate, u.IsDeleted, u.PasswordHash,
                            -- Wallet part
                            w.WalletId AS User_Wallet_WalletId, w.TotalBalance AS User_Wallet_TotalBalance,
                            w.ReservedBalance AS User_Wallet_ReservedBalance, w.UserId AS User_Wallet_UserId,
                            w.Version AS User_Wallet_Version
                        FROM dbo.Bid b
                        JOIN dbo.[User] u ON b.UserId = u.UserId
                        LEFT JOIN dbo.Wallet w ON u.UserId = w.UserId
                        WHERE b.BidId = @BidId;";

            var bidResult = await conn.QueryAsync<Bid, User, Wallet, Bid>(
                sql,
                (bid, user, wallet) =>
                {
                    bid.User = user;
                    if (bid.User != null)
                    {
                        bid.User.Wallet = wallet;
                        if (bid.User.Wallet != null)
                        {
                            bid.User.Wallet.Transactions = new List<Transaction>();
                        }
                    }
                    return bid;
                },
                new { BidId = id },
                splitOn: "User_UserId,User_Wallet_WalletId"
            );
            return bidResult.SingleOrDefault();
        }



        public async Task<Bid> GetLatestByAuctionId(int auctionId)
        {
            using var conn = _connectionFactory();
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

            var bids = await conn.QueryAsync<Bid, User, Bid>(
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
            var conn = _connectionFactory();

            //Make sure to use the same connection as the transaction if it is not null
            if (transaction != null)
            {
                conn = transaction.Connection;
            }
            const string sql = @"
                                INSERT INTO Bid (AuctionId, Amount, [TimeStamp], UserId)
                                VALUES (@AuctionId, @Amount, @TimeStamp, @UserId);
                                SELECT CAST(SCOPE_IDENTITY() as int);";


            var parameters = new
            {
                entity.AuctionId,
                entity.Amount,
                entity.TimeStamp,
                UserId = entity.User?.UserId
            };
            if (parameters.UserId == null) throw new ArgumentException("Bid must have a User with a userId.");


            return await conn.ExecuteScalarAsync<int>(sql, parameters, transaction: transaction);
        }

        public async Task<bool> UpdateAsync(Bid entity)
        {
            using var conn = _connectionFactory();
            const string sql = "UPDATE Bid SET AuctionId = @AuctionId, Amount = @Amount, TimeStamp = @TimeStamp WHERE BidId = @BidId;";
            var result = await conn.ExecuteAsync(sql, new { entity.AuctionId, entity.Amount, entity.TimeStamp, entity.BidId });
            return result > 0;
        }

        public async Task<List<Bid>> GetAllByAuctionIdAsync(int auctionId)
        {
            using var conn = _connectionFactory();
            const string sql = @"SELECT
                            b.BidId, b.Amount, b.TimeStamp, b.AuctionId, b.UserId,
                            -- User part
                            u.UserId AS User_UserId, u.UserName, u.FirstName, u.LastName, u.Email, u.PhoneNumber, u.Address, u.CantBuy, u.CantSell, u.RegistrationDate, u.IsDeleted, u.PasswordHash,
                            -- Wallet part
                            w.WalletId AS User_Wallet_WalletId, w.TotalBalance AS User_Wallet_TotalBalance,
                            w.ReservedBalance AS User_Wallet_ReservedBalance, w.UserId AS User_Wallet_UserId,
                            w.Version AS User_Wallet_Version
                        FROM dbo.Bid b
                        JOIN dbo.[User] u ON b.UserId = u.UserId
                        LEFT JOIN dbo.Wallet w ON u.UserId = w.UserId
                        WHERE b.AuctionId = @AuctionId
                        ORDER BY b.TimeStamp DESC;";

            var bids = await conn.QueryAsync<Bid, User, Wallet, Bid>(
                sql,
                (bid, user, wallet) =>
                {
                    bid.User = user;
                    if (bid.User != null)
                    {
                        bid.User.Wallet = wallet;
                        if (bid.User.Wallet != null)
                        {
                            bid.User.Wallet.Transactions = new List<Transaction>();
                        }
                    }
                    return bid;
                },
                new { AuctionId = auctionId },
                splitOn: "User_UserId,User_Wallet_WalletId"
            );
            return bids.ToList();
        }

        Task<int> IGenericDao<Bid>.InsertAsync(Bid entity)
        {
            throw new NotImplementedException("Use BidDAO.InsertBidAsync instead");
        }
    }
}
