using AuctionHouse.ClassLibrary.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AuctionHouse.ClassLibrary.Model
{
    public class Auction
    {

        #region Constructor
        public Auction(DateTime startTime, DateTime endTime, decimal startPrice, decimal buyOutPrice, decimal minimumBidIncrement, bool notify, Item item, int auctionId = -1, byte[] version = null)
        {
            StartTime=startTime;
            EndTime=endTime;
            StartPrice=startPrice;
            BuyOutPrice=buyOutPrice;
            MinimumBidIncrement=minimumBidIncrement;
            AuctionStatus=AuctionStatus.ACTIVE;
            Version=version;
            Notify = notify;
            this.item=item;
            Bids = new List<Bid>();
            if (auctionId != -1)
            {
                AuctionID = auctionId;
            }
            else
            {
                AuctionID = null;
            }
        }
        #endregion


        #region Properties
        public int? AuctionID { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public decimal StartPrice { get; set; }
        public decimal BuyOutPrice { get; set; }
        public decimal MinimumBidIncrement { get; set; }
        public AuctionStatus AuctionStatus { get; set; }
        [Timestamp]
        public byte[] Version { get; set; }
        public bool Notify { get; set; }
        public Item item { get; set; }
        public List<Bid> Bids { get; set; }

        #endregion


        #region Methods
        public bool AddBid(Bid bid)
        {
            try
            {
                Bids.Add(bid);
                return true;
            }
            catch
            {
                return false;
            }

        }

        public Bid GetHighestBid()
        {
            Bid highestBid = null;
            if (Bids.Count == 0)
            {
                return null;
            }

            foreach (Bid currentBid in Bids)
            {
                if (highestBid == null || currentBid.Amount > highestBid.Amount)
                {
                    highestBid = currentBid;
                }
            }

            return highestBid;
        }

        //toString method
        public override string ToString()
        {
            return $"Auction ID: {AuctionID}, Start Time: {StartTime}, End Time: {EndTime}, Start Price: {StartPrice}, Buy Out Price: {BuyOutPrice}, Minimum Bid Increment: {MinimumBidIncrement}, Auction Status: {AuctionStatus}, Item: {item.Name}";
        }


        #endregion


    }
}
