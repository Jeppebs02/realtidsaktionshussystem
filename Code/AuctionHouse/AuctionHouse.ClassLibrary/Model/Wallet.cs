using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionHouse.ClassLibrary.Model
{
    public class Wallet
    {
        #region Constructor
        public Wallet(decimal totalBalance, decimal reservedBalance, byte[] version = null)
        {
            TotalBalance = totalBalance;
            ReservedBalance = reservedBalance;
            Version = version;
            Transactions = new List<Transaction>();
        }
        #endregion

        #region Properties
        public decimal TotalBalance { get; set; }
        public decimal ReservedBalance { get; set; }

        [Timestamp]
        public byte[] Version { get; set; }

        public List<Transaction> Transactions { get; set; }
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

        public bool addTransaction(Transaction transaction) {
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
