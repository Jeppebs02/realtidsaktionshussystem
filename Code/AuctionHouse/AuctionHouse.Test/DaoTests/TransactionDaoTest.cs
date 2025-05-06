using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuctionHouse.ClassLibrary.Model;
using AuctionHouse.DataAccessLayer.DAO;
using AuctionHouse.DataAccessLayer.Interfaces;
using System.Data.SqlClient;
using Xunit.Abstractions;
using AuctionHouse.DataAccessLayer.DAO;

namespace AuctionHouse.Test.DaoTests
{
    public class TransactionDaoTest
    {
        #region Fields
        private readonly IDbConnection _connection;
        private readonly ITransactionDao _transactionDao;
        private readonly ITestOutputHelper _output;
        #endregion

        #region Constructor
        public TransactionDaoTest(ITestOutputHelper output)
        {
            _output = output;

            _output.WriteLine("Test Constructor: Attempting to read environment variable...");
            string connectionString = Environment.GetEnvironmentVariable("DatabaseConnectionString");


            _output.WriteLine($"Test Constructor: Retrieved Connection String: '{connectionString ?? "NULL"}'");

            if (string.IsNullOrEmpty(connectionString))
            {
                _output.WriteLine("Test Constructor: FATAL - Connection string is null or empty."); // Log before throwing
                // Consider throwing a more specific exception or using Assert.True within a test setup method if applicable
                throw new InvalidOperationException("FATAL: DatabaseConnectionString environment variable is not set or is empty. Check test configuration.");
            }

            _output.WriteLine("Test Constructor: Creating SqlConnection...");
            _connection = new SqlConnection(connectionString);
            _output.WriteLine("Test Constructor: Creating ItemDAO...");
            _transactionDao = new TransactionDAO(_connection);
            _output.WriteLine("Test Constructor: Setup complete.");

            //Clean tables
            CleanAndBuild.CleanDB();
            //Generate test data
            CleanAndBuild.GenerateFreshTestDB();
        }
        #endregion

        #region Build up and tear down methods
        // Clean up after each test
        public void Dispose()
        {
            CleanAndBuild.CleanDB();
        }
        #endregion

        #region Test
        public Task<bool> DeleteAsync(Transaction entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteByWalletId(int walletId)
        {
            throw new NotImplementedException();
        }

        public Task<List<T>> GetAllAsync<T>()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public async Task GetAllByWalletId_ShouldReturnAListOfTransactions()
        {
            // Arrange  
            int walletId = 1;

            // Act  
            IEnumerable<Transaction> transactions = await _transactionDao.GetAllByWalletId(walletId);
          
              
            // Assert  
            Assert.NotEmpty(transactions);
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
        #endregion
    }
}
