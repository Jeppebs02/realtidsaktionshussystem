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

        List<AuctionHouse.ClassLibrary.Model.Auction> auctions;

        BidLogic bidlogic;

        public AuctionHouse.ClassLibrary.Model.Auction? specificAuction { get; set; }

        [BindProperty(SupportsGet = true)]
        public int AuctionId { get; set; }


        public async Task<IActionResult> OnGetAsync()
        {
            Console.WriteLine($"Fetching auction with ID: {AuctionId}");

            try
            {
                // Load the page properties
                loadPageProperties();


                // Error checks
                if (auctions != null)
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

            // Load the page properties
            loadPageProperties();

            Bid newBid = new Bid(amount, DateTime.Now);

            if(amount <= 0)
            {
                errorMessage= "Bid amount must be greater than zero.";
            }
            else
            {
                
                var auctionId = specificAuction.AuctionID; // This should be replaced with the actual auction ID
                
                if (newBid.Amount > userWallet.AvailableBalance) {
                    errorMessage = "Insufficient funds.";

                }
                else 
                { 
                    errorMessage = $"Bid with {newBid.Amount} is good.";
                    _walletLogic.subtractBidAmountFromTotalBalance(username, newBid.Amount);
                    specificAuction.AddBid(newBid);
                }

            }
            // add auction to dummy list of auctions

            
            var JSONData = JsonConvert.SerializeObject(newBid);

            return Page();
        }


        public async Task<IActionResult> OnPostBuyOut()
        {
            // Load the page properties
            loadPageProperties();
            // Get the auction ID from the specific auction
            var auctionId = specificAuction.AuctionID;
            Console.WriteLine($"Trying to buy out auction: {specificAuction}");

            // Create a new bid with the buyout amount
            var buyoutBid = new Bid(specificAuction.BuyOutPrice, DateTime.Now);

            Console.WriteLine($"Created bid: {buyoutBid}");

            if (buyoutBid.Amount > userWallet.AvailableBalance)
            {
                Console.WriteLine($"buyout bid: {buyoutBid} \n (must be greater than) \n {userWallet.AvailableBalance}");
                errorMessage = "Insufficient funds.";

            }
            else
            {
                specificAuction.AuctionStatus = ClassLibrary.Enum.AuctionStatus.ENDED_SOLD;
                _walletLogic.subtractBidAmountFromTotalBalance(username, specificAuction.BuyOutPrice);
                specificAuction.AddBid(buyoutBid);
                errorMessage = $"Bid with {buyoutBid.Amount} is good. You did a buyout";
            }

            return Page();
        }



        private void loadPageProperties()
        {
            // We need to refetch the list of auctions when using the post method
            auctions = AuctionTestData.GetTestAuctions();
            //Then we set the property specificAuction.
            specificAuction = auctions[AuctionId];

            bidlogic = new();


            username = User.Identity?.Name ?? "alice";
            _walletLogic = new WalletLogic();
            _walletAccess = new WalletAccess();

            userWallet = _walletAccess.GetWalletForUser(username);




        }

    }
}
