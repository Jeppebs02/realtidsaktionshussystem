using AuctionHouse.ClassLibrary.Interfaces;
using AuctionHouse.ClassLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionHouse.ClassLibrary.Stubs
{
    public class BidLogic : IBidLogic
    {

        //Place bid might need some work in terms of subtracting from wallet 
        public Bid PlaceBid(int auctionId, decimal amount, User user)
        {
            Bid bid = new Bid(auctionId, amount, DateTime.Now, user);
            return bid;
        }



    }
    
    
}
