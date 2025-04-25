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


        public async Task<IActionResult> OnPostCreateAuctionAsync(DateTime startTime, DateTime endTime, decimal startPrice, decimal buyOutPrice, decimal minimumBidIncrement, string itemName, string itemDescription, Category category)
        {
            Item item;
            try
            {
                if (ImageFile == null || ImageFile.Length == 0)
                {
                    ModelState.AddModelError("ImageFile", "Please upload an image file.");
                    return Page();
                }
                else
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await ImageFile.CopyToAsync(memoryStream);
                        byte[] imageData = memoryStream.ToArray();
                        // Create the item with the provided data
                        item = new Item(itemName, itemDescription, category, imageData, ItemStatus.AVAILABLE);
                    }
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("ImageFile", "Error processing the image file: " + ex.Message);
                return Page();
            }

            // Using full namespace because "Auction" is a namespace in this project
            AuctionHouse.ClassLibrary.Model.Auction auction = new AuctionHouse.ClassLibrary.Model.Auction(startTime, endTime, startPrice, buyOutPrice, minimumBidIncrement, false, item);

            var JSONData = JsonConvert.SerializeObject(auction);

            Console.WriteLine("Auction Created: " + JSONData);


            return Page();
        }
    }
}
