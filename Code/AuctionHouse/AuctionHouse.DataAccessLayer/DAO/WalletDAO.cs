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
    public class WalletDAO : IWalletDao
    {
        private readonly IDbConnection _dbConnection;

        public WalletDAO(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        //STOLE THIS FROM JEPPE, WHO STOLE IT FROM DAPPER

        public Task<bool> DeleteAsync(Wallet entity)
        {
            const string sql = "DELETE FROM Wallet WHERE WalletId = @WalletId";

            TransactionDAO transactionDao = new TransactionDAO(_dbConnection);

            // Delete all transactions associated with the wallet
            transactionDao.DeleteByWalletId((int)entity.WalletId);

            int rowsAffected = _dbConnection.Execute(sql, new { entity.WalletId });

            return Task.FromResult(rowsAffected > 0);
        }

        public Task<List<T>> GetAllAsync<T>()
        {
            const string sql = "SELECT * FROM Wallet";

            var wallets = _dbConnection.Query<T>(sql).ToList();

            return Task.FromResult(wallets);
        }

        public Task<T?> GetByIdAsync<T>(int id)
        {
            const string sql = "SELECT * FROM Wallet WHERE WalletId = @WalletId";

            var wallet = _dbConnection.QuerySingleOrDefault<T>(sql, new { WalletId = id });

            return Task.FromResult(wallet);
        }

        public Task<Wallet> GetByUserId(int userId)
        {
            const string sql = "SELECT * FROM Wallet WHERE UserId = @UserId";

            var wallet = _dbConnection.QuerySingleOrDefault<Wallet>(sql, new { UserId = userId });

            return Task.FromResult(wallet);
        }

        public Task<int> InsertAsync(Wallet entity)
        {
            const string sql = "INSERT INTO Wallet (TotalBalance, ReservedBalance, UserId) " +
                "VALUES (@TotalBalance, @ReservedBalance, @UserId); SELECT CAST(SCOPE_IDENTITY() as int);";

            var walletId = _dbConnection.QuerySingle<int>(sql, new
            {
                entity.TotalBalance,
                entity.ReservedBalance,
                entity.UserId
            });

            entity.WalletId = walletId;

            return Task.FromResult(walletId);
        }

        public Task<bool> UpdateAsync(Wallet entity)
        {
            const string sql = "UPDATE Wallet SET TotalBalance = @TotalBalance, ReservedBalance = @ReservedBalance " +
                "WHERE WalletId = @WalletId";

            int rowsAffected = _dbConnection.Execute(sql, new
            {
                entity.TotalBalance,
                entity.ReservedBalance,
                entity.WalletId
            });

            return Task.FromResult(rowsAffected > 0);
        }
    }
}
