using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AuctionHouse.ClassLibrary.Model; //Fix this ask jeppe
using Microsoft.AspNetCore.Components;
using System.Runtime.CompilerServices;
using System.Text.Json;
using AuctionHouse.ClassLibrary.DTO;
using AuctionHouse.Requester;
using System.Text.Json.Serialization;

namespace AuctionHouse.WebSite.Pages.Auction
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public String? errorMessage { get; set; } = null;

        public WalletDTO userWallet { get; set; }

        [BindProperty]
        public UserDTO loggedInUser { get; set; }

        List<AuctionHouse.ClassLibrary.Model.Auction> auctions;

        public AuctionHouse.ClassLibrary.Model.Auction? specificAuction { get; set; }

        [BindProperty(SupportsGet = true)]
        public int AuctionId { get; set; }

        private APIRequester _apiRequester { get; set; }


        public async Task<IActionResult> OnGetAsync()
        {
            Console.WriteLine($"Fetching auction with ID: {AuctionId}");

            try
            {
                // Load the page properties
                await loadPageProperties();


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

            BidDTO newBid = new BidDTO(AuctionId, amount, DateTime.Now, loggedInUser);

            if (amount <= 0)
            {
                errorMessage = "Bid amount must be greater than zero.";
            }
            else
            {

                var auctionId = specificAuction.AuctionID;

                if (newBid.Amount > userWallet.GetAvailableBalance())
                {
                    errorMessage = "Insufficient funds.";
                }
                else
                {
                    var json = JsonSerializer.Serialize(newBid, new JsonSerializerOptions
                    {
                        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                        WriteIndented = true
                    });

                    Console.WriteLine(json);

                    newBid.ExpectedAuctionVersion = specificAuction.Version;

                    var response = await _apiRequester.Post($"api/bid", newBid);

                    errorMessage = response;
                }
            }
            // refresh the url
            return Page();
        }


        public async Task<IActionResult> OnPostBuyOutAsync()
        {
            // Load the page properties
            await loadPageProperties();
            // Get the auction ID from the specific auction

            Console.WriteLine($"Trying to buy out auction: {specificAuction}");

            // Create a new bid with the buyout amount
            var buyoutBid = new BidDTO(AuctionId, specificAuction.BuyOutPrice, DateTime.Now, loggedInUser);

            Console.WriteLine($"Created bid: {buyoutBid}");

            if (buyoutBid.Amount > userWallet.GetAvailableBalance())
            {
                Console.WriteLine($"buyout bid: {buyoutBid} \n (must be greater than) \n {userWallet.GetAvailableBalance()}");
                errorMessage = "Insufficient funds.";

            }

            return Page();
        }



        private async Task loadPageProperties()
        {
            // Instanciating API Requester
            _apiRequester = new APIRequester(new HttpClient());

            try
            {
                string auctionJson = await _apiRequester.Get($"api/auction/{AuctionId}");

                specificAuction = JsonSerializer.Deserialize<AuctionHouse.ClassLibrary.Model.Auction>(auctionJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                // Fetch and deserialize the logged-in user
                string userJson = await _apiRequester.Get("api/user/2");
                loggedInUser = JsonSerializer.Deserialize<UserDTO>(
                    userJson,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );

                userWallet = loggedInUser?.Wallet;
            }
            catch (Exception ex)
            {
                Console.WriteLine("There was an error" + ex);
            }

        }

    }
}
