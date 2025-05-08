using AuctionHouse.ClassLibrary.Model;
using System.Data;

namespace AuctionHouse.WebAPI.IBusinessLogic
{
    public interface IWalletLogic
    {
        Task<bool> ReserveFundsAsync(int walletId, decimal amountToReserve, byte[] expectedVersion, IDbTransaction transaction = null);

        Task<bool> AddFundsAsync(int walletId, decimal amountToReserve, byte[] expectedVersion, IDbTransaction transaction = null);

        Task<bool> ReleaseFundsAsync(int walletId, decimal amountToReserve, byte[] expectedVersion, IDbTransaction transaction = null);
    }
}
