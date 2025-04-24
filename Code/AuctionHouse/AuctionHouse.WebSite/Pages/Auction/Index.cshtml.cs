using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AuctionHouse.ClassLibrary.Model;
using Microsoft.AspNetCore.Components;

namespace AuctionHouse.WebSite.Pages.Auction
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public String? errorMessage { get; set; } = null;

        public void OnGet()
        {
        }



        public async Task<IActionResult> OnPostBid(decimal amount)
        {


            Bid newBid = new Bid(amount, DateTime.Now);

            if(amount <= 0)
            {
                errorMessage= "Bid amount must be greater than zero.";
            }
            else
            {
                errorMessage = $"Bid with {newBid.Amount} is good.";

            }

                return Page();
        }

    }
}
