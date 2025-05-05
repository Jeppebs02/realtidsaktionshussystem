using AuctionHouse.DataAccessLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using AuctionHouse.ClassLibrary.Model;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AuctionHouse.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {


        private readonly IItemDao _itemDao;

        public ItemController(IItemDao itemDao)
        {
            // This is where we inject the IItemDao into the controller
            _itemDao = itemDao;
        }




        // GET: api/<ItemController>
        [HttpGet]
        // This needs to be async so we can use await on the frontend
        public async Task<IEnumerable<Item>> Get()
        {
            // Same deal here, use await because we return Tasks.
            return await _itemDao.GetAllAsync<Item>();
        }

        // GET api/<ItemController>/5
        [HttpGet("{id}")]
        public async Task<Item> Get(int id)
        {
            return await _itemDao.GetByIdAsync<Item>(id);
        }

        // POST api/<ItemController>
        [HttpPost]
        public async Task<int> Post([FromBody]Item item)
        {
            return await _itemDao.InsertAsync(item);
        }

        // PUT api/<ItemController>/5
        [HttpPut("{id}")]
        public async Task<bool> Put(int id, [FromBody]Item item)
        {
            return await _itemDao.UpdateAsync(item);
        }

        // DELETE api/<ItemController>/5
        [HttpDelete("{id}")]
        public async Task<bool> Delete([FromBody]Item item)
        {
            return await _itemDao.DeleteAsync(item);
        }
    }
}
