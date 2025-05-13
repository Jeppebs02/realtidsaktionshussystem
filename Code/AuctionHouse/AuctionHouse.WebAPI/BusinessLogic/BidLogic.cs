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

        public async Task<Bid> GetByIdAsync(int id)
        {
            return await _bidDao.GetByIdAsync(id);
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

            // Is your bid higher than last bid + minimum increment?
            if (currentHighestBid.Result != null)
            {
                if (amountToBid < currentHighestBid.Result.Amount + auctionToBidOn.Result.MinimumBidIncrement)
                {
                    return "Bid is not higher than current highest bid + minimum increment";
                }
            }

            // Do you have enough available balance in your wallet?
            if (userWallet.GetAvailableBalance() < amountToBid)
            {
                return "You dont have enough money in the wallet";
            }

            // our transaction so we can do all operations in one transaction
            using var transaction = _connectionFactory().BeginTransaction();

            // Is the expected auction version the same as the current version in the db?
            if (!_auctionLogic.UpdateAuctionOptimistically(auctionToBidOn.Result.AuctionID!.Value, expectedAuctionVersion, transaction, 1).Result)
            {
                transaction.Rollback();
                return "Auction has been updated by another user, please refresh the page";
            }

            // Did the new bid go through?
            if (currentHighestBid.Id>=_bidDao.InsertBidAsync(bid, transaction).Result)
            {
                transaction.Rollback();
                return "Bid could not be placed";
            }

            // Is the bid amount equal or higher than buyout? if so, set auction to sold
            if (bid.Amount >= auctionToBidOn.Result.BuyOutPrice)
            {
                if (!_auctionLogic.UpdateAuctionStatusOptimistically(auctionToBidOn.Result.AuctionID!.Value, expectedAuctionVersion, ClassLibrary.Enum.AuctionStatus.ENDED_SOLD, transaction).Result)
                {
                    transaction.Rollback();
                    return "Error with auction status update";
                }
            }

            // Did the wallet update go through?
            if (!_walletLogic.ReserveFundsAsync(userWallet.WalletId!.Value, bid.Amount, expectedWalletVersion, transaction).Result)
            {
                transaction.Rollback();
                return "Error with wallet version, please try again.";
            }

            transaction.Commit();
            return "Bid placed succesfully :)";


        }


    }
}
