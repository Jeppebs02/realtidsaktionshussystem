using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AuctionHouse.ClassLibrary.Model; //Fix this ask jeppe
using Microsoft.AspNetCore.Components;
using System.Runtime.CompilerServices;
using AuctionHouse.ClassLibrary.Stubs;
using AuctionHouse.ClassLibrary.Interfaces;
using Newtonsoft.Json;
using AuctionHouse.ClassLibrary.Stubs;

namespace AuctionHouse.WebSite.Pages.Auction
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public String? errorMessage { get; set; } = null;

        private IWalletLogic? _walletLogic;//TODO Implement Interface instead
        public Wallet userWallet { get; set; } = null;
        string username;
        public AuctionHouse.ClassLibrary.Model.Auction specificAuction { get; set; } = null;

        [BindProperty(SupportsGet = true)]
        public int AuctionId { get; set; }


        public async Task<IActionResult> OnGetAsync()
        {
            Console.WriteLine($"Fetching auction with ID: {AuctionId}");

            try
            {
                // Wallet logic
                username = User.Identity?.Name ?? "alice";
                WalletLogic.wallets.TryGetValue(username, out Wallet userWallet);

                //Get list of stubbed auctions
                List<AuctionHouse.ClassLibrary.Model.Auction> auctions = AuctionTestData.GetTestAuctions();

                //Select a single auction based on the number entered in the url.
                specificAuction = auctions[AuctionId];

                if (specificAuction == null)
                {
                    Console.WriteLine($"Auction with ID {AuctionId} not found.");
                    // If no auction is found for the ID, return a 404 Not Found result
                    return NotFound();
                }

                // If auction found, the page will be rendered with the Auction data :)
                return Page();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"exception: {ex}");

                return NotFound(); // Or RedirectToPage("/Error");
            }
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
                
                var auctionId = 1; // This should be replaced with the actual auction ID
                
                if (bidlogic.PlaceBid(auctionId, username, amount).Amount > userWallet.AvailableBalance) {
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
