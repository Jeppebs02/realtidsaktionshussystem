using AuctionHouse.DataAccessLayer.Interfaces;
using AuctionHouse.ClassLibrary.Model;
using AuctionHouse.ClassLibrary.Enum;
using System;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionHouse.DataAccessLayer.DAO
{
    public class AuctionDAO : IAuctionDao
    {

        private readonly string _connectionString;
        public AuctionDAO()
        {
            _connectionString = Environment.GetEnvironmentVariable("DatabaseConnectionString");
        }


        /// <summary>
        /// Will NEVER be implemented
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<IEnumerable<Auction>> GetWithinDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// All auctions that a user owns/created (he is selling an item)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<IEnumerable<Auction>> GetAllByUserIDAsync(int userId)
        {
            throw new NotImplementedException();
        }
        public Task<IEnumerable<Auction>> GetAllByBidsAsync(int userId)
        {
            throw new NotImplementedException();
        }
        public Task<IEnumerable<Auction>> GetAllActiveAsync()
        {
            throw new NotImplementedException();
        }


        // GenericDao methods
        Task<int> IGenericDao<Auction>.InsertAsync(Auction entity)
        {
            throw new NotImplementedException();
        }

        Task<bool> IGenericDao<Auction>.UpdateAsync(Auction entity)
        {
            throw new NotImplementedException();
        }

        Task<bool> IGenericDao<Auction>.DeleteAsync(Auction entity)
        {
            throw new NotImplementedException();
        }

        Task<List<T>> IGenericDao<Auction>.GetAllAsync<T>()
        {
            throw new NotImplementedException();
        }

        Task<T?> IGenericDao<Auction>.GetByIdAsync<T>(int id) where T : default
        {
            throw new NotImplementedException();
        }
    }
    {
    }
}
