using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionHouse.ClassLibrary.Model
{
    public class User
    {



        #region Constructors
        public User(string userName, string password, string firstName, string lastName, string email, string phoneNumber, string address)
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
        }

        #endregion

        #region Properties
        public bool CantBuy { get; set; }
        public bool CantSell { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        #endregion
    }
}
