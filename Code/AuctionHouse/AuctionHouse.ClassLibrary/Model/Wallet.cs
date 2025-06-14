﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AuctionHouse.ClassLibrary.Model
{
    public class Wallet
    {
        #region Constructors
        // Blank constructor required by Dapper
        public Wallet() { }
        public Wallet(decimal totalBalance, decimal reservedBalance, int userId, byte[]? version = null, int? walletId = null)
        {
            WalletId = walletId;
            TotalBalance = totalBalance;
            ReservedBalance = reservedBalance;
            Version = version;
            UserId = userId;
        }
        #endregion

        #region Properties
        public int? WalletId { get; set; }
        public decimal TotalBalance { get; set; }
        public decimal ReservedBalance { get; set; }

        [Timestamp]
        public byte[] Version { get; set; }

        public List<Transaction> Transactions { get; set; } = new List<Transaction>();
        [JsonIgnore]
        public int? UserId { get; set; }
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
