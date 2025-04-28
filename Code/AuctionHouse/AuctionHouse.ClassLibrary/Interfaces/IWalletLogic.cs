using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionHouse.ClassLibrary.Interfaces
{
    public interface IWalletLogic
    {
        bool subtractBidAmountFromTotalBalance(string username, decimal amount);


    }
}
