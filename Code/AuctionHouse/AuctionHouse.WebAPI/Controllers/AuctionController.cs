using AuctionHouse.WebAPI.IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using AuctionHouse.ClassLibrary.Model;
using AuctionHouse.DataAccessLayer.Interfaces;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AuctionHouse.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuctionController : ControllerBase
    {

        private readonly IAuctionLogic _auctionLogic;

        public AuctionController(IAuctionLogic auctionLogic)
        {
            // This is where we inject the IItemDao into the controller
            
            _auctionLogic = auctionLogic;
        }



        // GET: api/<AuctionController>
        [HttpGet]
        public async Task<ActionResult<List<Auction>>> Get()
        {
            return Ok(await _auctionLogic.GetAllActiveAuctionsAsync());
        }

        // GET api/<AuctionController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Auction>> Get(int id)
        { 
            return Ok(await _auctionLogic.GetAuctionByIdAsync(id));
        }

        // POST api/<AuctionController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Auction entity)
        {
            Console.WriteLine("Recieved post to create an auction from a client");
            bool ans = await _auctionLogic.CreateAuctionAsync(entity);
            if (ans)
            {
                return Ok();
            }
            else
            {
                return BadRequest("Failed to create auction");
            }
        }

        // PUT api/<AuctionController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<AuctionController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
