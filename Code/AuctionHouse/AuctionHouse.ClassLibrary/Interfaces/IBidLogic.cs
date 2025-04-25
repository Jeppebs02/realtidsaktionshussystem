using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionHouse.ClassLibrary.Interfaces
{
    internal interface IBidLogic
    {
        decimal GetBidAmount(string username, decimal amount);
    }
}
