using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AuctionHouse.ClassLibrary.Stubs;
using AuctionHouse.WebAPI;
using System.Collections.Generic;
using AuctionHouse.ClassLibrary.Enum;

namespace AuctionHouse.WebSite.Pages.Homepage
{
    public class IndexModel : PageModel
    {
        public List<AuctionHouse.ClassLibrary.Model.Auction> TestAuctions { get; set; }
        public List<AuctionHouse.ClassLibrary.Model.Auction> FilteredAuctions { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }

        [BindProperty(SupportsGet = true)]
        public Category? SelectedCategory { get; set; }

        public void OnGet()
        {
            // Load test data
            TestAuctions = AuctionHouse.ClassLibrary.Stubs.AuctionTestData.GetTestAuctions();

            // Start with all auctions
            FilteredAuctions = TestAuctions;

            // Apply search term
            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                FilteredAuctions = FilteredAuctions
                    .Where(a => a.item.Name.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            // Apply category filter
            if (SelectedCategory.HasValue)
            {
                FilteredAuctions = FilteredAuctions
                    .Where(a => a.item.Category == SelectedCategory)
                    .ToList();
            }
        }

        public List<AuctionHouse.ClassLibrary.Model.Auction> GetActiveAuctions()
        {
            // TODO - make logic to fetch data from WebAPI
            return null;
        }
    }
}


