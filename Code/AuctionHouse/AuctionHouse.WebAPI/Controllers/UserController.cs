using Microsoft.AspNetCore.Mvc;
using AuctionHouse.ClassLibrary.Model;
using AuctionHouse.WebAPI.IBusinessLogic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AuctionHouse.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserLogic _userLogic;

        public UserController(IUserLogic userLogic)
        {
            _userLogic = userLogic;
        }

        // GET: api/<UserController>
        [HttpGet]
        public async Task<IEnumerable<User>> Get()
        {
            return await _userLogic.GetAllUsersAsync();
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public async Task<User> Get(int id)
        {
            return await _userLogic.GetUserByIdAsync(id);
        }

        // POST api/<UserController>
        [HttpPost]
        public async Task<int> Post([FromBody] User user)
        {
            return await _userLogic.CreateUserAsync(user);
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public async Task<bool> Put(int id, [FromBody] User user)
        {
            return await _userLogic.UpdateUserAsync(user);
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public async Task<bool> Delete(User user)
        {
            return await _userLogic.DeleteUserAsync(user);
        }
    }
}
