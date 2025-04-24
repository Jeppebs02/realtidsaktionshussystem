using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionHouse.ClassLibrary
{
    public class User
    {

        #region Properties
        private bool cantBuy { get; set; }
        private bool cantSell { get; set; }
        private string UserName { get; set; }
        private string Password { get; set; }
        private DateTime RegistrationDate { get; set; }
        private string FirstName { get; set; }
        private string LastName { get; set; }
        private string Email { get; set; }
        private string PhoneNumber { get; set; }
        private string Address { get; set; }
        #endregion

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
            cantBuy = false;
            cantSell = false;
        }

        #endregion

    }
}
