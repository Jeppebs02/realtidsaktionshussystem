using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AuctionHouse.ClassLibrary;
using AuctionHouse.WebAPI;

namespace AuctionHouse.WebSite.Pages.Auction.Active
{
    public class IndexModel : PageModel
    {
        public List<AuctionHouse.ClassLibrary.Model.Auction> ActiveAuctions { get; set; } 

        public void OnGet()
        {
            // Load active auctions from DB or service
            //ActiveAuctions = AuctionHouse.WebAPI.GetActiveAuctions(); // just an example
            new List<AuctionHouse.ClassLibrary.Model.Auction> ActiveAuctions(AuctionHouse.ClassLibrary.Stubs.ActiveAuctions.get(TestList));
        }

        public List<AuctionHouse.ClassLibrary.Model.Auction> GetActiveAuctions()
        {
            //TODO - make logic - deserialse json from WebAPI
            return null;
        }
    }
}

