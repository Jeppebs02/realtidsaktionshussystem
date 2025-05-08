using AuctionHouse.ClassLibrary.Model;
using AuctionHouse.DataAccessLayer.DAO;
using AuctionHouse.DataAccessLayer.Interfaces;
using AuctionHouse.Test.DaoTests;
using AuctionHouse.WebAPI.BusinessLogic;
using AuctionHouse.WebAPI.IBusinessLogic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionHouse.Test.APITests
{
    [Collection("Sequential")]
    public class BidLogicAPITest
    {

        #region Properties
        private readonly Func<IDbConnection> _connectionFactory;
        private readonly IBidLogic _bidLogic;
        private readonly IWalletLogic _walletLogic;
        private readonly IAuctionLogic _auctionLogic;

        private readonly IAuctionDao _auctionDao;
        private readonly IBidDao _bidDao;
        private readonly IItemDao _itemDao;
        private readonly IUserDao _userDao;
        private readonly IWalletDao _walletDao;
        private readonly ITransactionDao _transactionDao;

        #endregion

        #region Constructor
        public BidLogicAPITest()
        {
            var cs = Environment.GetEnvironmentVariable("DatabaseConnectionString");
            _connectionFactory = () =>
            {
                var c = new SqlConnection(cs);
                c.Open();                       // hand callers an *OPEN* connection
                return c;
            };
            _transactionDao = new TransactionDAO(_connectionFactory);
            _walletDao = new WalletDAO(_connectionFactory, _transactionDao);
            _userDao = new UserDAO(_connectionFactory, _walletDao);
            _bidDao = new BidDAO(_connectionFactory, _userDao);
            _itemDao = new ItemDAO(_connectionFactory, _userDao);
            _auctionDao = new AuctionDAO(_connectionFactory, _bidDao, _itemDao, _userDao);
            //logics
            _walletLogic = new WalletLogic(_connectionFactory,_walletDao);
            _auctionLogic = new AuctionLogic(_connectionFactory,_auctionDao);
            _bidLogic = new BidLogic(_connectionFactory, _walletLogic,_auctionLogic,_bidDao);
            //Clean tables
            CleanAndBuild.CleanDB();
            //Generate test data
            CleanAndBuild.GenerateFreshTestDB();
        }

        #endregion

        #region Tests

        [Fact]
        public async Task PlaceBidAsync_ValidBid_ReturnsSuccessMessage()
        {
            // Arrange
            var auction = await _auctionDao.GetByIdAsync(1);
            var expectedAuctionVersion = auction.Version;
            //Bidder must not own the auction
            var bidder1 = await _userDao.GetByIdAsync(2);

            Bid bidder1_bid = new Bid
            {
                AuctionId = auction.AuctionID!.Value,
                Amount = 650,
                TimeStamp = DateTime.Now,
                User = bidder1
            };


            // Act
            var result_bidder1 = await _bidLogic.PlaceBidAsync(bidder1_bid, expectedAuctionVersion);

            var newauction = await _auctionLogic.GetAuctionByIdAsync(auction.AuctionID!.Value);
            // Assert


            Assert.Equal(1, newauction.AmountOfBids);
        }

        [Fact]
        public async Task PlaceBidAsync_ValidBids_ConcurrencyTest_ReturnsSuccessMessage()
        {
            // Arrange
            var auction = await _auctionDao.GetByIdAsync(1);
            var expectedAuctionVersion = auction.Version;
            //Bidder must not own the auction
            var bidder1 = await _userDao.GetByIdAsync(2);
            var bidder2 = await _userDao.GetByIdAsync(3);

            Bid bidder1_bid = new Bid
            {
                AuctionId = auction.AuctionID!.Value,
                Amount = 650,
                TimeStamp = DateTime.Now,
                User = bidder1
            };


            Bid bidder2_bid = new Bid
            {
                AuctionId = auction.AuctionID!.Value,
                Amount = 700,
                TimeStamp = DateTime.Now,
                User = bidder2
            };

            // Act
            var result_bidder1 = await _bidLogic.PlaceBidAsync(bidder1_bid, expectedAuctionVersion);

            var result_bidder2 = await _bidLogic.PlaceBidAsync(bidder2_bid, expectedAuctionVersion);

            var newauction = await _auctionLogic.GetAuctionByIdAsync(auction.AuctionID!.Value);
            // Assert


            Assert.Equal(1, newauction.AmountOfBids);
        }

        #endregion

    }
}
