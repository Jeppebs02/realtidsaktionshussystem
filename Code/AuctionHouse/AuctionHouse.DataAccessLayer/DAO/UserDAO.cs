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
            // This is DI'ed into the constructor.
            _dbConnection = dbConnection;
        }
        
    
        Task<bool> IGenericDao<User>.DeleteAsync(User entity)
        {
           const string sql = "DELETE FROM [User] WHERE userId = @userId";
           int rowsAffected = _dbConnection.Execute(sql, new { userId = entity.userId });


            return Task.FromResult(rowsAffected > 0);
        }

        Task<List<T>> IGenericDao<User>.GetAllAsync<T>()
        {
            const string sql = "SELECT * FROM [User]";

            var users = _dbConnection.Query<User>(sql);

            return Task.FromResult(users.ToList() as List<T>);

        }

        Task<T?> IGenericDao<User>.GetByIdAsync<T>(int id) where T : default
        {
            const string sql = "SELECT * FROM [User] WHERE userId = @userId";

            throw new NotImplementedException();
        }

        Task<int> IGenericDao<User>.InsertAsync(User entity)
        {
            throw new NotImplementedException();
        }

        Task<bool> IGenericDao<User>.UpdateAsync(User entity)
        {
            throw new NotImplementedException();
        }
    }

    
    
    
}
