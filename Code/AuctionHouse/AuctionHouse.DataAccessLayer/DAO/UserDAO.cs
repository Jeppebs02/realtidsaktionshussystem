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

        public async Task<bool> DeleteByIdAsync(int id)
        {
            using var conn = _connectionFactory();
            const string sql = "UPDATE [User] set isDeleted = 1 where UserId = @UserId";
            int rowsAffected = await conn.ExecuteAsync(sql, new { UserId = id });
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
            // SQL to fetch User, then Wallet in two separate select statements in one batch
            const string sql = @"
        SELECT -- User part
            u.UserId, u.CantBuy, u.CantSell, u.UserName, u.PasswordHash,
            u.RegistrationDate, u.FirstName, u.LastName, u.Email,
            u.PhoneNumber, u.Address, u.IsDeleted
        FROM [User] u
        WHERE u.UserId = @UserIdParam;

        SELECT -- Wallet part
            w.WalletId, w.TotalBalance, w.ReservedBalance,
            w.UserId, w.Version
        FROM Wallet w
        WHERE w.UserId = @UserIdParam;
    ";

            User user = null;
            Wallet wallet = null;

            Console.WriteLine($"Executing QueryMultiple for GetByIdAsync with UserId: {id}");

            using (var multi = await conn.QueryMultipleAsync(sql, new { UserIdParam = id }))
            {
                // Read the first result set (User)
                user = await multi.ReadSingleOrDefaultAsync<User>();

                if (user != null)
                {
                    Console.WriteLine($"User read from QueryMultiple: UserId={user.UserId}, UserName={user.UserName}");
                    // Read the second result set (Wallet)
                    wallet = await multi.ReadSingleOrDefaultAsync<Wallet>();

                    if (wallet != null)
                    {
                        Console.WriteLine($"Wallet read from QueryMultiple: WalletId={wallet.WalletId}, Version present={wallet.Version != null}");
                        if (wallet.Version != null)
                        {
                            Console.WriteLine($"Wallet Version Length: {wallet.Version.Length}");
                        }
                        user.Wallet = wallet; // Assign the fetched wallet
                        if (user.Wallet.Transactions == null) // Should be initialized by Wallet constructor
                        {
                            user.Wallet.Transactions = new List<Transaction>();
                        }
                    }
                    else
                    {
                        Console.WriteLine($"No wallet found for UserId {id} in the second result set.");
                        user.Wallet = null; // Explicitly set to null if no wallet
                    }
                }
                else
                {
                    Console.WriteLine($"No user found for UserId {id} in the first result set.");
                }
            }

            if (user != null && user.Wallet != null)
            {
                Console.WriteLine($"Final User.Wallet (QueryMultiple): WalletId={user.Wallet.WalletId}, Version present={user.Wallet.Version != null}");
            }
            else if (user != null)
            {
                Console.WriteLine($"Final User.Wallet is NULL for user {user.UserId} (QueryMultiple)");
            }

            return user;
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







        // TEMPORARY TEST METHOD




    }

    
    
    
}
