using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionHouse.ClassLibrary
{
    public class Wallet
    {
        #region Constructor
        public Wallet(decimal totalBalance, decimal reservedBalance)
        {
            TotalBalance = totalBalance;
            ReservedBalance = reservedBalance;
        }
        #endregion

        #region Properties
        public decimal TotalBalance { get; set; }
        public decimal ReservedBalance { get; set; }
        #endregion



    }
}
