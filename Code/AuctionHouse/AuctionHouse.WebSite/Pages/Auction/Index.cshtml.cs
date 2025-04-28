using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AuctionHouse.ClassLibrary.Model;
using Microsoft.AspNetCore.Components;
using System.Runtime.CompilerServices;
using AuctionHouse.ClassLibrary.Stubs;
using AuctionHouse.ClassLibrary.Interfaces;
using Newtonsoft.Json;

namespace AuctionHouse.WebSite.Pages.Auction
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public String? errorMessage { get; set; } = null;
        private IWalletLogic? _walletLogic;//TODO Implement Interface instead

        public void OnGet()
        {
        }



        public async Task<IActionResult> OnPostBid(decimal amount)
        {
            _walletLogic = new WalletLogic(); 
            BidLogic bidlogic = new();

            Bid newBid = new Bid(amount, DateTime.Now);

            if(amount <= 0)
            {
                errorMessage= "Bid amount must be greater than zero.";
            }
            else
            {
                var username = User.Identity?.Name ?? "alice";
                var auctionId = 1; // This should be replaced with the actual auction ID
                if (bidlogic.PlaceBid(auctionId, username, amount)!=null) {
                    errorMessage = "Insufficient funds.";

                }
                else 
                { 
                errorMessage = $"Bid with {newBid.Amount} is good.";
                }

            }
            var JSONData = JsonConvert.SerializeObject(newBid);

            return Page();
        }

    }
}
