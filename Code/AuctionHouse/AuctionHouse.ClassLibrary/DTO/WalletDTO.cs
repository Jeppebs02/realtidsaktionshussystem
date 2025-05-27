using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using AuctionHouse.ClassLibrary.Model;

namespace AuctionHouse.ClassLibrary.DTO
{
    public class WalletDTO
    {
        #region Constructor
        public WalletDTO() { }
        public WalletDTO(decimal totalBalance, decimal reservedBalance, byte[]? version = null, int? walletId = null)
        {
            WalletId = walletId;
            TotalBalance = totalBalance;
            ReservedBalance = reservedBalance;
            Version = version;
        }
        #endregion

        #region Properties
        public int? WalletId { get; set; }
        public decimal TotalBalance { get; set; }
        public decimal ReservedBalance { get; set; }

        [Timestamp]
        public byte[] Version { get; set; }

        public List<Transaction>? Transactions { get; set; } = new List<Transaction>();

        #endregion

        #region Methods

        public decimal GetAvailableBalance()
        {

            return TotalBalance - ReservedBalance;

        }

        public void AddFunds(decimal amount)
        {
            TotalBalance += amount;
        }

        public bool reserveFunds(decimal amount)
        {
            if (amount <= GetAvailableBalance())
            {
                ReservedBalance += amount;
                return true;
            }
            return false;
        }

        public bool addTransaction(Transaction transaction)
        {
            try
            {
                Transactions.Add(transaction);
                return true;


            }
            catch (Exception ex)
            {
                return false;
            }


        }


        #endregion
    }
}
