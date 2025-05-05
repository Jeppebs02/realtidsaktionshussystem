using AuctionHouse.ClassLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionHouse.DataAccessLayer.Interfaces
{
    public interface ITransactionDao : IGenericDao<Transaction>
    {

        /// <summary>
        /// Gets all transactions belonging to a given wallet
        /// </summary>
        /// <param name="walletId"></param>
        /// <returns></returns>
        Task<IEnumerable<Transaction>> GetAllByWalletId(int walletId);


        Task<bool> DeleteByWalletId(int walletId);



    }
}
