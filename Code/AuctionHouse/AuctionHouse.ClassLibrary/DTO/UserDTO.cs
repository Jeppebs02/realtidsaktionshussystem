using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using AuctionHouse.ClassLibrary.Model;

namespace AuctionHouse.ClassLibrary.DTO
{
    public class UserDTO
    {
        #region Constructor
        public UserDTO() { }
        public UserDTO(string userName, string firstName, string lastName, string email, string phoneNumber, string address, WalletDTO? walletDTO, string? password)
        {
            UserName = userName;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
            Address = address;
            RegistrationDate = DateTime.Now;
            CantBuy = false;
            CantSell = false;
            Wallet = walletDTO;
        }

#endregion

        #region Properties
        public int? UserId { get; set; }

        public bool CantBuy { get; set; }

        public bool CantSell { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [StringLength(50, ErrorMessage = "Username cant be more than 50 Characters")]
        public string UserName { get; set; }

        public string? Password { get; set; } = null;

        [Required(ErrorMessage = "Registration date is required")]
        public DateTime RegistrationDate { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, ErrorMessage = "First name cant be more than 50 Characters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50, ErrorMessage = "Last name cant be more than 50 Characters")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "E-mail is required")]
        [StringLength(50, ErrorMessage = "E-mail cant be more than 50 Characters")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [StringLength(50, ErrorMessage = "First name cant be more than 50 Characters")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [StringLength(50, ErrorMessage = "Address cant be more than 50 Characters")]
        public string Address { get; set; }

        public WalletDTO? Wallet { get; set; }

        #endregion
    }
}
