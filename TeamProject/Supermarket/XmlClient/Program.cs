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
                foreach (var vendor in allVendors)
                {
                    var products = vendor.Products.ToList();
                    List<List<Order>> orders = new List<List<Order>>();

                    foreach (var order in products)
                    {
                        orders.Add(order.Orders.ToList());
                    }

                    XElement saleElement =
                        new XElement("sale", new XAttribute("vendor", vendor.Name));

                    foreach (var order in orders)
                    {
                        foreach (var item in order)
                        {
                            XElement added = new XElement("summary",
                                new XAttribute("date", item.Date.ToString("dd-MMM-yyyy",
                                    CultureInfo.InvariantCulture)), new XAttribute("total-sum", item.Sum)
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