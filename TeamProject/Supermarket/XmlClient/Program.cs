using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using SuperMarket.Model;
using Supermarkets.Data;

namespace XmlClient
{
    public class Program
    {
        public static void Main()
        {
            SuperMarketContext superMarketContext = new SuperMarketContext();
            XElement xmlSales = new XElement("Sales");
           
            using (superMarketContext)
            {
                var allVendors = superMarketContext.Vendors.ToList();
                foreach (var item in allVendors)
                {
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