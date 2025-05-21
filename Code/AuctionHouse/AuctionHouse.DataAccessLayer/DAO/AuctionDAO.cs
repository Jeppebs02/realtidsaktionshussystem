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

        private readonly Func<IDbConnection> _connectionFactory;
        private readonly IBidDao _bidDao;
        private readonly IUserDao _userDao;
        private readonly IItemDao _itemDao;

        public AuctionDAO(Func<IDbConnection> connectionFactory, IBidDao biddao, IItemDao itemDao, IUserDao userdao)
        {
            _connectionFactory = connectionFactory;
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
            const string sql = @"SELECT AuctionId, StartTime, EndTime, StartPrice, BuyOutPrice, MinimumBidIncrement, AuctionStatus, Version, Notify, ItemId, AmountOfBids
                                FROM dbo.Auction WHERE AuctionStatus = @Status;";
            return await GetAllAuctionsWithDetails(sql, new { Status = AuctionStatus.ACTIVE.ToString() });
        }

        public async Task<List<Auction>> GetAllAsync()
        {
            const string sql = @"SELECT AuctionId, StartTime, EndTime, StartPrice, BuyOutPrice, MinimumBidIncrement, AuctionStatus, Version, Notify, ItemId, AmountOfBids
                                FROM dbo.Auction;";
            return await GetAllAuctionsWithDetails(sql);
        }

        public async Task<Auction?> GetByIdAsync(int id, IDbTransaction? transaction)
        {
            IDbConnection conn;
            bool ownConnection = false;

            if (transaction != null)
            {
                conn = transaction.Connection;
                // We are using the transaction's connection, so Dapper calls should pass the transaction.
            }
            else
            {
                conn = _connectionFactory();

                ownConnection = true;
                // Dapper will open/close this connection for its operation if ownConnection is true.
            }



            // SQL to fetch only the Auction data
            const string auctionSql = @"SELECT
                                    AuctionId, StartTime, EndTime, StartPrice, BuyOutPrice,
                                    MinimumBidIncrement, AuctionStatus, Version, Notify,
                                    ItemId, AmountOfBids
                                FROM dbo.Auction
                                WHERE AuctionId = @AuctionId_Param;"; // Renamed param to avoid potential conflicts

            // Fetch the auction itself.
            // If a transaction is provided, it's passed to Dapper.
            var auctionTask = conn.QuerySingleOrDefaultAsync<Auction>(auctionSql, new { AuctionId_Param = id }, transaction: transaction);


            var bidsTask = _bidDao.GetAllByAuctionIdAsync(id); // 'id' is auctionId

            // Task for item will be initialized after auction is fetched, to use auction.itemId
            Task<Item?> itemTask;

            // Await the auction fetch first
            Auction? concreteAuction = await auctionTask;

            // Now that we have the auction, we can determine the itemId for the itemTask.
            // And we can run bidsTask and itemTask concurrently.
            if (concreteAuction.itemId.HasValue)
            {
                itemTask = _itemDao.GetByIdAsync(concreteAuction.itemId.Value);
            }
            else
            {
                itemTask = Task.FromResult<Item?>(null); // No item to fetch
            }

            // Await the remaining tasks
            // We already awaited auctionTask, so only bidsTask and itemTask are left.
            await Task.WhenAll(bidsTask, itemTask);

            concreteAuction.Bids = await bidsTask ?? new List<Bid>(); // Ensure Bids is initialized
            concreteAuction.item = await itemTask;

            return concreteAuction;
        }

        public Task<IEnumerable<Auction>> GetAllByUserIDAsync(int userId)
        {
            throw new NotImplementedException();
        }

        //public async Task<Auction> GetByIdAsync(int id)
        //{
        //    using var conn = _connectionFactory();
        //    const string sql = @"SELECT
        //                    AuctionId,
        //                    StartTime,
        //                    EndTime,
        //                    StartPrice,
        //                    BuyOutPrice,
        //                    MinimumBidIncrement,
        //                    AuctionStatus,
        //                    Version,
        //                    Notify,
        //                    ItemId,
        //                    AmountOfBids
        //                FROM dbo.Auction
        //    WHERE AuctionId = @AuctionId;";

        //    var auction = conn.QuerySingleAsync<Auction>(sql, new { AuctionId = id });
        //    var bids = _bidDao.GetAllByAuctionIdAsync(id);
        //    var item = _itemDao.GetItemByAuctionIdAsync(id);
        //    await Task.WhenAll(auction, bids, item);


        //    Auction concreteAuction = auction.Result;
        //    if(concreteAuction == null)
        //    {
        //        return null;
        //    }


        //    concreteAuction.Bids = bids.Result;

        //    concreteAuction.item = item.Result;

        //    return concreteAuction;

        //}

        public async Task<Auction> GetByIdAsync(int id)
        {
            return await this.GetByIdAsync(id, null);

        }



        public async Task<IEnumerable<Auction>> GetWithinDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public async Task<int> InsertAsync(Auction entity)
        {
            // dbo.Auction includes these columns: AuctionId (PK AI) StartTime, EndTime, StartPrice, BuyOutPrice, MinimumBidIncrement, AuctionStatus, Version, Notify, ItemId, AmountOfBids
            using var conn = _connectionFactory();
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

            var auctionId = await conn.ExecuteScalarAsync<int>(sql, parameters);
            return auctionId;
        }

        public async Task<bool> UpdateAsync(Auction entity)
        {
            throw new NotImplementedException("Dont use, use UpdateAuctionOptimistically instead");
        }

        public async Task<byte[]> UpdateAuctionOptimistically(int auctionId, byte[] expectedVersion, IDbTransaction transaction = null, int newBids = 1)
        {

            var conn = _connectionFactory();

            //Make sure to use the same connection as the transaction if it is not null
            if (transaction != null)
            {
                conn = transaction.Connection;
            }

            const string sql = @"UPDATE Auction
                                 SET AmountOfBids = AmountOfBids + @AmountOfBids
                                 OUTPUT INSERTED.Version
                                 WHERE AuctionId = @AuctionId
                                 AND Version = @ExpectedVersion";
            var parameters = new DynamicParameters();
            parameters.Add("AuctionId", auctionId);
            parameters.Add("AmountOfBids", newBids);
            parameters.Add("ExpectedVersion", expectedVersion);

            byte[] newVersion = await conn.ExecuteScalarAsync<byte[]>(sql, parameters, transaction: transaction);
            return newVersion;
        }

        public async Task<bool> UpdateAuctionStatusOptimisticallyAsync(int auctionId, byte[] expectedVersion, AuctionStatus newStatus, IDbTransaction transaction = null)
        {


            var conn = _connectionFactory();
            Console.WriteLine("Trying to update auction status");
            //Make sure to use the same connection as the transaction if it is not null
            if (transaction != null)
            {
                conn = transaction.Connection;
            }

            const string sql = @"UPDATE Auction
                                 SET AuctionStatus = @AuctionStatus
                                 WHERE AuctionId = @AuctionId
                                 AND Version = @ExpectedVersion";

            var parameters = new DynamicParameters();
            parameters.Add("AuctionId", auctionId);
            parameters.Add("AuctionStatus", newStatus.ToString());
            parameters.Add("ExpectedVersion", expectedVersion);
            int rowsAffected = await conn.ExecuteAsync(sql, parameters, transaction: transaction);
            Console.WriteLine(rowsAffected);
            return rowsAffected > 0;
        }

        public async Task<IEnumerable<Auction>> GetAllByBidsAsync(int userId)
        {
            throw new NotImplementedException();
        }





        private async Task<List<Auction>> GetAllAuctionsWithDetails(string auctionFilterSql, object filterParams = null)
        {
            using var conn = _connectionFactory();

            // 1. Fetch base auctions
            var auctions = (await conn.QueryAsync<Auction>(auctionFilterSql, filterParams)).ToList();
            if (!auctions.Any()) { return auctions; } 

            // Select "converts" the Auction object to an int.
            var auctionIds = auctions.Select(a => a.AuctionID!.Value).ToList();
            // Where filters auctions where the itemId property is not null
            // Then we select the itemId property and only take unique ids.
            var itemIdsToFetch = auctions.Where(a => a.itemId.HasValue).Select(a => a.itemId!.Value).Distinct().ToList();

            // 2. Batch fetch all items for these auctions (Items will include their User+Wallet)
            Dictionary<int, Item> itemsMap = new Dictionary<int, Item>();
            if (itemIdsToFetch.Any())
            {
                // Select takes an int and "converts" it to a Task<Item>
                List<Task<Item>> itemTasks = itemIdsToFetch.Select(itemId => _itemDao.GetByIdAsync(itemId)).ToList();
                Item[] fetchedItems = await Task.WhenAll(itemTasks);
                itemsMap = fetchedItems.Where(i => i != null).ToDictionary(i => i.ItemId!.Value);
            }

            // 3. Batch fetch all bids for these auctions (Bids will include their User+Wallet)
            Dictionary<int, List<Bid>> bidsByAuctionIdMap = new Dictionary<int, List<Bid>>();
            if (auctionIds.Any())
            {

                List<Task<(int auctionId, List<Bid> bids)>> bidTasks = auctionIds.Select(async auctionId =>
                {
                    var bidsForAuction = await _bidDao.GetAllByAuctionIdAsync(auctionId);
                    return (auctionId, bidsForAuction);
                }).ToList();
                var fetchedBidsForAllAuctions = await Task.WhenAll(bidTasks);
                foreach (var bidData in fetchedBidsForAllAuctions)
                {
                    bidsByAuctionIdMap[bidData.auctionId] = bidData.bids;
                }
            }

            // 4. Map items and bids back to auctions
            foreach (var auction in auctions)
            {
                if (auction.itemId.HasValue && itemsMap.TryGetValue(auction.itemId.Value, out Item item))
                {
                    auction.item = item;
                }
                if (bidsByAuctionIdMap.TryGetValue(auction.AuctionID!.Value, out List<Bid> bids))
                {
                    auction.Bids = bids;
                }
                else
                {
                    auction.Bids = new List<Bid>(); // Ensure initialized
                }
            }
            return auctions;
        }



    }

}

