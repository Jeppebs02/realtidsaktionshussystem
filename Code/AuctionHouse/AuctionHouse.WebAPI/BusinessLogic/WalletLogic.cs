using AuctionHouse.ClassLibrary.Model;
using AuctionHouse.WebAPI.IBusinessLogic;

namespace AuctionHouse.WebAPI.BusinessLogic
{
    public class WalletLogic : IWalletLogic
    {
        public Task<bool> AddFundsAsync(Wallet wallet, decimal amount)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ReleaseFundsAsync(Wallet wallet, decimal amount)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ReserveFundsAsync(Wallet wallet, decimal amount)
        {
            throw new NotImplementedException();
        }
    }
}
