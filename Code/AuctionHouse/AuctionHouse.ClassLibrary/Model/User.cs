using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionHouse.ClassLibrary.Model
{
    public class User
    {
        #region Constructors
        public User(string userName, string password, string firstName, string lastName, string email, string phoneNumber, string address, Wallet? wallet)
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
            Wallet = wallet;
        }

        #endregion

        #region Properties
        public int? userId { get; set; }

        public bool CantBuy { get; set; }

        public bool CantSell { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [StringLength(50, ErrorMessage = "Username cant be more than 50 Characters")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(50, ErrorMessage = "Password cant be more than 255 Characters")]
        public string Password { get; set; }

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

        public Wallet Wallet { get; set; }

        #endregion
    }
}
