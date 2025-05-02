using AuctionHouse.ClassLibrary.Model;
using AuctionHouse.ClassLibrary.Stubs;
using AuctionHouse.ClassLibrary.Interfaces;

namespace AuctionHouse.ClassLibrary.Interfaces
{
    public class WalletAccess : IWalletAccess
    {
        public Wallet Deposit(string username, decimal amount)
        {
            if (amount <= 0) throw new ArgumentException("Amount must be positive");

            if (!WalletLogic.wallets.TryGetValue(username.ToLower(), out var w))
                w = WalletLogic.wallets[username.ToLower()] = new Wallet(0, 0, 1);

            w.TotalBalance +=amount;
            return w;
        }

        public Wallet GetWalletForUser(string username)
        {
            if (WalletLogic.wallets.TryGetValue(username, out var wallet))
            {
                return wallet;
            }
            throw new KeyNotFoundException($"Wallet for user '{username}' not found.");
        }
    }
}
