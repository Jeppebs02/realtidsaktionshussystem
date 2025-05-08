using System.Data;
using AuctionHouse.ClassLibrary.Model;
using AuctionHouse.DataAccessLayer.Interfaces;
using AuctionHouse.WebAPI.IBusinessLogic;

namespace AuctionHouse.WebAPI.BusinessLogic
{
    public class BidLogic : IBidLogic
    {
        private readonly IAuctionLogic _auctionLogic;
        private readonly IBidDao _bidDao;
        private readonly IWalletLogic _walletLogic;
        private readonly Func<IDbConnection> _connectionFactory;


        public BidLogic(Func<IDbConnection> connectionFactory, IWalletLogic walletLogic, IAuctionLogic auctionLogic, IBidDao bidDao) {
            _auctionLogic = auctionLogic;
            _bidDao = bidDao;
            _walletLogic = walletLogic;
            _connectionFactory = connectionFactory;
        }


        public async Task<string> PlaceBidAsync(Bid bid, byte[] expectedAuctionVersion)
        {

            // Data from http request
            var userWallet = bid.User.Wallet;
            var expectedWalletVersion = userWallet.Version;
            var user = bid.User;
            var amountToBid = bid.Amount;

            // Data from db
            var auctionToBidOn = _auctionLogic.GetAuctionByIdAsync(bid.AuctionId);
            var currentHighestBid = _bidDao.GetLatestByAuctionId(bid.AuctionId);

            Task.WaitAll(auctionToBidOn, currentHighestBid);

            // Can you account buy?
            if (user.CantBuy)
            {
                return "You have been BANNED from buying";
            }

            // Is new bid higher than current highest bid?
            if(currentHighestBid.Result != null)
            {
                if (amountToBid <= currentHighestBid.Result.Amount)
                {
                    return "Bid is not higher than current highest bid";
                }
            }

            // Do you have enough available balance in your wallet?
            if (userWallet.GetAvailableBalance() < amountToBid)
            {
                return "You dont have enough money in the wallet";
            }

            _auctionLogic.UpdateAuctionOptimistically(auctionToBidOn.Result, expectedAuctionVersion, null, 1);




        }


    }
}
