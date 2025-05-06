using AuctionHouse.ClassLibrary.Model;
using AuctionHouse.DataAccessLayer.DAO;
using AuctionHouse.DataAccessLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionHouse.Test.DaoTests
{
    public class BidDaoTest
    {
        #region Fields
        private readonly IDbConnection _connection;
        private readonly IBidDao _bidDao;
        private readonly IUserDao _userDao;
        #endregion

        #region Constructor
        public BidDaoTest()
        {
            string connectionString = Environment.GetEnvironmentVariable("DatabaseConnectionString");
             this._connection = new SqlConnection(connectionString);



            TransactionDAO tdao = new TransactionDAO(_connection);

            WalletDAO wdao = new WalletDAO(_connection, tdao);

            _userDao = new UserDAO(_connection, wdao);

            _bidDao = new BidDAO(_connection);

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
        [Fact(Skip = "Skipped")]
        public async Task GetAllAsync_ShouldReturnListOfBids()
        {
            // Act
            List<Bid> bids = await _bidDao.GetAllAsync<Bid>();
                      
            // Assert   
            Assert.NotEmpty(bids);
        }


        [Fact]
        public async Task GetByIdAsync_ShouldReturnBid_WhenBidExists()
        {
            // Arrange  
            int bidId = 1;
            // Act  
            Bid? bid = await _bidDao.GetByIdAsync<Bid>(bidId);
            // Assert   
            Assert.NotNull(bid);
            Assert.Equal(bidId, bid?.BidId);
        }

        [Fact]
        public async Task GetLatestByAuctionId_ShouldReturnLatestBid_WhenBidsExist()
        {
            // Arrange  
            int auctionId = 1;
            // Act  
            Bid? bid = await _bidDao.GetLatestByAuctionId(auctionId);
            // Assert   
            Assert.NotNull(bid);
            Assert.Equal(600, bid.Amount);
        }

        [Fact]
        public async Task InsertAsync_ShouldReturnId_WhenBidIsInserted()
        {
            // Arrange  
            User user = await _userDao.GetByIdAsync<User>(1);
            Bid bid = new Bid(1, 500, DateTime.Now, user);
            // Act  
            int id = await _bidDao.InsertBidAsync(bid);
            // Assert  
            Assert.True(id >= 5);
        }
        #endregion
    }
}
