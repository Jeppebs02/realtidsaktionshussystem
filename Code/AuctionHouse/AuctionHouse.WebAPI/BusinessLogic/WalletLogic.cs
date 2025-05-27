using AuctionHouse.ClassLibrary.Model;
using AuctionHouse.WebAPI.IBusinessLogic;
using System.Data;
using AuctionHouse.DataAccessLayer.Interfaces;

namespace AuctionHouse.WebAPI.BusinessLogic
{
    public class WalletLogic : IWalletLogic
    {
        private readonly Func<IDbConnection> _connectionFactory;
        private readonly IWalletDao _walletDao;

        public WalletLogic(Func<IDbConnection> connectionFactory, IWalletDao walletDao)
        {
            _connectionFactory = connectionFactory;
            _walletDao = walletDao;
        }
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
            return _walletDao.ReserveFundsOptimisticallyAsync(walletId, amountToReserve, expectedVersion, transaction);
        }
    }
}
