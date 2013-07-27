using System;
using System.Collections.Generic;
using Ionic.Zip;
using System.IO;
using Excel;
using Supermarkets.Data;
using System.Globalization;
using System.Threading;
using SuperMarket.Model;
using System.Linq;

namespace Supermakets.Client
{
    public class ImportExcelReport
    {
        public static void ImportOrders()
        {
            CultureInfo invariantCulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentCulture = invariantCulture;
            string zipToUnpack = @"..\..\Sample-Sales-Reports.zip";
            string unpackDirectory = @"..\..\unzipedReports";

            Extract(unpackDirectory, zipToUnpack);
            string[] files = Directory.GetFiles(unpackDirectory, "*.xls", SearchOption.AllDirectories);

            foreach (string file in files)
            {
                var entities = ReadEntitiesFromFile(file);
                using (var conn = new SuperMarketContext())
                {
                    foreach (var order in entities)
                    {
                        conn.Orders.Add(order);
                    }

                    conn.SaveChanges();
                }
            }
        }

        public static IEnumerable<Order> ReadEntitiesFromFile(string filePath)
        {

            var stream = File.Open(filePath, FileMode.Open, FileAccess.Read);
            var orders = new List<Order>();
            var supermarketId = 0;

            string dateAsString = filePath.Substring(filePath.Length - 15, 11);

            var date = DateTime.ParseExact(dateAsString, "dd-MMM-yyyy", null);

            using (var reader = ExcelReaderFactory.CreateBinaryReader(stream))
            {
                while (reader.Read())
                {
                    string firstColumnValue = "" + (string)reader.GetValue(1);
                    if (firstColumnValue.StartsWith("Supermarket"))
                    {
                        var name = firstColumnValue;
                        using (var conn = new SuperMarketContext())
                        {
                            var superMarket = conn.Supermarkets.FirstOrDefault(s => s.Name == name);
                            if (superMarket == null)
                            {
                                conn.Supermarkets.Add(new Supermarket { Name = name });
                                conn.SaveChanges();
                                superMarket = conn.Supermarkets.FirstOrDefault(s => s.Name == name);
                            }     
                            
                            supermarketId = superMarket.SupermarketId;
                        }
                    }

                    if (reader.GetString(1) == "ProductID")
                    {
                        while (reader.Read())
                        {
                            if (reader.GetString(1) == "Total sum:")
                            {
                                break;
                            }

                            var quantity = reader.GetInt32(2);
                            decimal sum = reader.GetDecimal(3) * quantity;
                            var order = new Order { ProductId = reader.GetInt32(1), Quantity = quantity, Sum = sum, Date = date, SupermarketId = supermarketId, UnitPrice = reader.GetDecimal(3) };
                            orders.Add(order);
                        }
                    }
                }
            }

            return orders;
        }

        public static void Extract(string unpackDirectory, string zipToUnpack)
        {
            using (ZipFile zip1 = ZipFile.Read(zipToUnpack))
            {
                foreach (ZipEntry e in zip1)
                {
                    e.Extract(unpackDirectory, ExtractExistingFileAction.OverwriteSilently);
                }
            }
        }
    }
}
