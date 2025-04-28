using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuctionHouse.ClassLibrary.Model;
using AuctionHouse.ClassLibrary.Enum;
using System;
using System.Collections.Generic;

namespace AuctionHouse.ClassLibrary.Stubs
{
    public static class AuctionTestData
    {
        public static List<Auction> GetTestAuctions()
        {
            var testAuctions = new List<Auction>();

            // Dummy image data
            byte[] dummyImage = new byte[] { 0x20, 0x20, 0x20 };

            // Create some items
            var sword = new Item("Legendary Sword", "An ancient sword with mystical powers.", Category.OTHER, dummyImage, ItemStatus.AVAILABLE);
            var painting = new Item("Sunset Painting", "A beautiful painting of a sunset.", Category.ART_ANTIQUES, dummyImage, ItemStatus.AVAILABLE);
            var car = new Item("Vintage Car", "A classic 1960s Mustang.", Category.VEHICLES, dummyImage, ItemStatus.AVAILABLE);
            var laptop = new Item("Gaming Laptop", "High-end gaming laptop with RTX 4090.", Category.ELECTRONICS, dummyImage, ItemStatus.AVAILABLE);
            var book = new Item("First Edition Book", "Rare first edition of a famous novel.", Category.BOOKS_MANUSCRIPTS, dummyImage, ItemStatus.AVAILABLE);

            // Create auctions
            var auction1 = new Auction(
                DateTime.Now.AddHours(-1),
                DateTime.Now.AddDays(1),
                100m,
                1000m,
                10m,
                true,
                sword
            );

            var auction2 = new Auction(
                DateTime.Now.AddMinutes(-30),
                DateTime.Now.AddHours(12),
                200m,
                1500m,
                20m,
                false,
                painting
            );

            var auction3 = new Auction(
                DateTime.Now.AddHours(-2),
                DateTime.Now.AddDays(2),
                5000m,
                25000m,
                500m,
                true,
                car
            );

            var auction4 = new Auction(
                DateTime.Now.AddMinutes(-10),
                DateTime.Now.AddHours(6),
                1200m,
                2200m,
                50m,
                true,
                laptop
            );

            var auction5 = new Auction(
                DateTime.Now.AddHours(-5),
                DateTime.Now.AddDays(3),
                50m,
                500m,
                5m,
                false,
                book
            );

            // Add some bids to the auctions
            auction1.AddBid(new Bid(110m, DateTime.Now.AddMinutes(-50)));
            auction1.AddBid(new Bid(130m, DateTime.Now.AddMinutes(-40)));

            auction3.AddBid(new Bid(5500m, DateTime.Now.AddHours(-1)));
            auction3.AddBid(new Bid(6000m, DateTime.Now.AddMinutes(-30)));

            auction4.AddBid(new Bid(1250m, DateTime.Now.AddMinutes(-5)));

            // Add auctions to list
            testAuctions.Add(auction1);
            testAuctions.Add(auction2);
            testAuctions.Add(auction3);
            testAuctions.Add(auction4);
            testAuctions.Add(auction5);

            return testAuctions;
        }
    }
}
