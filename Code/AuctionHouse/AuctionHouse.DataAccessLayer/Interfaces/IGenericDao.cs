using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionHouse.DataAccessLayer.Interfaces
{
    public interface IGenericDao<T>
    {
        Task<int> InsertAsync(Task t);
        Task<bool>UpdateAsync(Task t);
        Task<bool> DeleteAsync(Task t);
        Task<List<T>> GetAllAsync<T>();
        Task<T?> GetByIdAsync<T>(int id);

    }
}
