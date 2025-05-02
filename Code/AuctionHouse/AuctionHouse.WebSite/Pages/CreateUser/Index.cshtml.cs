using AuctionHouse.ClassLibrary.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace AuctionHouse.WebSite.Pages.CreateUser
{
    public class IndexModel : PageModel
    {

        //Error message to show on the page.
        [BindProperty]
        public String? errorMessage { get; set; } = null;
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostCreateUserAsync(string userName, string password, string firstName, string lastName, string email, string phoneNumber, string address)
        {
            User newUser = new User(userName, password, firstName, lastName, email, phoneNumber, address, new Wallet(0, 0,1));

            var JSONData = JsonConvert.SerializeObject(newUser);

            return Page();
        }

    }
}
