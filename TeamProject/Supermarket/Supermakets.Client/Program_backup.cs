using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperMarket.Model;
using SuperMarket.Model.Migrations;
using Supermarkets.Data;
using System.Xml.Linq;
using System.Globalization;

namespace Supermakets.Client
{
    class Program
    {
        static void Main()
        {
            Database.SetInitializer(
               new MigrateDatabaseToLatestVersion<SuperMarketContext, Configuration>());
            SuperMarketContext superMarketContext = new SuperMarketContext();

            

            //using (superMarketContext)
            //{
            //    Supermarket superMarket4 = new Supermarket()
            //    {
            //        Name = "Kaspichan4 – Center4"
            //    };
            //    superMarketContext.Supermarkets.Find(4);
            //}

            //SuperMarketContext superMarketContext = new SuperMarketContext();
            XElement xmlSales = new XElement("Sales");

            using (superMarketContext)
            {
                var vendors = superMarketContext.Vendors.ToList();

                foreach (var item in vendors)
                {
                    Console.WriteLine(item.Name);
                    var products = item.Products.ToList();
                    List<List<Order>> orders = new List<List<Order>>();

                    foreach (var order in products)
                    {
                        orders.Add(order.Orders.ToList());
                    }
                    XElement saleElement =
                        new XElement("sale", new XAttribute("vendor", item.Name));

                    foreach (var item1 in orders)
                    {
                        foreach (var item2 in item1)
                        {
                            XElement added = new XElement("summary",
                                new XAttribute("date", item2.Date.ToString("dd-MMM-yyyy",
                                    CultureInfo.InvariantCulture)), new XAttribute("total-sum", item2.Sum)
                                );

                            saleElement.Add(added);
                        }
                    }

                    xmlSales.Add(saleElement);
                }
            }

            xmlSales.Save("../../Sales-by-Vendors-report.xml");
        }
    }
}
