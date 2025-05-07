using AuctionHouse.DataAccessLayer.Interfaces;
using AuctionHouse.ClassLibrary.Model;
using AuctionHouse.ClassLibrary.Enum;
using System;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionHouse.DataAccessLayer.DAO
{
    public class AuctionDAO : IAuctionDao
    {

        private readonly IDbConnection _dbConnection;
        private readonly IBidDao _bidDao;
        private readonly IUserDao _userDao;
        private readonly IItemDao _itemDao;

        public AuctionDAO(IDbConnection dbConnection, IBidDao biddao, IItemDao itemDao, IUserDao userdao)
        {
            _dbConnection = dbConnection;
            _bidDao = biddao;
            _itemDao = itemDao;
            _userDao = userdao;
        }

        public Task<bool> DeleteAsync(Auction entity)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Auction>> GetAllActiveAsync()
        {
            const string sql = @"SELECT
                            AuctionId,
                            StartTime,
                            EndTime,
                            StartPrice,
                            BuyOutPrice,
                            MinimumBidIncrement,
                            AuctionStatus,
                            Version,
                            Notify,
                            ItemId,
                            AmountOfBids
                        FROM dbo.Auction
                        WHERE AuctionStatus = 'ACTIVE'";

            var auctions = await _dbConnection.QueryAsync<Auction>(sql);

            foreach (var auction in auctions)
            {
                if (auction.Bids == null)
                {
                    auction.Bids = new List<Bid>();
                }
                auction.Bids = await _bidDao.GetAllByAuctionIdAsync(auction.AuctionID.Value);
            }

            return auctions;

        }

        public async Task<List<Auction>> GetAllAsync()
        {
            const string sql = @"SELECT
                            AuctionId,
                            StartTime,
                            EndTime,
                            StartPrice,
                            BuyOutPrice,
                            MinimumBidIncrement,
                            AuctionStatus,
                            Version,
                            Notify,
                            ItemId,
                            AmountOfBids
                        FROM dbo.Auction;";
            var auctions = (await _dbConnection.QueryAsync<Auction>(sql)).ToList();
            var result = new List<Auction>();


            foreach (var auction in auctions)
            {
                var bids = _bidDao.GetAllByAuctionIdAsync(auction.AuctionID!.Value);
                var item = _itemDao.GetByIdAsync(auction.itemId!.Value);

                Task.WaitAll(bids, item);

                auction.Bids = bids.Result.ToList();
                auction.item = item.Result;

                result.Add(auction);
            }



            return result;
        }

        public Task<IEnumerable<Auction>> GetAllByUserIDAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<Auction> GetByIdAsync(int id)
        {
            const string sql = @"SELECT
                            AuctionId,
                            StartTime,
                            EndTime,
                            StartPrice,
                            BuyOutPrice,
                            MinimumBidIncrement,
                            AuctionStatus,
                            Version,
                            Notify,
                            ItemId,
                            AmountOfBids
                        FROM dbo.Auction
            WHERE AuctionId = @AuctionId;";

            var auctionT = _dbConnection.QuerySingleOrDefaultAsync<Auction>(sql, new { AuctionId = id });
            var bids = _bidDao.GetAllByAuctionIdAsync(id);
            Task.WaitAll(auctionT, bids);


            Auction concreteAuction = auctionT.Result;
            if (concreteAuction.Bids == null)
            {
                concreteAuction.Bids = new List<Bid>();
            }

            List<Bid> bidsList = bids.Result.ToList();
            foreach (Bid bid in bidsList)
            {
                concreteAuction.Bids.Add(bid);
            }

            var item = await _itemDao.GetByIdAsync(concreteAuction.itemId!.Value);
            concreteAuction.item = item;


            return concreteAuction;

        }

        public async Task<IEnumerable<Auction>> GetWithinDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public async Task<int> InsertAsync(Auction entity)
        {
            // dbo.Auction includes these columns: AuctionId (PK AI) StartTime, EndTime, StartPrice, BuyOutPrice, MinimumBidIncrement, AuctionStatus, Version, Notify, ItemId, AmountOfBids

            const string sql = @"
                            INSERT INTO dbo.Auction
                            (StartTime, EndTime, StartPrice, BuyOutPrice, MinimumBidIncrement, AuctionStatus, ItemId)
                            OUTPUT INSERTED.AuctionId
                            VALUES
                            (@StartTime, @EndTime, @StartPrice, @BuyOutPrice, @MinimumBidIncrement, @AuctionStatus, @ItemId)";

            var parameters = new DynamicParameters();
            parameters.Add("StartTime", entity.StartTime);
            parameters.Add("EndTime", entity.EndTime);
            parameters.Add("StartPrice", entity.StartPrice);
            parameters.Add("BuyOutPrice", entity.BuyOutPrice);
            parameters.Add("MinimumBidIncrement", entity.MinimumBidIncrement);
            parameters.Add("AuctionStatus", entity.AuctionStatus.ToString());

            parameters.Add("ItemId", entity.item.ItemId);

            var auctionId = await _dbConnection.ExecuteScalarAsync<int>(sql, parameters);
            return auctionId;
        }

        public async Task<bool> UpdateAsync(Auction entity)
        {
            throw new NotImplementedException("Dont use, use UpdateAuctionOptimistically instead");
        }

        public async Task<bool> UpdateAuctionOptimistically(int auctionId, byte[] expectedVersion, IDbTransaction transaction = null, int newBids = 1)
        {
            const string sql = @"UPDATE Auction
                                 SET AmountOfBids = AmountOfBids + @AmountOfBids
                                 WHERE AuctionId = @AuctionId
                                 AND Version = @ExpectedVersion";
            var parameters = new DynamicParameters();
            parameters.Add("AuctionId", auctionId);
            parameters.Add("AmountOfBids", newBids);
            parameters.Add("ExpectedVersion", expectedVersion);

            int rowsAffected = await _dbConnection.ExecuteAsync(sql, parameters, transaction: transaction);
            return rowsAffected > 0;
        }

        public async Task<bool> UpdateAuctionStatusOptimisticallyAsync(int auctionId, byte[] expectedVersion, AuctionStatus newStatus, IDbTransaction transaction = null)
        {
            const string sql = @"UPDATE Auction
                                 SET AuctionStatus = @AuctionStatus
                                 WHERE AuctionId = @AuctionId
                                 AND Version = @ExpectedVersion";

            var parameters = new DynamicParameters();
            parameters.Add("AuctionId", auctionId);
            parameters.Add("AuctionStatus", newStatus);
            parameters.Add("ExpectedVersion", expectedVersion);
            int rowsAffected = await _dbConnection.ExecuteAsync(sql, parameters, transaction: transaction);
            return rowsAffected > 0;
        }

        public async Task<IEnumerable<Auction>> GetAllByBidsAsync(int userId)
        {
            throw new NotImplementedException();
        }
    }

}

