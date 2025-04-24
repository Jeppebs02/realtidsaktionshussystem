using AuctionHouse.ClassLibrary.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionHouse.ClassLibrary
{
    public class Transaction
    {


     #region Constructors

        public Transaction (TransactionType type, Decimal amount, DateTime timeStamp, string? description)
        {
            Type = type;
            Amount = amount;
            TimeStamp = timeStamp;
            Description = description;
        }

        #endregion 

        #region Properties

        public TransactionType Type { get; set; }
        public decimal Amount { get; set; }
        public DateTime TimeStamp { get; set; }
        public string? Description { get; set; }

        #endregion
    }
}
