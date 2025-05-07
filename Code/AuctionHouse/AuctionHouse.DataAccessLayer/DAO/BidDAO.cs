using AuctionHouse.ClassLibrary.Model;
using AuctionHouse.DataAccessLayer.Interfaces;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionHouse.DataAccessLayer.DAO
{
    public class BidDAO : IBidDao
    {

        private readonly IDbConnection _dbConnection;
        private readonly IUserDao _userDao;

        public BidDAO(IDbConnection dbConnection, IUserDao userdao)
        {
            _dbConnection = dbConnection;
            _userDao = userdao;
        }

        public async Task<bool> DeleteAsync(Bid entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Bid>> GetAllAsync()
        {
            return null;
        }



        public async Task<Bid> GetByIdAsync(int id)
        {
            const string sql = @"SELECT
                                BidId,
                                AuctionId,
                                Amount,
                                TimeStamp,
                                UserId    
                            FROM dbo.Bid
                            WHERE BidId = @BidId;";

            var bid = await _dbConnection.QuerySingleOrDefaultAsync(sql, new { BidId = id });
            Console.WriteLine("In BidDAO.GetByIdAsync");

                Console.WriteLine("Casted bidT as concrete bid");
                if(bid.User == null || true)
                {
                bid.User =await _userDao.GetByIdAsync(bid.UserId.Value);
                }
            

            return bid;

        }

        public async Task<Bid> GetLatestByAuctionId(int auctionId)
        {

            const string sql = @"SELECT TOP 1
                            b.BidId, b.Amount, b.TimeStamp, b.UserId AS BidUserId, b.AuctionId,
                            u.UserId AS User_UserId,
                            u.CantBuy, u.CantSell, u.UserName, u.PasswordHash,
                            u.RegistrationDate, u.FirstName, u.LastName, u.Email,
                            u.PhoneNumber, u.[Address], u.IsDeleted
                        FROM dbo.Bid b
                        JOIN dbo.[User] u ON b.UserId = u.UserId
                        WHERE b.AuctionId = @AuctionId
                        ORDER BY b.Amount DESC, b.TimeStamp DESC;";

            var bids = await _dbConnection.QueryAsync<Bid, User, Bid>(
                sql,
                map: (bid, user) =>
                {
                    // add the user to the bid, this is similar to the static map function in ItemDAO.
                    //This is just with a lambda :)
                    bid.User = user;
                    return bid;
                },
                param: new { AuctionId = auctionId },

                splitOn: "User_UserId"
            );

            return bids.FirstOrDefault();
        }

        // Since bid is where our concurrency lies, we need to add an IDbTransaction.
        public async Task<int> InsertBidAsync(Bid entity, IDbTransaction transaction = null)
        {
            const string sql = @"
                                INSERT INTO Bid (AuctionId, Amount, TimeStamp, UserId)
                                VALUES (@AuctionId, @Amount, @TimeStamp, @UserId);
                                SELECT CAST(SCOPE_IDENTITY() as int);";


            var parameters = new
            {
                entity.AuctionId,
                entity.Amount,
                entity.TimeStamp,
                UserId = entity.User?.UserId
            };
            if (parameters.UserId == null) throw new ArgumentException("Bid must have a User with a userId.");


            return await _dbConnection.ExecuteScalarAsync<int>(sql, parameters, transaction: transaction);
        }

        public async Task<bool> UpdateAsync(Bid entity)
        {
            const string sql = "UPDATE Bid SET AuctionId = @AuctionId, Amount = @Amount, TimeStamp = @TimeStamp WHERE BidId = @BidId;";
            var result = await _dbConnection.ExecuteAsync(sql, new { entity.AuctionId, entity.Amount, entity.TimeStamp, entity.BidId });
            return result > 0;
        }

        public async Task<List<Bid>> GetAllByAuctionIdAsync(int auctionId)
        {
            const string sql = @"SELECT
                            BidId, Amount, TimeStamp, UserId, AuctionId
                        FROM dbo.Bid
                        WHERE AuctionId = @AuctionId
                        ORDER BY TimeStamp DESC;";

            var bidT = await _dbConnection.QueryAsync<Bid>(sql, new { AuctionId = auctionId });
            Console.WriteLine("In BidDAO");
            foreach(var bid in bidT)
            {
                if (bid.User == null)
                {
                    bid.User = await _userDao.GetByIdAsync(bid.UserId!.Value);
                }
            }


            return bidT.ToList();


        }

        Task<int> IGenericDao<Bid>.InsertAsync(Bid entity)
        {
            throw new NotImplementedException("Use BidDAO.InsertBidAsync instead");
        }
    }
}
