using AuctionHouse.ClassLibrary.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionHouse.ClassLibrary.Model
{
    public class Transaction
    {


        #region Constructors
        // Blank constructor required by Dapper
        public Transaction() { }

        public Transaction (TransactionType type, decimal amount, DateTime timeStamp, string? description)
        {
            Type = type;
            Amount = amount;
            TimeStamp = timeStamp;
            Description = description;
        }

        #endregion 

        #region Properties
        public int? TransActionId { get; set; }

        [Required(ErrorMessage = "TransactionType is required")]
        public TransactionType Type { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "TimeStamp is required")]
        public DateTime TimeStamp { get; set; }

        public string? Description { get; set; }

        #endregion
    }
}
