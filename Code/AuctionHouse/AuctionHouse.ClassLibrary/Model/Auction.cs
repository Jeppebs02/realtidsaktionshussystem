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
        public Auction(DateTime startTime, DateTime endTime, decimal startPrice, decimal buyOutPrice, decimal minimumBidIncrement, bool notify, Item item)
        {
            StartTime = startTime;
            EndTime = endTime;
            StartPrice = startPrice;
            BuyOutPrice = buyOutPrice;
            MinimumBidIncrement = minimumBidIncrement;
            AuctionStatus = AuctionStatus.ACTIVE;
            Version = 1;
            Notify = notify;
            this.item = item;
            Bids = new List<Bid>();
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
        public bool Notify { get; set; }
        public Item item { get; set; }
        public List<Bid> Bids { get; set; }

        #endregion

        public void AddBid(Bid bid)
        {
            Bids.Add(bid);
        }
    }
}
