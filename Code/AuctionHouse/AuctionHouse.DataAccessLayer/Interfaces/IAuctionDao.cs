using AuctionHouse.ClassLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionHouse.DataAccessLayer.Interfaces
{
    public interface IAuctionDao : IGenericDao<Auction>
    {
        IEnumerable<Auction> GetWithinDateRange(DateTime startDate, DateTime endDate);
    }
}
