using AuctionHouse.ClassLibrary.Model;

namespace AuctionHouse.WebAPI.IBusinessLogic
{
    public interface IWalletLogic
    {
        Task<bool> ReserveFundsAsync(Wallet wallet, decimal amount);

        Task<bool> AddFundsAsync(Wallet wallet, decimal amount);

        Task<bool> ReleaseFundsAsync(Wallet wallet, decimal amount);
    }
}
