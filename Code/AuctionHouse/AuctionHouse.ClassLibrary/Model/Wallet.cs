using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionHouse.ClassLibrary.Model
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

        public decimal AvailableBalance
        {
            get
            {
                return TotalBalance - ReservedBalance;
            }
        }

        public void AddFunds(decimal amount)
        {
            TotalBalance += amount;
        }

        public bool reserveFunds(decimal amount)
        {
            if (amount <= AvailableBalance)
            {
                ReservedBalance += amount;
                return true;
            }
            return false;
        }

    }
}
