using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperMarket.Model;
using SuperMarket.Model.Migrations;
using Supermarkets.Data;
using XmlExpenses;

namespace Supermakets.Client
{
    class Program
    {
        static void Main()
        {
            Database.SetInitializer(
                new MigrateDatabaseToLatestVersion<SuperMarketContext, Configuration>());

            // 1: MySQL migration and SQL setup
            //SuperMarketContext context = new SuperMarketContext();
            //using (context)
            //{
            //    foreach (var item in context.Products)
            //    {
            //        //Console.WriteLine(item.Name);
            //    }
            //}

            // 1.2: Importing orders
            //ImportExcelReport.ImportOrders();

            // 5: Expenses

            //XmlImporter.ImportXmlToSql();

            //XmlImporter.ImportFromXmlToMongo();
        }
    }
}
