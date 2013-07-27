using System;
using System.Linq;
using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using SuperMarket.Model;
using Supermarkets.Data;
using System.Collections;

namespace PDFGenerator
{
    public class Program
    {
        private const int cols = 5;

        
        public static void Main(string[] args)
        {
            SuperMarketContext entities = new SuperMarketContext();
            //SupermarketDbEntities entities = new SupermarketDbEntities();

            var document = new Document();
            PdfWriter.GetInstance(document, new FileStream("../../SalesReport.pdf", FileMode.Create));
            document.Open();       

            PdfPTable table = new PdfPTable(cols);
            using (entities)
            {
                var ordersQuery =
                    from o in entities.Orders
                    join p in entities.Products
                    on o.ProductId equals p.ProductId
                    orderby o.Date
                    select o;
                var orders = ordersQuery.ToList();

                DateTime previousDate = new DateTime();

                //var firstOrder = orders.FirstOrDefault();

                PdfPCell titleCell = new PdfPCell(new Phrase("Aggregated Sales Report"));
                titleCell.Colspan = cols;
                titleCell.HorizontalAlignment = 1;
                table.AddCell(titleCell);

                int count = 0;
                decimal sum = 0m;
                //Console.WriteLine(orders.FirstOrDefault(o=>o.ProductId == 1));
                foreach (var order in orders)
                {
                    DateTime currentDate = order.Date;

                    if (currentDate.Date.CompareTo(previousDate.Date) != 0)
                    {
                        if (count > 0)
                        {
                            PdfPCell fullSum = new PdfPCell(new Phrase(string.Format("Total sum: {0}", sum)));
                            fullSum.Colspan = 5;
                            fullSum.HorizontalAlignment = 2;
                            table.AddCell(fullSum);
                            sum = 0m;
                        }

                        count++;

                        GenerateTableHeadRow(table, currentDate);

                        previousDate = currentDate;
                    }

                    table.AddCell(GenerateCell(order.Product.Name, 1, false));
                    table.AddCell(GenerateCell(order.Quantity.ToString(), 1, false));
                    table.AddCell(GenerateCell(order.UnitPrice.ToString(), 1, false));
                    table.AddCell(GenerateCell(order.Supermarket.Name.ToString(), 1, false));
                    table.AddCell(GenerateCell(order.Sum.ToString(), 1, false));
                    sum += order.Sum;
                }                

                document.Add(table);

                document.Close();
    
            }     
        }


        public static PdfPCell GenerateCell(string content, int colspan, bool isBackgroundGrey)
        {
            PdfPCell cell = new PdfPCell(new Phrase(content));

            if (isBackgroundGrey == true)
            {
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            }

            cell.Colspan = colspan;
            return cell;
        }
        
        private static void GenerateTableHeadRow(PdfPTable table, DateTime date)
        {
            table.AddCell(GenerateCell(string.Format("Date: {0:d}", date), cols, true));
            table.AddCell(GenerateCell(PdfColumn.Product.ToString(), 1, true));
            table.AddCell(GenerateCell(PdfColumn.Quantity.ToString(), 1, true));
            table.AddCell(GenerateCell(PdfColumn.UnitPrice.ToString(), 1, true));
            table.AddCell(GenerateCell(PdfColumn.Location.ToString(), 1, true));
            table.AddCell(GenerateCell(PdfColumn.Sum.ToString(), 1, true));
        }        
    }
}
