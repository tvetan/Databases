using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperMarket.Model;
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
                from p in collection.AsQueryable<Product>()
                
                select p;

            var a = products.ToList().GroupBy(v=>v.VendorName).Select(x=>x);
            
            foreach (var item in a)
            {
                Console.WriteLine(item.Key);
            }
        }
    }
}
