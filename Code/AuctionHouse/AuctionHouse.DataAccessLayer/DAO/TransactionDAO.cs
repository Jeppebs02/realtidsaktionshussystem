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
    public class TransactionDAO : ITransactionDao
    {


        private readonly IDbConnection _dbConnection;

        public TransactionDAO(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public Task<bool> DeleteAsync(Transaction entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteByWalletId(int walletId)
        {
            const string sql = "DELETE FROM [Transaction] WHERE WalletId = @WalletId";

            int rowsAffected = _dbConnection.Execute(sql, new { WalletId = walletId });

            return Task.FromResult(rowsAffected > 0);

        }

        public Task<List<T>> GetAllAsync<T>()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Transaction>> GetAllByWalletId(int walletId)
        {
            throw new NotImplementedException();
        }

        public Task<T?> GetByIdAsync<T>(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> InsertAsync(Transaction entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(Transaction entity)
        {
            throw new NotImplementedException();
        }
    }
}
