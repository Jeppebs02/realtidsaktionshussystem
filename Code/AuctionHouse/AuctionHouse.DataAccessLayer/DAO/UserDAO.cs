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
        private readonly IDbConnection _dbConnection;
        private readonly IWalletDao walletDAO;

        public UserDAO(IDbConnection dbConnection, IWalletDao walletDAO)
        {

            _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
            this.walletDAO = walletDAO;
        }


        public async Task<bool> DeleteAsync(User entity)
        {
           const string sql = "UPDATE [User] set isDeleted = 1 where UserId = @UserId";
           int rowsAffected = await _dbConnection.ExecuteAsync(sql, new { UserId = entity.UserId });


            return rowsAffected > 0;
        }

        public async Task<List<User>> GetAllAsync()
        {
            const string sql = @"SELECT * FROM [User] WHERE isDeleted = 0";

            var users = await _dbConnection.QueryAsync<User>(sql);

                foreach (var user in users)
                {
                    var wallet = await walletDAO.GetByUserId(user.UserId.Value);
                    user.Wallet = wallet;
                }
            

            return users.ToList();
        }

        public async Task<User> GetByIdAsync(int id)
        {
            const string sql = @"SELECT 
                                UserId,
                                CantBuy,
                                CantSell,
                                UserName,
                                PasswordHash,
                                RegistrationDate,
                                FirstName,
                                LastName,
                                Email,
                                PhoneNumber,
                                Address,
                                IsDeleted
                            FROM [User]
                            WHERE UserId = @UserId;";

            var user = await _dbConnection.QuerySingleOrDefaultAsync<User>(sql, new { UserId = id });


                Console.WriteLine($"UserId from UserDAO: {user!.UserId}");
            // This fixed it
                var wallet = await walletDAO.GetByUserId(user.UserId!.Value);
                if(user.Wallet == null)
                {
                user.Wallet = wallet;
                }


            

            return user;
        }

         public async Task<int> InsertAsync(User entity)
        {
            const string sql = "INSERT INTO [User] (CantBuy, CantSell, UserName, PasswordHash, RegistrationDate, FirstName, LastName, Email, PhoneNumber, Address)" +
                "VALUES (@cantBuy, @cantSell, @userName, @passwordHash, @registrationDate, @firstName, @lastName, @email, @phoneNumber, @address); SELECT CAST(SCOPE_IDENTITY() as int);";

            var UserId  = await _dbConnection.QuerySingleOrDefaultAsync<int>(sql, new
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
            const string sql = "UPDATE [User] SET CantBuy = @cantBuy, CantSell = @cantSell, UserName = @userName, FirstName = @firstName, LastName = @lastName, Email = @email, PhoneNumber = @phoneNumber, [Address] = @address WHERE UserId = @UserId";
            int rowsaffected = await _dbConnection.ExecuteAsync(sql, new
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
