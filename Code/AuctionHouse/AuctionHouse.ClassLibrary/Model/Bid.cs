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
        public Bid(int auctionId, decimal amount, DateTime timeStamp)
        {
            this.AuctionId = auctionId;
            this.Amount = amount;
            this.TimeStamp = timeStamp;
        }

        public Bid(decimal amount, DateTime timeStamp)
        {
            this.Amount = amount;
            this.TimeStamp = timeStamp;
        }
        #endregion

        #region Properties
        public int AuctionId { get; set; }
        public decimal Amount { get; set; }
        public DateTime TimeStamp { get; set; }
        #endregion

        #region Methods

        public override string ToString()
        {
            return $"Bid: {Amount}, Time: {TimeStamp}, AuctionId: {AuctionId}";
        }

        #endregion
    }
}
