using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuctionHouse.ClassLibrary.Enum;

namespace AuctionHouse.ClassLibrary
{
    public class Item
    {
        #region Constructor
        public Item(string name, string description, Category category, byte[] imageData, ItemStatus itemStatus)
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

        public Category Category { get; set; }

        public byte[] ImageData { get; set; }

        public ItemStatus ItemStatus { get; set; }
        #endregion
    }
}
