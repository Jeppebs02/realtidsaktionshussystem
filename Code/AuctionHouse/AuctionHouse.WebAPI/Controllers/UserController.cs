using Microsoft.AspNetCore.Mvc;
using AuctionHouse.DataAccessLayer.Interfaces;
using AuctionHouse.ClassLibrary.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AuctionHouse.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserDao _userDao;

        public UserController(IUserDao userDao)
        {
            _userDao = userDao;
        }

        // GET: api/<UserController>
        [HttpGet]
        public async Task<IEnumerable<User>> Get()
        {
            return await _userDao.GetAllAsync<User>();
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public async Task<User> Get(int id)
        {
            return await _userDao.GetByIdAsync<User>(id);
        }

        // POST api/<UserController>
        [HttpPost]
        public async Task<int> Post([FromBody] User user)
        {
            return await _userDao.InsertAsync(user);
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public async Task<bool> Put(int id, [FromBody] User user)
        {
            return await _userDao.UpdateAsync(user);
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public async Task<bool> Delete(User user)
        {
            return await _userDao.DeleteAsync(user);
        }
    }
}
