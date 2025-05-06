using System.Data;
using AuctionHouse.ClassLibrary.Model;
using AuctionHouse.DataAccessLayer.Interfaces;
using AuctionHouse.WebAPI.IBusinessLogic;

namespace AuctionHouse.WebAPI.BusinessLogic
{
    public class BidLogic : IBidLogic
    {
        private readonly IAuctionDao _auctionDao;
        private readonly IBidDao _bidDao;
        private readonly IWalletDao _walletDao;
        private readonly IDbConnection _dbConnection;


        public BidLogic() {}
        public Task<bool> PlaceBidAsync(Bid bid)
        {
            throw new NotImplementedException();
        }


    }
}
