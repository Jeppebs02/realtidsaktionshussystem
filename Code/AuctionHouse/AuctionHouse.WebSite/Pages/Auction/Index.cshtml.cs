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

        [BindProperty(SupportsGet = true)]
        public int? userId { get; set; }

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



        // Helpers to park the message in either ViewData (same request) or TempData (redirect)
        private IActionResult ShowModal(string message, bool redirect)
        {
            if (redirect)
            {
                TempData["PopupMessage"] = message;   // lives for one request after redirect
                return RedirectToPage(new { AuctionId });
            }

            ViewData["PopupMessage"] = message;       // lives only for this request
            return Page();
        }

        public async Task<IActionResult> OnPostBidAsync(decimal amount)
        {
            await loadPageProperties();


            // --- place bid ---
            var bid = new BidDTO(AuctionId, amount, DateTime.UtcNow, loggedInUser)
            { ExpectedAuctionVersion = specificAuction.Version };

            var apiResponse = await _apiRequester.Post("api/bid", bid);

            if (!string.IsNullOrWhiteSpace(apiResponse))
            {
                return ShowModal(apiResponse, redirect: true);
            }


            // success – no modal, just redirect
            return RedirectToPage(new { AuctionId });

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

            return RedirectToPage(new { AuctionId = this.AuctionId, userId = loggedInUser.UserId });

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


                var userIdStr = Request.Cookies["selectedUserId"];                // Use session storage to get userid
                if (string.IsNullOrEmpty(userIdStr))
                {
                    userIdStr = Request.Query["userId"]; // fallback via URL
                }

                if (!int.TryParse(userIdStr, out int selectedUserId))
                {

                    selectedUserId = 2; // fallback user (optional)

                }


                string userJson = await _apiRequester.Get($"api/user/{selectedUserId}");
                loggedInUser = JsonSerializer.Deserialize<UserDTO>(userJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                userWallet = loggedInUser?.Wallet;

            }
            catch (Exception ex)
            {
                Console.WriteLine("There was an error" + ex);
            }

        }

    }
}
