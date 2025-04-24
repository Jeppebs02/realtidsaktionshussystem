using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionHouse.ClassLibrary
{
    public class Bid
    {
        #region Constructor
        public Bid(int amount, DateTime timeStamp)
        {
            Amount = amount;
            TimeStamp = timeStamp;
        }
        #endregion

        #region Properties
        public int Amount { get; set; }
        public DateTime TimeStamp { get; set; }
        #endregion

    }
}
