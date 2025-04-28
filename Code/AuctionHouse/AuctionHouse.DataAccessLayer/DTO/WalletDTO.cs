using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionHouse.DataAccessLayer.DTO
{
    public class WalletDTO
    {
        public record WalletDto(decimal Available, decimal Reserved);
    }
}
