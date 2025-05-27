using AuctionHouse.ClassLibrary.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionHouse.DataAccessLayer.Interfaces
{
    public interface IWalletDao : IGenericDao<Wallet>
    {

        /// <summary>
        /// Gets a specific user's wallet
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<Wallet> GetByUserId(int userId);

        Task<bool> ReserveFundsOptimisticallyAsync(int walletId, decimal amountToReserve, byte[] expectedVersion, IDbTransaction transaction = null);

        Task<byte[]> UpdateTotalBalanceAsync(Wallet entity);
    }
}
