using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuctionHouse.ClassLibrary.Model;
using AuctionHouse.DataAccessLayer.Interfaces;
using System.Data.SqlClient;
using Dapper;
using System.Dynamic;

namespace AuctionHouse.DataAccessLayer.DAO
{
    public class UserDAO : IUserDao
{
        private readonly Func<IDbConnection> _connectionFactory;
        private readonly IWalletDao walletDAO;

        public UserDAO(Func<IDbConnection> connectionFactory, IWalletDao walletDAO)
        {

            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            this.walletDAO = walletDAO;
        }


        public async Task<bool> DeleteAsync(User entity)
        {
            using var conn = _connectionFactory();

            const string sql = "UPDATE [User] set isDeleted = 1 where UserId = @UserId";
           int rowsAffected = await conn.ExecuteAsync(sql, new { UserId = entity.UserId });


            return rowsAffected > 0;
        }

        public async Task<List<User>> GetAllAsync()
        {
            using var conn = _connectionFactory();
            const string sql = @"SELECT
                            u.UserId, u.CantBuy, u.CantSell, u.UserName, u.PasswordHash,
                            u.RegistrationDate, u.FirstName, u.LastName, u.Email,
                            u.PhoneNumber, u.Address, u.IsDeleted,
                            w.WalletId AS Wallet_WalletId, w.TotalBalance AS Wallet_TotalBalance,
                            w.ReservedBalance AS Wallet_ReservedBalance, w.UserId AS Wallet_UserId,
                            w.Version AS Wallet_Version
                        FROM [User] u
                        LEFT JOIN Wallet w ON u.UserId = w.UserId
                        WHERE u.isDeleted = 0;";

            var users = await conn.QueryAsync<User, Wallet, User>(
                sql,
                (user, wallet) =>
                {
                    user.Wallet = wallet;
                    if (user.Wallet != null)
                    {
                        user.Wallet.Transactions = new List<Transaction>();
                    }
                    return user;
                },
                splitOn: "Wallet_WalletId"
            );
            return users.ToList();
        }

        public async Task<User> GetByIdAsync(int id)
        {
            using var conn = _connectionFactory();
            const string sql = @"SELECT
                            u.UserId, u.CantBuy, u.CantSell, u.UserName, u.PasswordHash,
                            u.RegistrationDate, u.FirstName, u.LastName, u.Email,
                            u.PhoneNumber, u.Address, u.IsDeleted,
                            -- Wallet part (aliased for Dapper multi-mapping)
                            w.WalletId AS Wallet_WalletId, w.TotalBalance AS Wallet_TotalBalance,
                            w.ReservedBalance AS Wallet_ReservedBalance, w.UserId AS Wallet_UserId,
                            w.Version AS Wallet_Version
                        FROM [User] u
                        LEFT JOIN Wallet w ON u.UserId = w.UserId
                        WHERE u.UserId = @UserId;";

            var userResult = await conn.QueryAsync<User, Wallet, User>(
                sql,
                (user, wallet) =>
                {
                    if (user != null) // User should exist if found by ID
                    {
                        user.Wallet = wallet; // Wallet can be null if LEFT JOIN and no wallet exists
                        if (user.Wallet != null)
                        {
                            user.Wallet.Transactions = new List<Transaction>(); // Initialize, don't load
                        }
                    }
                    return user;
                },
                new { UserId = id },
                splitOn: "Wallet_WalletId" // Dapper splits to a new Wallet object here
            );

            return userResult.SingleOrDefault();
        }

        public async Task<int> InsertAsync(User entity)
        {
            using var conn = _connectionFactory();

            const string sql = "INSERT INTO [User] (CantBuy, CantSell, UserName, PasswordHash, RegistrationDate, FirstName, LastName, Email, PhoneNumber, Address)" +
                "VALUES (@cantBuy, @cantSell, @userName, @passwordHash, @registrationDate, @firstName, @lastName, @email, @phoneNumber, @address); SELECT CAST(SCOPE_IDENTITY() as int);";

            var UserId  = await conn.QuerySingleOrDefaultAsync<int>(sql, new
            {
                cantBuy = entity.CantBuy,
                cantSell = entity.CantSell,
                userName = entity.UserName,
                passwordHash = entity.Password,
                registrationDate = entity.RegistrationDate,
                firstName = entity.FirstName,
                lastName = entity.LastName,
                email = entity.Email,
                phoneNumber = entity.PhoneNumber,
                address = entity.Address
            });

            Wallet wallet = new Wallet(0, 0, UserId);
            await walletDAO.InsertAsync(wallet);
            return  Task.FromResult(UserId).Result;
        }

         public async Task<bool> UpdateAsync(User entity)
        {
            using var conn = _connectionFactory();

            const string sql = "UPDATE [User] SET CantBuy = @cantBuy, CantSell = @cantSell, UserName = @userName, FirstName = @firstName, LastName = @lastName, Email = @email, PhoneNumber = @phoneNumber, [Address] = @address WHERE UserId = @UserId";
            int rowsaffected = await conn.ExecuteAsync(sql, new
            {
                UserId = entity.UserId,
                cantBuy = entity.CantBuy,
                cantSell = entity.CantSell,
                userName = entity.UserName,
                firstName = entity.FirstName,
                lastName = entity.LastName,
                email = entity.Email,
                phoneNumber = entity.PhoneNumber,
                address = entity.Address
            });

            return Task.FromResult(rowsaffected > 0).Result;
        }
    }

    
    
    
}
