using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionHouse.DataAccessLayer.Interfaces
{
    public interface IConnectionFactory
    {
        Task<IDbConnection> CreateOpenConnectionAsync();

    }
}
