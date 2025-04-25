using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AuctionHouse.ClassLibrary.Model;
using AuctionHouse.WebAPI;

namespace AuctionHouse.WebSite.Pages.Auction.Active
{
    public class IndexModel : PageModel
    {
        public List<AuctionHouse.ClassLibrary.Model.Auction> ActiveAuctions { get; set; }

        public void OnGet()
        {
            // Load active auctions from DB or service
            ActiveAuctions = AuctionHouse.WebAPI.GetActiveAuctions(); // just an example
        }
    }

