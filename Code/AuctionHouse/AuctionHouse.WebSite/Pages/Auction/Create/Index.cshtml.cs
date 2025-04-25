using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AuctionHouse.ClassLibrary.Enum;
using AuctionHouse.ClassLibrary.Model;
using Newtonsoft.Json;

namespace AuctionHouse.WebSite.Pages.CreateAuction
{
    public class IndexModel : PageModel
    {

        // BindProperty takes attributes from the cshtml page and binds them to the properties in this class
        [BindProperty]
        public IFormFile? ImageFile { get; set; }

        //Error message to show on the page.
        [BindProperty]
        public String? errorMessage { get; set; } = null;

        public List<Category> Categories { get; set; } = new List<Category>();
        public void OnGet()
        {
            Categories = Enum.GetValues(typeof(Category)).Cast<Category>().ToList();
            Console.WriteLine("Page loaded");
        }


        public async Task<IActionResult> OnPostCreateAuctionAsync(DateTime startTime, DateTime endTime, decimal startPrice, decimal buyOutPrice, decimal minimumBidIncrement, string itemName, string itemDescription, Category category)
        {
            Item item;
            Console.WriteLine("Item object declared");
            try
            {

                Console.WriteLine("About to check img file");
                if (ImageFile == null || ImageFile.Length == 0)
                {
                    ModelState.AddModelError("ImageFile", "Please upload an image file.");
                    return Page();
                }

                else
                {
                    Console.WriteLine("Image file checked, about to load to byte[]");
                    using (var memoryStream = new MemoryStream())
                    {
                        Console.WriteLine("Copying to memory stream");
                        await ImageFile.CopyToAsync(memoryStream);
                        Console.WriteLine("Creating bytearray");
                        byte[] imageData = memoryStream.ToArray();
                        // Create the item with the provided data
                        Console.WriteLine("Creating item Object");
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
