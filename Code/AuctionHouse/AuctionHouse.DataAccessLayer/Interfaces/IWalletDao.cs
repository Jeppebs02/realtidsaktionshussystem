using AuctionHouse.ClassLibrary.Model;
using System;
using System.Collections.Generic;
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



    }
}
