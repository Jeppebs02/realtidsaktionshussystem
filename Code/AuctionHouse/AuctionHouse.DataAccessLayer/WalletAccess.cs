using AuctionHouse.ClassLibrary.Model;

namespace AuctionHouse.DataAccessLayer
{
    public class WalletAccess : IWalletAccess
    {
        private static readonly Dictionary<string, Wallet> _wallets = new()
           {
               { "alice", new Wallet(2000, 200) },
               { "bob", new Wallet(1000, 0) } ,
           };

        public Wallet GetWalletForUser(string username)
        {
            if (_wallets.TryGetValue(username, out var wallet))
            {
                return wallet;
            }
            throw new KeyNotFoundException($"Wallet for user '{username}' not found.");
        }
    }
}
