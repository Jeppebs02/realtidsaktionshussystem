using AuctionHouse.ClassLibrary.Model;
using AuctionHouse.WebAPI.IBusinessLogic;
using System.Data;

namespace AuctionHouse.WebAPI.BusinessLogic
{
    public class WalletLogic : IWalletLogic
    {
        public Task<bool> AddFundsAsync(int walletId, decimal amountToReserve, byte[] expectedVersion, IDbTransaction transaction = null)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ReleaseFundsAsync(int walletId, decimal amountToRelease, byte[] expectedVersion, IDbTransaction transaction = null)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ReserveFundsAsync(int walletId, decimal amountToReserve, byte[] expectedVersion, IDbTransaction transaction = null)
        {
            throw new NotImplementedException();
        }
    }
}
