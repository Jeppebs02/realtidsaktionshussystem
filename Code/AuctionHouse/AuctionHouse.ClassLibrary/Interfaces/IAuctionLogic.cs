using AuctionHouse.ClassLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionHouse.ClassLibrary.Interfaces
{
    public interface IAuctionLogic
    {
        Auction getAuctionByID(int auctionId);
        bool addBidToAuction(int auctionId, string userName, Bid bid);
    }
}
