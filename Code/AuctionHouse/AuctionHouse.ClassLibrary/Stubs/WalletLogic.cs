using AuctionHouse.ClassLibrary.Interfaces;
using AuctionHouse.ClassLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionHouse.ClassLibrary.Stubs
{
    public class WalletLogic : IWalletLogic
    {
        public bool subtractBidAmountFromTotalBalance(string username, decimal amount)
        {
            if (_wallets.TryGetValue(username.ToLower(), out var wallet))
            {
                return wallet.reserveFunds(amount);
            }
            return false;

        }

        public static readonly Dictionary<string, Wallet> _wallets = new()
           {
               { "alice", new Wallet(2000, 200) },
               
           };
    }
    
    
}
