using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AuctionHouse.ClassLibrary.Model; //Fix this ask jeppe
using Microsoft.AspNetCore.Components;
using System.Runtime.CompilerServices;
using AuctionHouse.ClassLibrary.Stubs;
using AuctionHouse.ClassLibrary.Interfaces;
using Newtonsoft.Json;
using AuctionHouse.DataAccessLayer;

namespace AuctionHouse.WebSite.Pages.Auction
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public String? errorMessage { get; set; } = null;

        private IWalletLogic? _walletLogic;//TODO Implement Interface instead
        private IWalletAccess _walletAccess;
        public Wallet userWallet { get; set; }
        
        public string username;

        public AuctionHouse.ClassLibrary.Model.Auction? specificAuction { get; set; }

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


                // Error checks
                if(auctions != null)
                {
                    Console.WriteLine("list of auctions is set");
                }


                //Select a single auction based on the number entered in the url.
                specificAuction = auctions[AuctionId];
                if (specificAuction != null) {
                    Console.WriteLine($"Specific auction is set:{specificAuction.ToString()}");
                
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
            username = User.Identity?.Name ?? "alice";
            _walletLogic = new WalletLogic();
            _walletAccess = new WalletAccess();

            userWallet = _walletAccess.GetWalletForUser(username);


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
            // add auction to dummy list of auctions

            
            var JSONData = JsonConvert.SerializeObject(newBid);

            return Page();
        }

    }
}
