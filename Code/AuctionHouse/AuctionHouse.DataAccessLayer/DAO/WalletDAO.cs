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
        private readonly ITransactionDao _transactionDao;

        public WalletDAO(IDbConnection dbConnection, ITransactionDao transactionDAO)
        {
            _dbConnection = dbConnection;
            _transactionDao = transactionDAO;
        }

        //STOLE THIS FROM JEPPE, WHO STOLE IT FROM DAPPER

        public async Task<bool> DeleteAsync(Wallet entity)
        {
            const string sql = "DELETE FROM Wallet WHERE WalletId = @WalletId";

            TransactionDAO transactionDao = new TransactionDAO(_dbConnection);

            // Delete all transactions associated with the wallet
            await transactionDao.DeleteByWalletId((int)entity.WalletId);

            int rowsAffected = await _dbConnection.ExecuteAsync(sql, new { entity.WalletId });

            return rowsAffected > 0;
        }

        public async Task<List<T>> GetAllAsync<T>()
        {
            const string sql = @"SELECT
                            WalletId,
                            TotalBalance,
                            ReservedBalance,
                            Version,
                            UserId
                            FROM dbo.Wallet;";

            // Await the result of QueryAsync and then convert it to a list
            var wallets = await _dbConnection.QueryAsync<T>(sql);

            return wallets.ToList();
        }

        public async Task<T?> GetByIdAsync<T>(int id)
        {
            const string sql = "SELECT * FROM Wallet WHERE WalletId = @WalletId";

            var walletT = await _dbConnection.QuerySingleOrDefaultAsync<T>(sql, new { WalletId = id });


            if (walletT is Wallet concreteWallet)
            {

                if (concreteWallet.Transactions == null)
                {
                    concreteWallet.Transactions = new List<Transaction>();
                }
                var transactions = await _transactionDao.GetAllByWalletId(concreteWallet.WalletId.Value);
                foreach ( Transaction transaction in transactions)
                {
                    concreteWallet.Transactions.Add(transaction);

                }
            }

            return walletT;
        }

        public async Task<Wallet> GetByUserId(int userId)
        {
            const string sql = "SELECT * FROM Wallet WHERE UserId = @UserId";

            var wallet = await _dbConnection.QuerySingleOrDefaultAsync<Wallet>(sql, new { UserId = userId });
            if (wallet.Transactions == null)
            {
                wallet.Transactions = new List<Transaction>();
            }

            var transactions = await _transactionDao.GetAllByWalletId(wallet.WalletId.Value);

            foreach (var transaction in transactions)
            {
                wallet.Transactions.Add(transaction);
            }

            return wallet;
        }

        public async Task<int> InsertAsync(Wallet entity)
        {
            const string sql = "INSERT INTO Wallet (TotalBalance, ReservedBalance, UserId) " +
                "VALUES (@TotalBalance, @ReservedBalance, @UserId); SELECT CAST(SCOPE_IDENTITY() as int);";

            

            var walletId = await _dbConnection.QuerySingleAsync<int>(sql, new
            {
                entity.TotalBalance,
                entity.ReservedBalance,
                entity.UserId
            });

            entity.WalletId = walletId;

            return walletId;
        }

        public async Task<bool> UpdateAsync(Wallet entity)
        {
            // TODO
            // This should take a expected version and check against the version in db (like in ReserveFundsOptmisticallyAsync)
            // It should also NOT BE ABLE to change reserved balance, only total balance, so maybe change the name
            const string sql = "UPDATE Wallet SET TotalBalance = @TotalBalance, ReservedBalance = @ReservedBalance " +
                "WHERE WalletId = @WalletId";

            int rowsAffected = await _dbConnection.ExecuteAsync(sql, new
            {
                entity.TotalBalance,
                entity.ReservedBalance,
                entity.WalletId
            });

            return rowsAffected > 0;
        }


        public async Task<bool> ReserveFundsOptimisticallyAsync(int walletId, decimal amountToReserve, byte[] expectedVersion, IDbTransaction transaction = null)
        {
            const string sql = @"
                UPDATE Wallet
                SET ReservedBalance = ReservedBalance + @AmountToReserve
                WHERE WalletId = @WalletId AND Version = @ExpectedVersion;";
            var parameters = new
            {
                AmountToReserve = amountToReserve,
                WalletId = walletId,
                ExpectedVersion = expectedVersion
            };
            int rowsAffected = await _dbConnection.ExecuteAsync(sql, parameters, transaction);
            return rowsAffected > 0;
        }
    }
}
