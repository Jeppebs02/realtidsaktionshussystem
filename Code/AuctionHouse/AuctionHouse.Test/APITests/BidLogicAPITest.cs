﻿using AuctionHouse.ClassLibrary.Model;
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
            _bidDao = new BidDAO(_connectionFactory);
            _itemDao = new ItemDAO(_connectionFactory);
            _auctionDao = new AuctionDAO(_connectionFactory, _bidDao, _itemDao, _userDao);
            //logics
            _walletLogic = new WalletLogic(_connectionFactory,_walletDao);
            _auctionLogic = new AuctionLogic(_connectionFactory,_auctionDao, _itemDao);
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
            var result_bidder1 = _bidLogic.PlaceBidAsync(bidder1_bid, expectedAuctionVersion);

            var result_bidder2 = _bidLogic.PlaceBidAsync(bidder2_bid, expectedAuctionVersion);

            Task.WaitAll(result_bidder1, result_bidder2);

            var newauction = await _auctionLogic.GetAuctionByIdAsync(auction.AuctionID!.Value);
            // Assert


            Assert.Equal(1, newauction.AmountOfBids);
            Assert.Equal("Bid placed succesfully :)", result_bidder1.Result);
            Assert.Equal("Auction has been updated by another user, please refresh the page", result_bidder2.Result);
        }
        [Fact]
        public async Task PlaceBidAsync_ValidWallet_ConcurrencyTest_ReturnsSuccessMessage()
        {
            // Arrange
            var auction1 = await _auctionDao.GetByIdAsync(1);
            var auction2 = await _auctionDao.GetByIdAsync(3);
            var expectedAuctionVersion1 = auction1.Version;
            var expectedAuctionVersion2 = auction2.Version;
            //Bidder must not own the auction
            var bidder1 = await _userDao.GetByIdAsync(2);
            
            

            Bid bid1 = new Bid
            {
                AuctionId = auction1.AuctionID!.Value,
                Amount = 650,
                TimeStamp = DateTime.Now,
                User = bidder1
            };


            Bid bid2 = new Bid
            {
                AuctionId = auction2.AuctionID!.Value,
                Amount = 1000,
                TimeStamp = DateTime.Now,
                User = bidder1
            };

            // Act
            var result_bid1 = _bidLogic.PlaceBidAsync(bid1, expectedAuctionVersion1);

            var result_bid2 = _bidLogic.PlaceBidAsync(bid2, expectedAuctionVersion2);

            Task.WaitAll(result_bid1, result_bid2);

            var newauction1 = await _auctionLogic.GetAuctionByIdAsync(auction1.AuctionID!.Value);
            var newauction2 = await _auctionLogic.GetAuctionByIdAsync(auction2.AuctionID!.Value);
            // Assert


            Assert.Equal(1, newauction1.AmountOfBids);
            Assert.Equal(0, newauction2.AmountOfBids);
            Assert.Equal("Bid placed succesfully :)", result_bid1.Result);
            Assert.Equal("Error with wallet version, please try again.", result_bid2.Result);
        }

        #endregion

    }
}
