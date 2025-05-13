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
        private readonly Func<IDbConnection> _connectionFactory;
        #endregion

        #region Constructor
        public WalletDaoTest()
        {
            var cs = Environment.GetEnvironmentVariable("DatabaseConnectionString")
                 ?? "Server=localhost;Database=AuctionHouseTest;Trusted_Connection=True;TrustServerCertificate=True;";

            _connectionFactory = () =>
            {
                var c = new SqlConnection(cs);
                c.Open();                       // hand callers an *OPEN* connection
                return c;
            };

            TransactionDAO transactionDAO = new TransactionDAO(_connectionFactory);
            _walletDao = new WalletDAO(_connectionFactory, transactionDAO);
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
            decimal old_balance = wallet.TotalBalance; // Store the old balance for verification
            wallet.TotalBalance += 50; // Update the wallet balance

            // Act  
            byte[] result = await _walletDao.UpdateTotalBalanceAsync(wallet);

            decimal new_balance = _walletDao.GetByUserId(1).Result.TotalBalance; // Get the updated balance

            // Assert  
            Assert.NotEqual(old_balance,new_balance);
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
