using Microsoft.AspNetCore.Mvc;
using AuctionHouse.ClassLibrary.Model;
using AuctionHouse.DataAccessLayer.Interfaces;
using AuctionHouse.WebAPI.IBusinessLogic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AuctionHouse.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BidController : ControllerBase
    {
        
        private readonly IBidLogic _bidLogic;

        public BidController(IBidLogic bidLogic)
        {
            // This is where we inject the IItemDao into the controller
            _bidLogic = bidLogic;
        }

        // GET: api/<BidController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<BidController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Bid>> Get(int id)
        {
            var bid = await _bidLogic.GetByIdAsync(id);

            if (bid is null) { 
                return NotFound();
            }

            return Ok(bid);
        }

        // POST api/<BidController>
        [HttpPost]
        public async Task<ActionResult<string>> Post([FromBody] Bid bid, byte[] expectedAuctionVersion)
        {
            var result = _bidLogic.PlaceBidAsync(bid, expectedAuctionVersion);

            if (result.Result == "Bid is not higher than current highest bid")
            {
                return BadRequest(result.Result);
                
            }
            else if (result.Result == "You have been BANNED from buying")
            {
                return BadRequest(result.Result);

            }
            else if (result.Result == "Bid placed successfully")
            {
                return Ok(result.Result);
            }
            else if (result.Result == "You dont have enough money in the wallet")
            {
                return BadRequest(result.Result);
            }
            else if (result.Result == "Auction has been updated by another user, please refresh the page")
            {
                return BadRequest(result.Result);
            }
            else if (result.Result == "Bid could not be placed")
            {
                return BadRequest(result.Result);
            }
            else if (result.Result == "Bid placed succesfully :)")
            {
                return Ok(result.Result);
            }
            else
            {
                throw new Exception(result.Result);
            }

        }

        // PUT api/<BidController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<BidController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
