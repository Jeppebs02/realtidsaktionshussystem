using AuctionHouse.ClassLibrary.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AuctionHouse.ClassLibrary
{
    public class Auction
    {
        #region Constructor
        public Auction(string auctionID, DateTime startTime, DateTime endTime, decimal startPrice, decimal buyOutPrice, decimal minimumBidIncrement)
        {
            this.AuctionID = auctionID;
            this.StartTime = startTime;
            this.EndTime = endTime;
            this.StartPrice = startPrice;
            this.BuyOutPrice = buyOutPrice;
            this.MinimumBidIncrement = minimumBidIncrement;
            this.AuctionStatus = AuctionStatus;
            this.Version = 1;
        }
        #endregion


        #region Properties
        public string AuctionID { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public decimal StartPrice { get; set; }
        public decimal BuyOutPrice { get; set; }
        public decimal MinimumBidIncrement { get; set; }
        public AuctionStatus AuctionStatus { get; set; }
        public int Version { get; set; }
        public String Notify { get; set; }
        #endregion
    }
}
