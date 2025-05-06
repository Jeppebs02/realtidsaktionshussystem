using AuctionHouse.ClassLibrary.Interfaces;
using Microsoft.AspNetCore.Mvc;
using AuctionHouse.ClassLibrary.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AuctionHouse.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BidController : ControllerBase
    {
        private readonly IBidLogic _bidLogic;

        // GET: api/<BidController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<BidController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<BidController>
        [HttpPost]
        public void Post([FromBody] Bid bid)
        {
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
