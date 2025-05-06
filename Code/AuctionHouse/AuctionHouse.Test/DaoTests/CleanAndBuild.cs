using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Microsoft.VisualStudio.TestPlatform.Utilities;

namespace AuctionHouse.Test.DaoTests
{
    public class CleanAndBuild
    {
        #region Fields
        //Connection
        private readonly IDbConnection _connection;

        //Teardown path
        private readonly string _truncateScriptPath = "TruncateTables.sql";
        //Generate testdata path
        private readonly string _generateTestDataScriptPath = "GenerateTestData.sql";
        //Both are added as links by rightclicking the AuctionHouse.Test Project
        //adding them as links, then chaning their properties to Build "Content" and Copy to
        //output directory to "Copy if newer"
        #endregion

        #region Constructor
        public CleanAndBuild() 
        {
            //Connection string
            string connectionString = Environment.GetEnvironmentVariable("DatabaseConnectionString");

            //check if connectionstring is empty
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("FATAL: DatabaseConnectionString environment variable is not set or is empty. Check test configuration.");
            }

            _connection = new SqlConnection(connectionString);
        }
        #endregion

        #region Methods
        //Execute SQL Script method from filepath
        private void ExecuteSqlScript(string filePath)
        {
            //reads all text from the passed file path
            string sql = File.ReadAllText(filePath);

            //creates command with connection, and runs
            //command with the SQL copied from the passed filepath
            using var command = _connection.CreateCommand();
            command.CommandText = sql;

            //Open connection if it isnt open
            if (_connection.State != ConnectionState.Open)
            {
                _connection.Open();
            }
            //Execute command
            command.ExecuteNonQuery();
        }

        //Clean Tables
        public void CleanDB()
        {
            ExecuteSqlScript(_truncateScriptPath);
        }

        //Generate testdata
        public void GenerateFreshTestDB()
        {
            ExecuteSqlScript(_generateTestDataScriptPath);
        }
        #endregion
    }
}
