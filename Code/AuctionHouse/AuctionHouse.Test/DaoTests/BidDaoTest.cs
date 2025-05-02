using AuctionHouse.ClassLibrary.Model;
using AuctionHouse.DataAccessLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionHouse.Test.DaoTests
{
    public class BidDaoTest
    {
        private readonly IBidDao _bidDao;
        private readonly IUserDao _userDao;



        [Fact]
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
            User user = _userDao.GetByIdAsync<User>(1).Result;
            Bid bid = new Bid(1, 500, DateTime.Now, user);
            // Act  
            int id = await _bidDao.InsertAsync(bid);
            // Assert  
            Assert.True(id > 5);
        }
    }
}
