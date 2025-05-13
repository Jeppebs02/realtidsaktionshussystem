using System.Data;
using System.Data.SqlClient;
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
            var auctionToBidOn = await _auctionLogic.GetAuctionByIdAsync(bid.AuctionId);
            var currentHighestBid = await _bidDao.GetLatestByAuctionId(bid.AuctionId);

            //Task.WaitAll(auctionToBidOn, currentHighestBid);

            // Can you account buy?
            if (user.CantBuy)
            {
                return "You have been BANNED from buying";
            }

            // Is new bid higher than current highest bid?
            if (currentHighestBid != null)
            {
                if (amountToBid <= currentHighestBid.Amount)
                {
                    return "Bid is not higher than current highest bid";
                }
            }

            // Is your bid higher than last bid + minimum increment?
            if (currentHighestBid != null)
            {
                if (amountToBid < currentHighestBid.Amount + auctionToBidOn.MinimumBidIncrement)
                {
                    return "Bid is not higher than current highest bid + minimum increment";
                }
            }

            // Do you have enough available balance in your wallet?
            if (userWallet.GetAvailableBalance() < amountToBid)
            {
                return "You dont have enough money in the wallet";
            }



            IDbConnection? connectionForTransaction = null; // Declare here for finally block
            IDbTransaction? transaction = null;

            try
            {
                connectionForTransaction = _connectionFactory();
                //if (connectionForTransaction.State != ConnectionState.Open) await connectionForTransaction.OpenAsync();
                transaction = connectionForTransaction.BeginTransaction();

                // 1. Update Auction Optimistically and get the NEW version
                byte[]? newVersionFromFirstUpdate = await _auctionLogic.UpdateAuctionOptimistically(
                    auctionToBidOn.AuctionID!.Value,
                    expectedAuctionVersion,
                    transaction,
                    1);

                if (newVersionFromFirstUpdate == null) // Optimistic lock failed
                {
                    // No need to manually rollback, 'using var transaction' will if not committed.
                    // But explicit is fine too.
                    transaction.Rollback(); // Handled by using if exception or no commit
                    return "Auction has been updated by another user, please refresh the page";
                }

                // 2. Insert the Bid
                var insertedBidId = await _bidDao.InsertBidAsync(bid, transaction);
                if (insertedBidId <= 0) // Or based on whatever InsertBidAsync returns on failure
                {
                    return "Bid could not be placed"; // Transaction will be rolled back by using
                }
                // The check `currentHighestBid!=null && currentHighestBid.BidId>=insertedBidId` is still potentially problematic
                // and might not be necessary if InsertBidAsync is reliable. Consider removing or making it more robust.

                // 3. Buyout Condition
                if (bid.Amount >= auctionToBidOn.BuyOutPrice)
                {
                    // We USE the newVersionFromFirstUpdate for the next optimistic lock.
                    // NO NEED to call _auctionLogic.GetAuctionByIdAsync here.
                    byte[] versionForStatusUpdate = newVersionFromFirstUpdate;

                    bool statusUpdated = await _auctionLogic.UpdateAuctionStatusOptimistically(
                        auctionToBidOn.AuctionID!.Value,
                        versionForStatusUpdate,
                        ClassLibrary.Enum.AuctionStatus.ENDED_SOLD,
                        transaction);

                    if (!statusUpdated) // Optimistic lock for status update failed
                    {
                        return "Error with auction status update (e.g., version mismatch during buyout)"; // Txn rolled back
                    }
                }

                // 4. Wallet Update
                if (!await _walletLogic.ReserveFundsAsync(userWallet.WalletId!.Value, bid.Amount, expectedWalletVersion, transaction))
                {
                    return "Error with wallet version, please try again."; // Txn rolled back
                }

                transaction.Commit();
                return "Bid placed succesfully :)";
            }
            catch (Exception ex)
            {
                // Log the exception (ex.ToString())
                // The transaction will be rolled back automatically by the 'using' statement if an exception occurs
                // before Commit() is called.
                return $"An unexpected error occurred: {ex.Message}";
            }
            finally
            {
                // The 'using var transaction' will dispose the transaction (rolling back if not committed).
                // The 'connectionForTransaction' needs to be disposed/closed if we opened it.
                if (connectionForTransaction != null && connectionForTransaction.State == ConnectionState.Open)
                {
                    if (connectionForTransaction is SqlConnection sqlConn) await sqlConn.CloseAsync();
                    else connectionForTransaction.Close();
                }
                connectionForTransaction?.Dispose();
            }
        }


        }
}
