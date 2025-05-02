using AuctionHouse.DataAccessLayer.Interfaces;
using AuctionHouse.ClassLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionHouse.Test.DaoTests
{
    public class AuctionDaoTest
    {

        private readonly IAuctionDao _auctionDao;

        [Fact]
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


        [Fact]
        public async Task GetAllByUserIDAsync_ShouldReturnAuctions_WhenUserIdExists()
        {
            // Arrange  
            int userId = 1;
            // Act  
            IEnumerable<Auction> auctions = await _auctionDao.GetAllByUserIDAsync(userId);

            IEnumerable<Auction> auctions2 = await _auctionDao.GetAllAsync<Auction>();
            // Assert  
            Assert.NotNull(auctions);

            foreach (var auction in auctions2)
            {
                if (auction.item.User.userId== userId)
                {

                }
            }

            //Get all auctions that the user has an item in.


        }


        [Fact]
        public async Task GetAllByBidsAsync_ShouldReturnAuctions_WhenUserIdExists()
        {
            // Arrange  
            int userId = 1;
            // Act  
            IEnumerable<Auction> auctions = await _auctionDao.GetAllByBidsAsync(userId);

            IEnumerable<Auction> auctions2 = await _auctionDao.GetAllByUserIDAsync(userId);
            // Assert  
            Assert.NotNull(auctions);
            //Does Auction have a userid?
        }



    }
}
