using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AuctionHouse.ClassLibrary.Stubs;
using AuctionHouse.WebAPI;
using System.Collections.Generic;

namespace AuctionHouse.WebSite.Pages.Auction.Active
{
    public class IndexModel : PageModel
    {
        public List<AuctionHouse.ClassLibrary.Model.Auction> TestAuctions { get; set; }

        public void OnGet()
        {
            // Load test data
            TestAuctions = AuctionHouse.ClassLibrary.Stubs.AuctionTestData.GetTestAuctions();
        }

        public List<AuctionHouse.ClassLibrary.Model.Auction> GetActiveAuctions()
        {
            // TODO - make logic to fetch data from WebAPI
            return null;
        }
    }
}


