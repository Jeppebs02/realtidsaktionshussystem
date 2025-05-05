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
        #region Constructors
        // Blank constructor required by Dapper
        public Bid() { }
        public Bid(int auctionId, decimal amount, DateTime timeStamp, User user, int? bidId = null)
        {
            AuctionId = auctionId;
            Amount = amount;
            TimeStamp = timeStamp;
            User = user;
            BidId = bidId;
        }

        #endregion

        #region Properties
        public int? BidId { get; set; }
        public int AuctionId { get; set; }
        public decimal Amount { get; set; }
        public DateTime TimeStamp { get; set; }

        [Required(ErrorMessage = "User is required")]
        public User User { get; set; }
        #endregion

        #region Methods

        public override string ToString()
        {
            return $"Bid: {Amount}, Time: {TimeStamp}, AuctionId: {AuctionId}";
        }

        #endregion
    }
}
