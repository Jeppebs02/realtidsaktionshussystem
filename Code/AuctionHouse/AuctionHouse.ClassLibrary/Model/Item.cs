using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using AuctionHouse.ClassLibrary.Enum;

namespace AuctionHouse.ClassLibrary.Model
{
    public class Item
    {
        #region Constructors

        // Blank constructor requied by Dapper
        public Item() { }
        public Item(User user, string name, string description, Category category, byte[] imageData)
        {
            User = user;
            Name = name;
            Description = description;
            Category = category;
            ImageData = imageData;
        }
        #endregion

        #region Properties
        public int? ItemId { get; set; }
        [JsonIgnore]
        public int? UserId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "´Description is required")]
        [StringLength(255, ErrorMessage = "Description cannot be longer than 255 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "An Category must be chosen")]
        public Category Category { get; set; }

        [Required(ErrorMessage = "An Image must be uploaded")]
        public byte[] ImageData { get; set; }

        [Required(ErrorMessage = "An User must be chosen")]
        public User User { get; set; }
        #endregion
    }
}
