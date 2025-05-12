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
        private readonly Func<IDbConnection> _connectionFactory;
        private readonly ITransactionDao _transactionDao;

        public WalletDAO(Func<IDbConnection> connectionFactory, ITransactionDao transactionDAO)
        {
            _connectionFactory = connectionFactory;
            _transactionDao = transactionDAO;
        }

        //STOLE THIS FROM JEPPE, WHO STOLE IT FROM DAPPER

        public async Task<bool> DeleteAsync(Wallet entity)
        {
            const string sql = "DELETE FROM Wallet WHERE WalletId = @WalletId";
            
            using var conn = _connectionFactory();

            TransactionDAO transactionDao = new TransactionDAO(_connectionFactory);

            // Delete all transactions associated with the wallet
            await transactionDao.DeleteByWalletId((int)entity.WalletId);

            int rowsAffected = await conn.ExecuteAsync(sql, new { entity.WalletId });

            return rowsAffected > 0;
        }

        public async Task<List<Wallet>> GetAllAsync()
        {
            using var conn = _connectionFactory();

            const string sql = @"SELECT
                            WalletId,
                            TotalBalance,
                            ReservedBalance,
                            Version,
                            UserId
                            FROM dbo.Wallet;";

            // Await the result of QueryAsync and then convert it to a list
            var wallets = await conn.QueryAsync<Wallet>(sql);

            return wallets.ToList();
        }

        public async Task<Wallet?> GetByIdAsync(int id)
        {
            using var conn = _connectionFactory();

            const string sql = "SELECT * FROM Wallet WHERE WalletId = @WalletId";

            var walletT = await conn.QuerySingleOrDefaultAsync<Wallet>(sql, new { WalletId = id });


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
            using var conn = _connectionFactory();

            const string sql = @"SELECT 
                                WalletId,
                                TotalBalance,
                                ReservedBalance,
                                UserId,
                                Version
                            FROM Wallet
                            WHERE UserId = @UserId;";

            Console.WriteLine("in walletDAO");

            var wallet = await conn.QuerySingleOrDefaultAsync<Wallet>(sql, new { UserId = userId });


            if (wallet == null)
            {
                Console.WriteLine("Wallet is null from walletdao");
            }
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
            using var conn = _connectionFactory();

            const string sql = "INSERT INTO Wallet (TotalBalance, ReservedBalance, UserId) " +
                "VALUES (@TotalBalance, @ReservedBalance, @UserId); SELECT CAST(SCOPE_IDENTITY() as int);";

            

            var walletId = await conn.QuerySingleAsync<int>(sql, new
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
            // USE ANOTHER FUNCTION
            return false;
        }


        public async Task<bool> ReserveFundsOptimisticallyAsync(int walletId, decimal amountToReserve, byte[] expectedVersion, IDbTransaction transaction = null)
        {

            var conn = _connectionFactory();

            //Make sure to use the same connection as the transaction if it is not null
            if (transaction != null)
            {
                conn = transaction.Connection;
            }

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
            int rowsAffected = await conn.ExecuteAsync(sql, parameters, transaction);
            return rowsAffected > 0;
        }

        public async Task<byte[]> UpdateTotalBalanceAsync(Wallet entity)
        {
            using var conn = _connectionFactory();

            const string sql = @"
                                UPDATE Wallet
                                SET TotalBalance = @TotalBalance
                                OUTPUT INSERTED.Version
                                WHERE WalletId = @WalletId AND Version = @ExpectedVersion;
                            ";

            // Use OUTPUT INSERTED.Version to get the new version after the update
            // The OUTPUT clause returns the value of the specified column from the updated row
            // This is why we put byte[]? in the return type. To say "execute this sql and return the version of the wallet"

            var result = await conn.QueryFirstOrDefaultAsync<byte[]?>(sql, new
            {
                entity.TotalBalance,
                entity.WalletId,
                ExpectedVersion = entity.Version
            });

            // If the update was successful, the new version will be returned
            // If no rows were affected (unsuccessful update), result will be null
            return result;
        }
    }
}
