using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AuctionHouse.ClassLibrary.Enum;
using AuctionHouse.ClassLibrary.Model;
using Newtonsoft.Json;
using AuctionHouse.ClassLibrary.Stubs;

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
            Console.WriteLine("Categories loaded");
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

            // Validate the auction dates
            if (!IsValidDate(startTime, endTime))
            {

                Console.WriteLine("Date validation failed");
                
                return Page();
            }
            if(!IsValidBidIncrement(minimumBidIncrement, startPrice))
            {
                Console.WriteLine("Bid increment validation failed");

                return Page();
            }
            else
            {

                // Using full namespace because "Auction" is a namespace in this project
                AuctionHouse.ClassLibrary.Model.Auction auction = new AuctionHouse.ClassLibrary.Model.Auction(startTime, endTime, startPrice, buyOutPrice, minimumBidIncrement, false, item);

                var JSONData = JsonConvert.SerializeObject(auction);

                auction.AuctionID = AuctionTestData.testAuctions.Count + 1; // Giv nyt id
                AuctionTestData.testAuctions.Add(auction);

                Console.WriteLine($"Auction Created with data: {JSONData}");


                return Page();


            }


        }


        public bool IsValidBidIncrement(decimal minimumBidIncrement, decimal startPrice)
        {
            // Check if the bid increment is valid
            if (startPrice <= 1 || minimumBidIncrement <= 1)
            {
                errorMessage = "The minimum bid increment and start price must be greater than 1";
                return false;
            }
            return true;
        }


        public bool IsValidDate(DateTime startDate, DateTime endDate)
        {
            // Check if the date is in the past
            TimeSpan timeSpan = endDate - startDate;

            Console.WriteLine($"Start date is: {startDate}");
            Console.WriteLine($"End date is: {endDate}");
            Console.WriteLine($"timespan is: {timeSpan.TotalHours}");

            if (timeSpan.TotalHours < 1.0)
            {
                errorMessage = "The Auction duration must exceed 1 hour";
                return false;
            }

            return true;
        }
    }



}
