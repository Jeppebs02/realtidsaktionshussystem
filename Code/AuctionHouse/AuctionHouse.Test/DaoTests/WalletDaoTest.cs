using AuctionHouse.ClassLibrary.Model;
using AuctionHouse.DataAccessLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace AuctionHouse.Test.DaoTests
{
    public class WalletDaoTest
    {
        private readonly IWalletDao _walletDao;


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

        [Fact]
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
            List<Wallet> wallets = await _walletDao.GetAllAsync<Wallet>();

            // Assert  
            Assert.NotEmpty(wallets);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnWallet_WhenIdExists()
        {
            // Arrange  
            int id = 1;

            // Act  
            Wallet wallet = await _walletDao.GetByIdAsync<Wallet>(id);

            // Assert  
            Assert.NotNull(wallet);
            Assert.Equal(id, wallet.WalletId); // Assuming Wallet has an Id property  
        }
    }


}
