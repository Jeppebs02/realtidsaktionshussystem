using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionHouse.ClassLibrary
{
    public class Item
    {
        #region Constructor
        public Item(string name, string description, string category, byte[] imageData, string itemStatus)
        {
            Name = name;
            Description = description;
            Category = category;
            ImageData = imageData;
            ItemStatus = itemStatus;
        }
        #endregion

        #region Properties
        public string Name { get; set; }

        public string Description { get; set; }

        public string Category { get; set; }

        public byte[] ImageData { get; set; }

        public string ItemStatus { get; set; }
        #endregion
    }
}
