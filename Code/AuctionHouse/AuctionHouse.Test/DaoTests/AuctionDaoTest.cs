using AuctionHouse.DataAccessLayer.Interfaces;
using AuctionHouse.ClassLibrary.Enum;
using AuctionHouse.ClassLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using AuctionHouse.DataAccessLayer.DAO;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace AuctionHouse.Test.DaoTests
{

    [Collection("Sequential")]
    public class AuctionDaoTest
    {
        #region Fields
        private readonly Func<IDbConnection> _connectionFactory;

        private readonly IAuctionDao _auctionDao;
        private readonly IBidDao _bidDao;
        private readonly IItemDao _itemDao;
        private readonly IUserDao _userDao;
        private readonly IWalletDao _walletDao;
        private readonly ITransactionDao _transactionDao;
        #endregion

        #region Constructor
        public AuctionDaoTest()
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
            _walletDao = new WalletDAO(_connectionFactory, _transactionDao);
            _userDao = new UserDAO(_connectionFactory,_walletDao);
            _bidDao = new BidDAO(_connectionFactory, _userDao);
            _itemDao = new ItemDAO(_connectionFactory, _userDao);
            _auctionDao = new AuctionDAO(_connectionFactory,_bidDao, _itemDao, _userDao);

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
        [Fact(Skip = "Not needed rn")]
        public async Task GetWithinDateRangeAsync_ShouldReturnAuctions_WhenDateRangeIsValid()
        {
            // Arrange  
            DateTime startDate = new DateTime(2023, 1, 1);
            DateTime endDate = new DateTime(2023, 12, 31);
            // Act  
            IEnumerable<Auction> auctions = await _auctionDao.GetWithinDateRangeAsync(startDate, endDate);
            // Assert  
            Assert.NotNull(auctions);

            //Check that auction start and end times are within the specified range
            Assert.All(auctions, auction => Assert.InRange(auction.StartTime, startDate, endDate));
            Assert.All(auctions, auction => Assert.InRange(auction.EndTime, startDate, endDate));
        }


        [Fact(Skip = "Not needed rn")]
        public async Task GetAllByUserIDAsync_ShouldReturnAuctions_WhenUserIdExists()
        {
            // Arrange  
            int userId = 1;
            // Act  
            IEnumerable<Auction> auctions = await _auctionDao.GetAllByUserIDAsync(userId);

            IEnumerable<Auction> allAuctions = await _auctionDao.GetAllAsync();

            IEnumerable<Auction> auctions2 = new List<Auction>();
            // Assert  
            Assert.NotNull(auctions);

            foreach (var auction in allAuctions)
            {
                if (auction.item.User.UserId == userId)
                {
                    auctions2.Append(auction);
                }
            }

            //Get all auctions that the user has an item in.
            Assert.Equal(auctions, auctions2);

            //will Possibly break
        }


        [Fact(Skip = "Not needed rn")]
        public async Task GetAllByBidsAsync_ShouldReturnAuctions_WhenUserIdExists()
        {
            // Arrange  
            int userId = 1;
            // Act  
            IEnumerable<Auction> auctions = await _auctionDao.GetAllByBidsAsync(userId);

            IEnumerable<Auction> auctions2 = new List<Auction>();

            IEnumerable<Auction> allAuctions = await _auctionDao.GetAllAsync();

            foreach (var auction in allAuctions)
            {
                if (auction.Bids.Any(b => b.User.UserId == userId))
                {
                    auctions2.Append(auction);
                }
            }

            // Assert  
            Assert.NotNull(auctions);
            Assert.Equal(auctions, auctions2);
            
        }


        [Fact]
        public async Task GetAllActiveAsync_ShouldReturnActiveAuctions()
        {
            // Arrange  
            // Act  
            IEnumerable<Auction> auctions = await _auctionDao.GetAllActiveAsync();
            // Assert  
            Assert.NotNull(auctions);
            // Check that all auctions are active
            Assert.All(auctions, auction => Assert.Equal(AuctionStatus.ACTIVE, auction.AuctionStatus));
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllAuctions()
        {
            // Arrange  
            // Act  
            IEnumerable<Auction> auctions = await _auctionDao.GetAllAsync();
            // Assert  
            Assert.NotNull(auctions);
            // Check that all auctions are returned
            Assert.NotEmpty(auctions);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnAuction_WhenIdExists()
        {
            // Arrange  
            int auctionId = 1;
            // Act  
            Auction auction = await _auctionDao.GetByIdAsync(auctionId);
            // Assert  
            Assert.NotNull(auction);
            Assert.Equal(auctionId, auction.AuctionID);
        }

        [Fact]
        public async Task InsertAsync_ShouldInsertAuction()
        {
            // Arrange  
            Wallet wallet = new Wallet(1000, 0,0);
            User user = new User("testUser", "testPassword", "firstname", "lastname", "email", "phonenr", "address", wallet);
            user.UserId = 1;
            Item item = new Item(user, "testItem", "testDescription", Category.ELECTRONICS, new byte[0]);
            item.ItemId = 1;
            Auction auction = new Auction(DateTime.Now, DateTime.Now.AddDays(1), 100, 200, 10, false, item);
            // Act  
            int newAuctionId = await _auctionDao.InsertAsync(auction);
            // Assert  
            Assert.True(newAuctionId>0);
        }

        [Fact]
        public async Task UpdateOptimisticallyAsync_ShouldChangeVersion()
        {
            // Arrange  
            int auctionId = 1;
            Auction auction = await _auctionDao.GetByIdAsync(auctionId);
            var originalVersion = auction.Version;

            // Act  
            byte[] result = await _auctionDao.UpdateAuctionOptimistically(auctionId, auction.Version);

            Auction newAuction = await _auctionDao.GetByIdAsync(auctionId);
            var newVersion = newAuction.Version;
            // Assert  
            Assert.NotEqual(originalVersion,newVersion);
        }

        [Fact(Skip = "Not needed rn")]
        public async Task DeleteAsync_ShouldDeleteAuction()
        {
            // Arrange  
            int auctionId = 1;
            Auction auction = await _auctionDao.GetByIdAsync(auctionId);
            // Act  
            bool result = await _auctionDao.DeleteAsync(auction);
            // Assert  
            Assert.True(result);
        }


        //TODO: Create a test for AuctionDAO.UpdateAuctionStatusOptimisticallyAsync()
        #endregion
    }
}
