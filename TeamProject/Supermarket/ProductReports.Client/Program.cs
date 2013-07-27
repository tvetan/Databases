using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using Newtonsoft.Json;
using SuperMarket.Model;

namespace ProductReports.Client
{
    class Program
    {
 

        static void Main()
        {
            var mongoClient = new MongoClient("mongodb://localhost/");
            var mongoServer = mongoClient.GetServer();
            var database = mongoServer.GetDatabase("ProductsDb");
            var collection = database.GetCollection<Product>("Products");
            SuperMarketContext context = new SuperMarketContext();
            
            using (context)
            {
                foreach (var item in context.Products)
                {
                    IMongoQuery query1 = Query<Product>.EQ(x => x.Id, item.ProductId);
                    var res = collection.FindOne(query1);
                    if (res == null)//don't exist
                    {
                        AddProduct(context, item, collection);
                    }

                    //Console.WriteLine(incomeSum);
                    //Console.WriteLine(quantitySum);
                    //SELECT SUM(o.Sum), Sum(o.Quantity), p.Name, v.Name FROM Orders o JOIN Products p
                    //ON o.ProductId = p.ProductId
                    //JOIN Vendors v
                    //ON p.VendorId = v.VendorId
                    //WHERE p.ProductId = 1
                    //GROUP BY p.ProductId, v.Name, p.Name
                    ////Product deserializedProduct = JsonConvert.DeserializeObject<Product>(json);
                }
            }
        }
  
        private static void AddProduct(SuperMarketContext context,
            Supermarkets.Data.Product item, MongoCollection<Product> collection)
        {
            var entity = from o in context.Orders
                         join p in context.Products on o.ProductId equals p.ProductId
                         join v in context.Vendors on p.VendorId equals v.VendorId
                         where p.ProductId == item.ProductId
                         select new { p.Name, p.ProductId, o.Sum, o.Quantity };
                          
            var incomeSum = entity.Select(o => o.Sum).ToList().Sum();
            var quantitySum = entity.Select(o => o.Quantity).ToList().Sum();

            var addedProduct = new Product
            {
                Id = item.ProductId,
                Name = item.Name,
                QuantitySold = quantitySum,
                TotalIncome = incomeSum,
                VendorName = item.Vendor.Name
            };
            collection.Insert(addedProduct);
            var query = Query<Product>.EQ(x => x.Id, item.ProductId);
            var product = collection.FindOne(query);
            string serializedProduct = JsonConvert.SerializeObject(product, Formatting.Indented);
            ////Console.WriteLine(json);
            bool isExists = System.IO.Directory.Exists(@"..\..\ProductReports");
            if (!isExists)
            {
                System.IO.Directory.CreateDirectory(@"..\..\ProductReports");
            }
            StreamWriter writer = new StreamWriter(@"..\..\ProductReports\" + product.Id + ".json");
            using (writer)
            {
                writer.WriteLine(serializedProduct);
            }
        }
    }
}