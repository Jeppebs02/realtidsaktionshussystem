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

    [Collection("Sequential")]
    public class TransactionDaoTest
    {
        #region Fields
        private readonly Func<IDbConnection> _connectionFactory;
        private readonly ITransactionDao _transactionDao;
        
        #endregion

        #region Constructor
        public TransactionDaoTest()
        {
            var cs = Environment.GetEnvironmentVariable("DatabaseConnectionString")
                 ?? "Server=localhost;Database=AuctionHouseTest;Trusted_Connection=True;TrustServerCertificate=True;";

            _connectionFactory = () =>
            {
                var c = new SqlConnection(cs);
                c.Open();                       // hand callers an *OPEN* connection
                return c;
            };
            
            _transactionDao = new TransactionDAO(_connectionFactory);

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
