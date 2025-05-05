using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuctionHouse.ClassLibrary.Model;
using AuctionHouse.DataAccessLayer.DAO;
using AuctionHouse.DataAccessLayer.Interfaces;
using System.Data.SqlClient;

namespace AuctionHouse.Test.DaoTests
{
    public class ItemDaoTest
    {
        private readonly IDbConnection _connection;
        private readonly IItemDao _itemDao;

        public ItemDaoTest()
        {
            // Initialize fields inside the constructor where 'this' is fully valid
            _connection = new SqlConnection(Environment.GetEnvironmentVariable("DatabaseConnectionString"));
            Console.WriteLine($"SQL connection: {_connection.ToString()}");
            _itemDao = new ItemDAO(_connection); // Now you can use _connection
        }



        [Fact]
        public async Task GetAllAsync_ShouldReturnListOfBids()
        {
            // Act
            List<Item> items = await _itemDao.GetAllAsync<Item>();

            // Assert   
            Assert.NotEmpty(items);
        }


    }
}
