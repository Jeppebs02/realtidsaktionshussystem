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


        private readonly Func<IDbConnection> _connectionFactory;

        public TransactionDAO(Func<IDbConnection> connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public Task<bool> DeleteAsync(Transaction entity)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteByWalletId(int walletId)
        {
            using var conn = _connectionFactory();

            const string sql = "DELETE FROM [Transaction] WHERE WalletId = @WalletId";

            int rowsAffected = await conn.ExecuteAsync(sql, new { WalletId = walletId });

            return rowsAffected > 0;

        }

        public Task<List<Transaction>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Transaction>> GetAllByWalletId(int walletId)
        {
            using var conn = _connectionFactory();

            const string sql = "SELECT * FROM [Transaction] WHERE WalletId = @WalletId";

            var transactions = await conn.QueryAsync<Transaction>(sql, new { WalletId = walletId });

            return transactions;
        }

        public Task<Transaction> GetByIdAsync(int id)
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
