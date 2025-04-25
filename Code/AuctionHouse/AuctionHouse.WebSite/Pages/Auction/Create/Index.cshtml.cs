using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AuctionHouse.ClassLibrary.Enum;
using AuctionHouse.ClassLibrary.Model;
using Newtonsoft.Json;

namespace AuctionHouse.WebSite.Pages.CreateAuction
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public IFormFile? ImageFile { get; set; }

        public List<Category> Categories { get; set; } = new List<Category>();
        public void OnGet()
        {
            Categories = Enum.GetValues(typeof(Category)).Cast<Category>().ToList();
            Console.WriteLine("desadsa");
        }


        public async Task<IActionResult> OnPostCreateAuction(DateTime startTime, DateTime endTime, decimal startPrice, decimal buyOutPrice, decimal minumimBidIncrement, bool notify, string itemName, string itemDescription, Category category) {
            Item item;
            if (ImageFile == null || ImageFile.Length == 0)
            {
                ModelState.AddModelError("ImageFile", "Please upload an image file.");
                return Page();
            }else{
                using (var memoryStream = new MemoryStream())
                {
                    await ImageFile.CopyToAsync(memoryStream);
                    byte[] imageData = memoryStream.ToArray();
                    // Create the item with the provided data
                    item = new Item(itemName, itemDescription, category, imageData, ItemStatus.AVAILABLE);
                }
            }

            // Using full namespace because "Auction" is a namespace in this project
            AuctionHouse.ClassLibrary.Model.Auction auction = new AuctionHouse.ClassLibrary.Model.Auction(startTime, endTime, startPrice, buyOutPrice, minumimBidIncrement, notify, item);

            var JSONData = JsonConvert.SerializeObject(auction);

            Console.WriteLine("Auction Created: " + JSONData);


            return Page(); 
        }
    }
}
