using AuctionHouse.ClassLibrary.Model;
using AuctionHouse.DataAccessLayer.Interfaces;
using Dapper;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AuctionHouse.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {

        private readonly IWalletDao _walletDao;

        public WalletController(IWalletDao walletDao)
        {
             _walletDao = walletDao;
        }

        // GET: api/<WalletController>
        [HttpGet]
        public async Task<IEnumerable<Wallet>> Get()
        {
            return await _walletDao.GetAllAsync<Wallet>();
        }

        // GET api/<WalletController>/5
        [HttpGet("{id}")]
        public async Task<Wallet> Get(int id)
        {
            return await _walletDao.GetByIdAsync<Wallet>(id);
        }

        // POST api/<WalletController>
        [HttpPost]
        public string Post([FromBody]string value)
        {
            return "Brug noget andet i stedet for, evt. fra user";
        }

        // PUT api/<WalletController>/5
        [HttpPut("{id}")]
        public async Task<bool> Put([FromBody]Wallet wallet)
        {
            return await _walletDao.UpdateAsync(wallet);
        }

        // DELETE api/<WalletController>/5
        [HttpDelete("{id}")]
        public async Task<bool> Delete(Wallet wallet)
        {
            return await _walletDao.DeleteAsync(wallet);
        }
    }
}
