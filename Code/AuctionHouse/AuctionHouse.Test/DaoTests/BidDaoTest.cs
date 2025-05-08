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

    [Collection("Sequential")]
    public class BidDaoTest
    {
        #region Fields
        private readonly Func<IDbConnection> _connectionFactory;
        private readonly IBidDao _bidDao;
        private readonly IUserDao _userDao;
        #endregion

        #region Constructor
        public BidDaoTest()
        {
            var cs = Environment.GetEnvironmentVariable("DatabaseConnectionString")
                  ?? "Server=localhost;Database=AuctionHouseTest;Trusted_Connection=True;TrustServerCertificate=True;";

            _connectionFactory = () =>
            {
                var c = new SqlConnection(cs);
                c.Open();                       // hand callers an *OPEN* connection
                return c;
            };

            TransactionDAO tdao = new TransactionDAO(_connectionFactory);

            WalletDAO wdao = new WalletDAO(_connectionFactory, tdao);

            _userDao = new UserDAO(_connectionFactory, wdao);

            _bidDao = new BidDAO(_connectionFactory, _userDao);

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
            List<Bid> bids = await _bidDao.GetAllAsync();
                      
            // Assert   
            Assert.NotEmpty(bids);
        }


        [Fact]
        public async Task GetByIdAsync_ShouldReturnBid_WhenBidExists()
        {
            // Arrange  
            int bidId = 1;
            // Act  
            Bid? bid = await _bidDao.GetByIdAsync(bidId);
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
            User user = await _userDao.GetByIdAsync(1);
            Bid bid = new Bid(1, 500, DateTime.Now, user);
            // Act  
            int id = await _bidDao.InsertBidAsync(bid);
            // Assert  
            Assert.True(id >= 5);
        }
        #endregion
    }
}
