using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AuctionHouse.ClassLibrary.Enum;
using AuctionHouse.ClassLibrary.Model;
using System.Text.Json;
using AuctionHouse.ClassLibrary.DTO;
using AuctionHouse.Requester;
using System.Text.Json.Serialization;
using System.Runtime.CompilerServices;

namespace AuctionHouse.WebSite.Pages.CreateAuction
{
    public class IndexModel : PageModel
    {
        //IAuctionDao auctionDao = new AuctionDaoStub();

        // BindProperty takes attributes from the cshtml page and binds them to the properties in this class
        [BindProperty]
        public IFormFile? ImageFile { get; set; }

        //Error message to show on the page.
        [BindProperty]
        public String? errorMessage { get; set; } = null;

        private APIRequester _apiRequester { get; set; }

        public List<Category> Categories { get; set; } = new List<Category>();
        public void OnGet()
        {
            Categories = Enum.GetValues(typeof(Category)).Cast<Category>().ToList();
            _apiRequester = new APIRequester(new HttpClient());
            Console.WriteLine("Categories loaded");
            var userIdStr = Request.Cookies["selectedUserId"];
            if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out int userId))
            {
                ModelState.AddModelError("", "No valid user selected.");
            }
        }


        public async Task<IActionResult> OnPostCreateAuctionAsync(DateTime startTime, DateTime endTime, decimal startPrice, decimal buyOutPrice, decimal minimumBidIncrement, string itemName, string itemDescription, Category category)
        {
            Console.WriteLine("Creat auction pressed");
            _apiRequester = new APIRequester(new HttpClient());

            // Check if the model state is valid
            if (!ModelState.IsValid)
            {
                Console.WriteLine("Model state is not valid");
                foreach (var entry in ModelState)
                {
                    var key = entry.Key;
                    var errors = entry.Value.Errors;
                    foreach (var error in errors)
                    {
                        Console.WriteLine($" - Field: {key}, Error: {error.ErrorMessage}");
                    }
                }

                return Page();
            }

            var userIdStr = Request.Cookies["selectedUserId"];
            if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out int userId))
            {
                ModelState.AddModelError("", "No valid user selected.");
                return Page();
            }

            string userJson = await _apiRequester.Get($"api/user/{userId}");
            var user = JsonSerializer.Deserialize<User>(userJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            user.Password = "passwd"; // Clear the password for security reasons

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


                        item = new Item(user, itemName, itemDescription, category, imageData);
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
            if(!IsValidPricing(minimumBidIncrement, startPrice, buyOutPrice))
            {
                Console.WriteLine("Bid increment validation failed");

                return Page();
            }
            else
            {

                // Using full namespace because "Auction" is a namespace in this project
                AuctionHouse.ClassLibrary.Model.Auction auction = new AuctionHouse.ClassLibrary.Model.Auction(startTime, endTime, startPrice, buyOutPrice, minimumBidIncrement, false, item);

                Console.WriteLine("About to post to API to create auction");
                string respose = await _apiRequester.Post("api/auction", auction);


                return RedirectToPage("Index");


            }


        }


        public bool IsValidPricing(decimal minimumBidIncrement, decimal startPrice, decimal buyOutPrice)
        {
            // Check if the bid increment is valid
            if (startPrice <= 1 || minimumBidIncrement <= 1)
            {
                errorMessage = "The minimum bid increment and start price must be greater than 1";
                return false;
            }
            if(buyOutPrice < startPrice)
            {
                errorMessage = "The buyout price must be greater than the start price";
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
