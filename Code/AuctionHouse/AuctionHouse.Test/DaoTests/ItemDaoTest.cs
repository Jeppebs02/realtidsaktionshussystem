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
using Xunit.Abstractions;

namespace AuctionHouse.Test.DaoTests
{
    public class ItemDaoTest
    {
        private readonly IDbConnection _connection;
        private readonly IItemDao _itemDao;
        private readonly ITestOutputHelper _output;


        public ItemDaoTest(ITestOutputHelper output)
        {
            _output = output;

            _output.WriteLine("Test Constructor: Attempting to read environment variable...");
            string connectionString = Environment.GetEnvironmentVariable("DatabaseConnectionString");

            
            _output.WriteLine($"Test Constructor: Retrieved Connection String: '{connectionString ?? "NULL"}'");
                
            if (string.IsNullOrEmpty(connectionString))
            {
                _output.WriteLine("Test Constructor: FATAL - Connection string is null or empty."); // Log before throwing
                // Consider throwing a more specific exception or using Assert.True within a test setup method if applicable
                throw new InvalidOperationException("FATAL: DatabaseConnectionString environment variable is not set or is empty. Check test configuration.");
            }

            _output.WriteLine("Test Constructor: Creating SqlConnection...");
            _connection = new SqlConnection(connectionString);
            _output.WriteLine("Test Constructor: Creating ItemDAO...");
            _itemDao = new ItemDAO(_connection);
            _output.WriteLine("Test Constructor: Setup complete.");
        }





        [Fact]
        public async Task GetAllAsync_ShouldReturnListOfItems()
        {
            // Act
            Console.WriteLine($"env var: {Environment.GetEnvironmentVariable("DatabaseConnectionString")}");
            List<Item> items = await _itemDao.GetAllAsync<Item>();

            // Assert   
            Assert.NotEmpty(items);
        }


    }
}
