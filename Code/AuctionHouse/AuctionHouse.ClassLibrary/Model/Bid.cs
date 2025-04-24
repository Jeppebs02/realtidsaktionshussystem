using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionHouse.ClassLibrary.Model
{
    public class Bid
    {
        #region Constructor
        public Bid(decimal amount, DateTime timeStamp)
        {
            this.Amount = amount;
            this.TimeStamp = timeStamp;
        }
        #endregion

        #region Properties
        public decimal Amount { get; set; }
        public DateTime TimeStamp { get; set; }
        #endregion

    }
}
