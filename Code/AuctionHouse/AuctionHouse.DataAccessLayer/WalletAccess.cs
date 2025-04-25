using AuctionHouse.ClassLibrary.Model;
using AuctionHouse.ClassLibrary.Stubs;

namespace AuctionHouse.DataAccessLayer
{
    public class WalletAccess : IWalletAccess
    {
        

        public Wallet GetWalletForUser(string username)
        {
            if (WalletLogic._wallets.TryGetValue(username, out var wallet))
            {
                return wallet;
            }
            throw new KeyNotFoundException($"Wallet for user '{username}' not found.");
        }
    }
}
