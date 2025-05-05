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

        public UserDAO(IDbConnection dbConnection)
        {
           
            _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        }
        
    
        public async Task<bool> DeleteAsync(User entity)
        {
           const string sql = "DELETE FROM [User] WHERE userId = @userId";
           int rowsAffected = await _dbConnection.ExecuteAsync(sql, new { userId = entity.userId });


            return rowsAffected > 0;
        }

        public async Task<List<T>> GetAllAsync<T>()
        {
            const string sql = "SELECT * FROM [User]";

            var users = await _dbConnection.QueryAsync<User>(sql);

            return users.ToList() as List<T>;

        }

         public async Task<T?> GetByIdAsync<T>(int id)
        {
            const string sql = "SELECT * FROM [User] WHERE userId = @userId";

            var user = await _dbConnection.QuerySingleOrDefaultAsync<T>(sql, new { userId = id });

            return user;
        }

         public async Task<int> InsertAsync(User entity)
        {
            const string sql = "INSERT INTO [User] (CantBuy, CantSell, UserName, PasswordHash, RegistrationDate, FirstName, LastName, Email, PhoneNumber, Address)" +
                "VALUES (@cantBuy, @cantSell, @userName, @passwordHash, @registrationDate, @firstName, @lastName, @email, @phoneNumber, @address); SELECT CAST(SCOPE_IDENTITY() as int);";

            var userId  = await _dbConnection.QuerySingleOrDefaultAsync<int>(sql, new
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
            return  Task.FromResult(userId).Result;
        }

         public async Task<bool> UpdateAsync(User entity)
        {
            const string sql = "UPDATE [User] SET CantBuy = @cantBuy, CantSell = @cantSell, UserName = @userName, RegistrationDate = @registrationDate, FirstName = @firstName, LastName = @lastName, Email = @email, PhoneNumber = @phoneNumber, Address = @address WHERE userId = @userId";
            int rowsaffected = await _dbConnection.ExecuteAsync(sql, new
            {
                userId = entity.userId,
                cantBuy = entity.CantBuy,
                cantSell = entity.CantSell,
                userName = entity.UserName,
                registrationDate = entity.RegistrationDate,
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
