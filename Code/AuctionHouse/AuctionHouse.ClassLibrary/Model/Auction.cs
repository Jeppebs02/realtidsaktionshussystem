using AuctionHouse.ClassLibrary.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AuctionHouse.ClassLibrary.Model
{
    public class Auction
    {
        #region Constructor
        public Auction(string auctionID, DateTime startTime, DateTime endTime, decimal startPrice, decimal buyOutPrice, decimal minimumBidIncrement)
        {
            AuctionID = auctionID;
            StartTime = startTime;
            EndTime = endTime;
            StartPrice = startPrice;
            BuyOutPrice = buyOutPrice;
            MinimumBidIncrement = minimumBidIncrement;
            AuctionStatus = AuctionStatus;
            Version = 1;
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
        public string Notify { get; set; }
        #endregion
    }
}
