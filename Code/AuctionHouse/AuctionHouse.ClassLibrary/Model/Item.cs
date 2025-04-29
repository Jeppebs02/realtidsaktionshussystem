using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuctionHouse.ClassLibrary.Enum;

namespace AuctionHouse.ClassLibrary.Model
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

        [Required]
        [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters.")]
        public string Description { get; set; }

        public Category Category { get; set; }

        public byte[] ImageData { get; set; }

        public ItemStatus ItemStatus { get; set; }
        #endregion
    }
}
