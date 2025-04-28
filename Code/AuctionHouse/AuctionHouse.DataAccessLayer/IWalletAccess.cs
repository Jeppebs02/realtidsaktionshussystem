using AuctionHouse.ClassLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace AuctionHouse.DataAccessLayer
{
    public interface IWalletAccess
    {
        Wallet GetWalletForUser(string username);
        Wallet Deposit(string username, decimal amount);
    }
}
