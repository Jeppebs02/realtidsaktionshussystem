using AuctionHouse.ClassLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionHouse.ClassLibrary.Interfaces
{
    public interface IBidLogic
    {
        Bid PlaceBid(int auctionId, decimal amount, User user);

    }
}
