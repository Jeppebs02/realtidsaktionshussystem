using Microsoft.AspNetCore.Mvc;
using AuctionHouse.ClassLibrary.Model;
using AuctionHouse.DataAccessLayer.Interfaces;
using AuctionHouse.WebAPI.IBusinessLogic;
using AuctionHouse.ClassLibrary.DTO;

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

            if (bid is null)
            {
                return NotFound();
            }

            return Ok(bid);
        }

        // POST api/<BidController>
        [HttpPost]
        public async Task<ActionResult<string>> Post([FromBody] BidDTO bidDTO)
        {

            Bid bid = convertFromBidDTO(bidDTO);

            var result = _bidLogic.PlaceBidAsync(bid, bid.ExpectedAuctionVersion);

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


        private Bid convertFromBidDTO(BidDTO bidDTO)
        {
            //public Wallet(decimal totalBalance, decimal reservedBalance, int userId, byte[]? version = null, int? walletId = null)
            Wallet wallet = new Wallet(bidDTO.User.Wallet.TotalBalance, bidDTO.User.Wallet.ReservedBalance, bidDTO.User.UserId!.Value, bidDTO.User.Wallet.Version, bidDTO.User.Wallet.WalletId);

            //public User(string userName, string password, string firstName, string lastName, string email, string phoneNumber, string address, Wallet? wallet)
            User user = new User(bidDTO.User.UserName, null, bidDTO.User.FirstName, bidDTO.User.LastName, bidDTO.User.Email, bidDTO.User.PhoneNumber, bidDTO.User.Address, wallet);
            user.UserId = bidDTO.User.UserId;

            //int auctionId, decimal amount, DateTime timeStamp, User user, int? bidId = null)
            Bid bid = new Bid(bidDTO.AuctionId, bidDTO.Amount, bidDTO.TimeStamp, user);
            bid.ExpectedAuctionVersion = bidDTO.ExpectedAuctionVersion;

            return bid;

        }

    }


}
