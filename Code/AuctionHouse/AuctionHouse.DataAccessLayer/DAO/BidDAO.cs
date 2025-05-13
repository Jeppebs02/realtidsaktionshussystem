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
            const string sqlBid = "SELECT BidId, Amount, TimeStamp, UserId, AuctionId FROM dbo.Bid WHERE BidId = @BidId;";
            Bid bid = await conn.QuerySingleOrDefaultAsync<Bid>(sqlBid, new { BidId = id });

            if (bid != null && bid.UserId.HasValue)
            {
                const string sqlUserWallet = @"
                     SELECT u.UserId, u.CantBuy, u.CantSell, u.UserName, u.PasswordHash, u.RegistrationDate, u.FirstName, u.LastName, u.Email, u.PhoneNumber, u.Address, u.IsDeleted,
                            w.WalletId AS Wallet_WalletId, w.TotalBalance AS Wallet_TotalBalance, w.ReservedBalance AS Wallet_ReservedBalance, w.UserId AS Wallet_UserId, w.Version AS Wallet_Version
                     FROM dbo.[User] u
                     LEFT JOIN dbo.Wallet w ON u.UserId = w.UserId
                     WHERE u.UserId = @UserId;";

                // Using the multi-mapping that failed for UserDAO directly
                var userWithWallet = (await conn.QueryAsync<User, Wallet, User>(
                    sqlUserWallet,
                    (usr, wlt) => {
                        if (wlt != null && wlt.WalletId.HasValue)
                        {
                            wlt.Transactions = new List<Transaction>();
                            usr.Wallet = wlt;
                        }
                        else
                        {
                            usr.Wallet = null;
                        }
                        return usr;
                    },
                    new { UserId = bid.UserId.Value },
                    splitOn: "Wallet_WalletId"
                )).SingleOrDefault();

                // Fallback to QueryMultiple for user/wallet if the above fails, similar to UserDAO.GetByIdAsync
                // For now, assuming if UserDAO.GetByIdAsync QueryMultiple works, a similar approach here will be needed
                // if the QueryAsync<User,Wallet,User> fails.
                // Using UserDAO's QueryMultiple as a model:
                if (userWithWallet == null)
                { // If the above multi-map fails, try UserDAO's QueryMultiple way
                    const string userWalletQueryMultipleSql = @"
                         SELECT UserId, CantBuy, CantSell, UserName, PasswordHash, RegistrationDate, FirstName, LastName, Email, PhoneNumber, Address, IsDeleted FROM [User] WHERE UserId = @UserIdParam;
                         SELECT WalletId, TotalBalance, ReservedBalance, UserId, Version FROM Wallet WHERE UserId = @UserIdParam;";
                    using (var multi = await conn.QueryMultipleAsync(userWalletQueryMultipleSql, new { UserIdParam = bid.UserId.Value }))
                    {
                        User usr = await multi.ReadSingleOrDefaultAsync<User>();
                        if (usr != null)
                        {
                            Wallet wlt = await multi.ReadSingleOrDefaultAsync<Wallet>();
                            if (wlt != null)
                            {
                                wlt.Transactions = new List<Transaction>();
                                usr.Wallet = wlt;
                            }
                            else
                            {
                                usr.Wallet = null;
                            }
                            bid.User = usr;
                        }
                    }
                }
                else
                {
                    bid.User = userWithWallet;
                }
            }
            return bid;
        }



        public async Task<Bid> GetLatestByAuctionId(int auctionId)
        {
            // This method seems like it was already efficient, joining User.
            // Let's adapt it to also join Wallet and use QueryMultiple for robustness.
            using var conn = _connectionFactory();
            const string sql = @"
                SELECT TOP 1 b.BidId, b.Amount, b.TimeStamp, b.UserId, b.AuctionId
                FROM dbo.Bid b
                WHERE b.AuctionId = @AuctionId
                ORDER BY b.Amount DESC, b.TimeStamp DESC;
                
                -- Sub-query for User and Wallet, will only execute if a bid is found
                -- and uses the UserId from that bid
                SELECT u.UserId, u.CantBuy, u.CantSell, u.UserName, u.PasswordHash, u.RegistrationDate, u.FirstName, u.LastName, u.Email, u.PhoneNumber, u.Address, u.IsDeleted,
                       w.WalletId, w.TotalBalance, w.ReservedBalance, w.UserId AS WalletUserId, w.Version AS WalletVersion
                FROM dbo.[User] u
                LEFT JOIN dbo.Wallet w ON u.UserId = w.UserId
                WHERE u.UserId = (SELECT TOP 1 UserId FROM dbo.Bid WHERE AuctionId = @AuctionId ORDER BY Amount DESC, TimeStamp DESC);
            ";

            Bid latestBid = null;
            using (var multi = await conn.QueryMultipleAsync(sql, new { AuctionId = auctionId }))
            {
                latestBid = await multi.ReadSingleOrDefaultAsync<Bid>();
                if (latestBid != null && latestBid.UserId.HasValue)
                {
                    // This structure is a bit more complex for ReadSingleOrDefaultAsync<(User, Wallet)> directly from QueryMultiple
                    // as the second query is conditional on the first. A simpler way is:
                    User user = await multi.ReadSingleOrDefaultAsync<User>(); // Read User from second result set
                    if (user != null)
                    {
                        // We need to ensure the Wallet part is also read. The second SELECT gets User and Wallet.
                        // Let's adjust the second query to only fetch wallet if user exists
                        // Or, more simply, read User and Wallet separately if they are returned as separate objects by Dapper from one SELECT

                        // For simplicity, assuming User object read from second query contains Wallet if joined correctly there
                        // However, the previous QueryMultiple example for UserDAO.GetByIdAsync is better:
                        // Read User, then Read Wallet. Let's adapt that.
                        // The multi-map (User, Wallet, User) is actually for a single SELECT. QueryMultiple needs two SELECTS.

                        // Re-evaluating the second query's return: It returns User and Wallet columns.
                        // We need to map this potentially combined row to User and then Wallet.
                        // This is where a temporary helper object or a direct QueryAsync<User,Wallet,User> on that result set might be needed.
                        // Or, more robustly, always fetch user, then always try to fetch wallet for that user.

                        // Let's assume the second query result is directly mappable to User, and we need to get the Wallet part
                        // This part is tricky with QueryMultiple if the second query joins User and Wallet.
                        // A cleaner way if GetByIdAsync_V2 works for Bid:
                        User bidUser = await GetUserWithWalletById(conn, latestBid.UserId.Value); // Helper
                        latestBid.User = bidUser;
                    }
                }
            }
            return latestBid;
        }




        // Helper method to use within BidDAO if needed, mirroring UserDAO's GetByIdAsync logic
        private async Task<User> GetUserWithWalletById(IDbConnection conn, int userId)
        {
            const string userWalletQueryMultipleSql = @"
                 SELECT UserId, CantBuy, CantSell, UserName, PasswordHash, RegistrationDate, FirstName, LastName, Email, PhoneNumber, Address, IsDeleted FROM [User] WHERE UserId = @UserIdParam;
                 SELECT WalletId, TotalBalance, ReservedBalance, UserId, Version FROM Wallet WHERE UserId = @UserIdParam;";
            User user = null;
            using (var multi = await conn.QueryMultipleAsync(userWalletQueryMultipleSql, new { UserIdParam = userId }))
            {
                user = await multi.ReadSingleOrDefaultAsync<User>();
                if (user != null)
                {
                    Wallet wlt = await multi.ReadSingleOrDefaultAsync<Wallet>();
                    if (wlt != null)
                    {
                        wlt.Transactions = new List<Transaction>();
                        user.Wallet = wlt;
                    }
                    else
                    {
                        user.Wallet = null;
                    }
                }
            }
            return user;
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
            // Step 1: Fetch all bids for the auction
            const string bidsSql = @"SELECT BidId, Amount, TimeStamp, UserId, AuctionId
                                    FROM dbo.Bid
                                    WHERE AuctionId = @AuctionId
                                    ORDER BY TimeStamp DESC;";
            var bids = (await conn.QueryAsync<Bid>(bidsSql, new { AuctionId = auctionId })).ToList();

            if (!bids.Any())
            {
                return bids;
            }

            // Step 2: Collect all unique UserIds from these bids
            var userIds = bids.Where(b => b.UserId.HasValue)
                              .Select(b => b.UserId!.Value)
                              .Distinct()
                              .ToList();

            if (!userIds.Any())
            {
                // This case should ideally not happen if bids always have users.
                // If it can, bids without users will have bid.User = null.
                return bids;
            }

            // Step 3: Fetch all unique users and their wallets for these UserIds
            // Using the QueryMultiple approach that worked for UserDAO
            const string usersWalletsSql = @"
                SELECT u.UserId, u.CantBuy, u.CantSell, u.UserName, u.PasswordHash, u.RegistrationDate, u.FirstName, u.LastName, u.Email, u.PhoneNumber, u.Address, u.IsDeleted
                FROM [User] u
                WHERE u.UserId IN @UserIds;

                SELECT w.WalletId, w.TotalBalance, w.ReservedBalance, w.UserId, w.Version
                FROM Wallet w
                WHERE w.UserId IN @UserIds;
            ";

            Dictionary<int, User> usersMap = new Dictionary<int, User>();
            using (var multi = await conn.QueryMultipleAsync(usersWalletsSql, new { UserIds = userIds }))
            {
                var userList = (await multi.ReadAsync<User>()).ToList();
                var walletList = (await multi.ReadAsync<Wallet>()).ToList();

                var walletsByUserId = walletList.ToDictionary(w => w.UserId);

                foreach (var u in userList)
                {
                    if (walletsByUserId.TryGetValue(u.UserId, out Wallet w))
                    {
                        w.Transactions = new List<Transaction>();
                        u.Wallet = w;
                    }
                    else
                    {
                        u.Wallet = null;
                    }
                    usersMap[u.UserId!.Value] = u;
                }
            }

            // Step 4: Assign users to bids
            foreach (var bid in bids)
            {
                if (bid.UserId.HasValue && usersMap.TryGetValue(bid.UserId.Value, out User user))
                {
                    bid.User = user;
                }
                else
                {
                    bid.User = null; // Should not happen if UserId is FK and not null
                }
            }
            return bids;
        }


        Task<int> IGenericDao<Bid>.InsertAsync(Bid entity)
        {
            throw new NotImplementedException("Use BidDAO.InsertBidAsync instead");
        }
    }
}
