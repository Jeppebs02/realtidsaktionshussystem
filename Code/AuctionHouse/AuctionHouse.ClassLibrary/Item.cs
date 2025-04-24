using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionHouse.ClassLibrary
{
    public class Item
    {
        #region
        public string Name { get; set; }

        public string Description { get; set; }

        public string Category { get; set; }

        public List<byte[]> ImageData { get; set; }

        public string ItemStatus { get; set; }
        #endregion
    }
}
