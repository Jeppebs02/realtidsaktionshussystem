using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AuctionHouse.ClassLibrary.Model; //Fix this ask jeppe
using AuctionHouse.Requester;
using Microsoft.AspNetCore.Components;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net.Http;
using System.Text;

namespace AuctionHouse.WebSite.Pages.Auction
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public String? errorMessage { get; set; } = null;
        public Wallet userWallet { get; set; }

        [BindProperty]
        public User loggedInUser { get; set; }

        APIRequester _apiRequester;

        public AuctionHouse.ClassLibrary.Model.Auction? specificAuction { get; set; }

        [BindProperty(SupportsGet = true)]
        public int AuctionId { get; set; }


        public async Task<IActionResult> OnGetAsync()
        {
            Console.WriteLine($"Fetching auction with ID: {AuctionId}");

            try
            {
                // Load the page properties
                await loadPageProperties();


                // Error checks
                if (specificAuction != null)
                {
                    Console.WriteLine("Auction is set");
                }


                //Select a single auction based on the number entered in the url.

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
    


        public async Task<IActionResult> OnPostBidAsync(decimal amount)
        {

            // Load the page properties
            await loadPageProperties();

            Bid newBid = new Bid(AuctionId, amount, DateTime.Now, loggedInUser);
            byte[] expectedAuctionVersion = specificAuction.Version;
            newBid.ExpectedAuctionVersion = expectedAuctionVersion;

            // Check if the auction is null
            if (expectedAuctionVersion==null)
            {
                errorMessage = "Auction version is null";
            }

            // check if amount is valid
            if (amount <= 0)
            {
                errorMessage= "Bid amount must be greater than zero.";
            }
            else
            {
                
                var auctionId = specificAuction.AuctionID;
                
                if (newBid.Amount > userWallet.GetAvailableBalance()) {
                    errorMessage = "Insufficient funds.";

                }
                else 
                {


                    //remove userid

                    Wallet cleanedWallet = new Wallet
                    {
                        WalletId = userWallet.WalletId,
                        TotalBalance = userWallet.TotalBalance,
                        Version = userWallet.Version
                    };



                    var cleanedUser = new User
                    {
                        UserId = loggedInUser.UserId,
                        UserName = loggedInUser.UserName,
                        Password = loggedInUser.Password,
                        CantBuy = loggedInUser.CantBuy,
                        CantSell = loggedInUser.CantSell,
                        RegistrationDate = loggedInUser.RegistrationDate,
                        FirstName = loggedInUser.FirstName,
                        LastName = loggedInUser.LastName,
                        Email = loggedInUser.Email,
                        PhoneNumber = loggedInUser.PhoneNumber,
                        Address = loggedInUser.Address,
                        Wallet = cleanedWallet
                    };

                    var cleanedBid = new Bid
                    {
                        BidId = null,
                        AuctionId = specificAuction!.AuctionID.Value,
                        Amount = amount,
                        TimeStamp = DateTime.UtcNow,
                        User = cleanedUser,
                        ExpectedAuctionVersion = specificAuction.Version
                    };

                    //debug print
                    var json = JsonSerializer.Serialize(cleanedBid, new JsonSerializerOptions
                    {
                        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                        WriteIndented = true
                    });

                    Console.WriteLine(json);

                    var response = await _apiRequester.Post($"api/bid", cleanedBid);

                    errorMessage = response;
                }

            }
            // add auction to dummy list of auctions

            



            return Page();
        }


        public async Task<IActionResult> OnPostBuyOutAsync()
        {
            // Load the page properties
            await loadPageProperties();
            // Get the auction ID from the specific auction

            Console.WriteLine($"Trying to buy out auction: {specificAuction}");

            // Create a new bid with the buyout amount
            var buyoutBid = new Bid(AuctionId,specificAuction.BuyOutPrice, DateTime.Now, loggedInUser);

            Console.WriteLine($"Created bid: {buyoutBid}");

            if (buyoutBid.Amount > userWallet.GetAvailableBalance())
            {
                Console.WriteLine($"buyout bid: {buyoutBid} \n (must be greater than) \n {userWallet.GetAvailableBalance()}");
                errorMessage = "Insufficient funds.";

            }
            else
            {
                specificAuction.AuctionStatus = ClassLibrary.Enum.AuctionStatus.ENDED_SOLD;
                specificAuction.AddBid(buyoutBid);
                errorMessage = $"Bid with {buyoutBid.Amount} is good. You did a buyout";
            }

            return Page();
        }



        private async Task loadPageProperties()
        {
            _apiRequester = new APIRequester(new HttpClient());

            try
            {
                // Fetch and deserialize the specific auction
                string auctionJson = await _apiRequester.Get($"api/auction/{AuctionId}");
                specificAuction = JsonSerializer.Deserialize<AuctionHouse.ClassLibrary.Model.Auction>(
                    auctionJson,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );

                // Fetch and deserialize the logged-in user
                string userJson = await _apiRequester.Get("api/user/2");
                loggedInUser = JsonSerializer.Deserialize<User>(
                    userJson,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );

                userWallet = loggedInUser?.Wallet;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error during loadPageProperties: " + ex.Message);
                // optionally set errorMessage = ex.Message;
            }
        }


    }
}
