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
        public Bid PlaceBid(int? auctionId, string userName, decimal amount)
        {
            WalletLogic walletLogic = new WalletLogic();
            Bid bid = new Bid(amount, DateTime.Now);
            walletLogic.subtractBidAmountFromTotalBalance(userName, amount);
            return bid;
        }


    }
    
    
}
