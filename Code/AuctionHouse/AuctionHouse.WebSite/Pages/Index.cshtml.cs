using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AuctionHouse.ClassLibrary.Enum;
using Newtonsoft.Json;
using AuctionHouse.Requester;

namespace AuctionHouse.WebSite.Pages.Homepage
{
    public class IndexModel : PageModel
    {
        public List<AuctionHouse.ClassLibrary.Model.Auction> Auctions { get; set; }
        public List<AuctionHouse.ClassLibrary.Model.Auction> FilteredAuctions { get; set; } = new();

        private APIRequester _apiRequester;

        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }

        [BindProperty(SupportsGet = true)]
        public Category? SelectedCategory { get; set; }

        public async Task OnGetAsync()
        {

            //get json response from API
            _apiRequester = new APIRequester(new HttpClient());

            var json = await _apiRequester.Get("api/auction");
            Console.WriteLine(json);
            // Deserialize the JSON response into a list of auctions

            var auctions = JsonConvert.DeserializeObject<List<AuctionHouse.ClassLibrary.Model.Auction>>(json);


            // Load test data
            Auctions = auctions;

            // Start with all auctions
            FilteredAuctions = Auctions;

            // Apply search termz
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


