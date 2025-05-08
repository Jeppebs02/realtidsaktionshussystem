using AuctionHouse.ClassLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionHouse.DataAccessLayer.Interfaces
{
    public interface IItemDao : IGenericDao<Item>
    {
        /// <summary>
        /// Gets all items by a specific user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<List<Item>> GetAllByUserId(int id);


    }
}
