using AuctionHouse.ClassLibrary.Model;
using AuctionHouse.DataAccessLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionHouse.DataAccessLayer.Stub
{
    internal class AuctionDaoStub : IAuctionDao
    {
        private List<Auction> _auctions = new();
        public Task<bool> DeleteAsync(Task t)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Auction>> GetAllActiveAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<T>> GetAllAsync<T>()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Auction>> GetAllByBidsAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Auction>> GetAllByUserIDAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<T?> GetByIdAsync<T>(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Auction> GetWithinDateRange(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Auction>> GetWithinDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public Task<int> InsertAsync(Task t)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(Task t)
        {
            throw new NotImplementedException();
        }

        Task<bool> IGenericDao<Auction>.DeleteAsync(Auction entity)
        {
            throw new NotImplementedException();
        }

        Task<int> IGenericDao<Auction>.InsertAsync(Auction entity)
        {
            throw new NotImplementedException();
        }

        Task<bool> IGenericDao<Auction>.UpdateAsync(Auction entity)
        {
            throw new NotImplementedException();
        }
    }
}
