using System;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace GenerateTaxedExcel
{
    class GenerateTaxedExcelCOnsole
    {
        static void Main(string[] args)
        {
            var mongoClient = new MongoClient("mongodb://localhost/");
            var mongoServer = mongoClient.GetServer();
            var database = mongoServer.GetDatabase("ProductsDb");
            var collection = database.GetCollection<Product>("Products");

            var products =
                from product in collection.AsQueryable<Product>()
                select product;
          
            var productsGroupedbyVendor = products.ToList().GroupBy(product => product.VendorName).Select(x => x);
            
            foreach (var productGroup in productsGroupedbyVendor)
            {
                Console.WriteLine(productGroup.Key);
            }
        }
    }
}