using AuctionHouse.ClassLibrary.Interfaces;
using Microsoft.AspNetCore.Mvc;
using AuctionHouse.ClassLibrary.Model;
using AuctionHouse.DataAccessLayer.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AuctionHouse.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuctionController : ControllerBase
    {

        private readonly IAuctionDao _auctionDao;

        public AuctionController(IAuctionDao auctionDao)
        {
            // This is where we inject the IItemDao into the controller
            _auctionDao = auctionDao;
        }



        // GET: api/<AuctionController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<AuctionController>/5
        [HttpGet("{id}")]
        public async Task<Auction> Get(int id)
        {
            return await _auctionDao.GetByIdAsync(id);
        }

        // POST api/<AuctionController>
        [HttpPost]
        public void Post([FromBody]string value)
        {
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
