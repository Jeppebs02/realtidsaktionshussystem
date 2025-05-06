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
    public static class CleanAndBuild
    {
        #region Fields
        //Teardown path
        private static readonly string _truncateScriptPath = "TruncateTables.sql";
        //Generate testdata path
        private static readonly string _generateTestDataScriptPath = "GenerateTestData.sql";
        //Both are added as links by rightclicking the AuctionHouse.Test Project
        //adding them as links, then chaning their properties to Build "Content" and Copy to
        //output directory to "Copy if newer"
        #endregion

        #region Methods
        //Execute SQL Script method from filepath
        private static void ExecuteSqlScript(string filePath)
        {
            //Get connection string from Docker-compose
            string connectionString = Environment.GetEnvironmentVariable("DatabaseConnectionString");

            //Check if the connection string
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("FATAL: DatabaseConnectionString environment variable is not set or empty.");
            }

            //Read SQL script from file path
            string sql = File.ReadAllText(filePath);

            //Create a new SQL connection
            using var connection = new SqlConnection(connectionString);

            //Open the connection to the database
            connection.Open();

            //Create command with connection and assign the SQL script to it
            using var command = connection.CreateCommand();
            command.CommandText = sql;

            //Execute the SQL script in the database
            command.ExecuteNonQuery();
        }

        //Clean Tables method
        public static void CleanDB()
        {
            ExecuteSqlScript(_truncateScriptPath);
        }

        //Generate testdata method
        public static void GenerateFreshTestDB()
        {
            ExecuteSqlScript(_generateTestDataScriptPath);
        }
        #endregion
    }
}
