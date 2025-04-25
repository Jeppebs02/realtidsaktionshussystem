using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AuctionHouse.ClassLibrary.Enum;

namespace AuctionHouse.WebSite.Pages.CreateAuction
{
    public class IndexModel : PageModel
    {
        public List<Category> Categories { get; set; } = new List<Category>();
        public void OnGet()
        {
            Categories = Enum.GetValues(typeof(Category)).Cast<Category>().ToList();
        }
    }
}
