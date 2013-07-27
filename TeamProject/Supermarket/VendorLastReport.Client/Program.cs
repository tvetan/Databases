using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.IO;
using SuperMarket.Model;

namespace VendorLastReport.Client
{
    
    class Program
    {
        static void Main()
        {
            var mongoClient = new MongoClient("mongodb://localhost/");
            var mongoServer = mongoClient.GetServer();
            var database = mongoServer.GetDatabase("ProductsDb");
            var collection = database.GetCollection<VendorLastReport.Client.Product>("Products");
            var products = from w in collection.AsQueryable<VendorLastReport.Client.Product>()
                           select w;

            FillingData();
            //foreach (var product in products)
            //{
            //    AddProduct(product.Id, product.Name, product.VendorName,
            //        product.QuantitySold, product.TotalIncome);

            //    Console.WriteLine(product);
            //}

            //var productsByVendors =
            //    from p in collection.AsQueryable<Product>()

            //    select p;

            //var vends = productsByVendors.ToList().GroupBy(v => v.VendorName).Select(x => x);

            //foreach (var vend in vends)
            //{
            //    Console.WriteLine(vend.Key);
            //}

            
            
            OleDbConnectionStringBuilder csbuilder = new OleDbConnectionStringBuilder();
            csbuilder.Provider = "Microsoft.ACE.OLEDB.12.0";
            csbuilder.DataSource = @"..\..\Products-Total-Report.xlsx";
            csbuilder.Add("Extended Properties", "Excel 12.0 Xml;HDR=YES");

            using (OleDbConnection connection = new OleDbConnection(csbuilder.ConnectionString))
            {
                connection.Open();
                foreach (var item in products)
                {
                    string insertToTable =
                                          @"INSERT INTO [Sheet1$] (Vendor, Incomes,Expenses,Taxes, [Financial Result] )" +
                                          " VALUES (@vendor, @income, @expenses, @text, @finansiol)";
                    using (OleDbCommand command = connection.CreateCommand())
                    {                       
                        command.CommandType = CommandType.Text;
                        command.CommandText = insertToTable;
                        command.Parameters.AddWithValue("@vendor", item.VendorName);
                        command.Parameters.AddWithValue("@income", item.TotalIncome);
                        command.Parameters.AddWithValue("@expenses", "");
                        command.Parameters.AddWithValue("@text", "taxes");
                        command.Parameters.AddWithValue("@finansiol", "finansial reults");

                        command.ExecuteNonQuery();
                    }
                }
            }
        }
  
        private static void AddProduct(int id, string name, string vendorName,
            int quantitySold, decimal totalIncome)
        {
            SQLiteConnection connection =
                new SQLiteConnection(@"Data Source=..\..\VendorReport.db;Version=3;");
            connection.Open();

            using (connection)
            {
                SQLiteCommand containsInDatabase = new SQLiteCommand(
                    string.Format("select count(*) from Products where ProductId = {0}", id),
                    connection);

                long count = (long)containsInDatabase.ExecuteScalar();
                
                if (count > 0L)
                {
                    Console.WriteLine("here");
                    return;
                }

                SQLiteCommand command =
                    new SQLiteCommand("INSERT INTO Products VALUES (@id, @name, @vendorName, @quantitySold, @totalIncome)",
                        connection);
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@vendorName", vendorName);
                command.Parameters.AddWithValue("@quantitySold", quantitySold);
                command.Parameters.AddWithValue("@totalIncome", totalIncome);
                command.ExecuteNonQuery();
            }
        }

        public static void FillingData()
        {
            SQLiteConnection connection =
                new SQLiteConnection(@"Data Source=..\..\VendorReport.db;Version=3;");
            connection.Open();

            SuperMarketContext sqldb = new SuperMarketContext();

            using (connection)
            {
                using (sqldb)
                {
                    foreach (var product in sqldb.Products)
                    {
                        SQLiteCommand command =
                        new SQLiteCommand("INSERT INTO Reports VALUES (@prodName, '25%')", connection);
                        command.Parameters.AddWithValue("@prodName", product.Name);

                        command.ExecuteNonQuery();       
                    }
                }
            }
        }
    }
}