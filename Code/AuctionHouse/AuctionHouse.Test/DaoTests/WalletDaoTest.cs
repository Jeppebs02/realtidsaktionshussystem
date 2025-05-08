using AuctionHouse.ClassLibrary.Model;
using AuctionHouse.DataAccessLayer.DAO;
using AuctionHouse.DataAccessLayer.Interfaces;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace AuctionHouse.Test.DaoTests
{

    [Collection("Sequential")]
    public class WalletDaoTest
    {
        #region Fields
        private readonly IWalletDao _walletDao;
        private readonly IDbConnection _connection;
        private readonly ITestOutputHelper _output;
        #endregion

        #region Constructor
        public WalletDaoTest(ITestOutputHelper output)
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
            TransactionDAO transactionDAO = new TransactionDAO(_connection);
            _walletDao = new WalletDAO(_connection, transactionDAO);
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
        [Fact]
        public async Task GetByUserId_ShouldReturnWallet_WhenUserIdExists()
        {
            // Arrange  
            int userId = 1;

            // Act  
            Wallet wallet = await _walletDao.GetByUserId(userId);

            // Assert  
            Assert.NotNull(wallet);
            Assert.Equal(userId, wallet.UserId); // Assuming Wallet has a UserId property  
        }


        //TODO DELETE CUZ USER TEST SHOULD TEST THIS!

        [Fact(Skip = "Not needed rn")]
        public async Task InsertAsync_ShouldReturnId_WhenWalletIsInserted()
        {
            // Arrange  
            // remember user with id 4 must have no wallet in db at the time of this test
            
            Wallet wallet = new Wallet(100, 0, 4);

            // Act  
            int id = await _walletDao.InsertAsync(wallet);

            // Assert  
            Assert.True(id > 0);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnTrue_WhenWalletIsUpdated()
        {
            // Arrange  
            Wallet wallet = await _walletDao.GetByUserId(1); // Assuming user with ID 1 exists
            wallet.TotalBalance += 50; // Update the wallet balance

            // Act  
            bool result = await _walletDao.UpdateAsync(wallet);

            // Assert  
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnTrue_WhenWalletIsDeleted()
        {
            // Arrange  
            Wallet wallet = await _walletDao.GetByUserId(2);

            // Act  
            bool result = await _walletDao.DeleteAsync(wallet);

            // Assert  
            Assert.True(result);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnListOfWallets()
        {
            // Act  
            List<Wallet> wallets = await _walletDao.GetAllAsync();

            // Assert  
            Assert.NotEmpty(wallets);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnWallet_WhenIdExists()
        {
            // Arrange  
            int id = 1;

            // Act  
            Wallet wallet = await _walletDao.GetByIdAsync(id);

            // Assert  
            Assert.NotNull(wallet);
            Assert.Equal(id, wallet.WalletId); // Assuming Wallet has an Id property  
        }
        #endregion
    }
}
