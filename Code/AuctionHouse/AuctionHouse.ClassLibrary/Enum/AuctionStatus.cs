using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionHouse.ClassLibrary.Enum
{
    public enum AuctionStatus
    {
        SCHEDULED,
        ACTIVE, 
        ENDED_SOLD,
        ENDED_UNSOLD,
        CANCELLED
    }
}
