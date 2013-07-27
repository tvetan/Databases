using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Bookstore.Data;

namespace _05.SimpleSearchBooks
{
    public static class Program
    {
        static void Main()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("../../test6.xml");
            string title = xmlDoc.GetChildText("/query/title");
            string author = xmlDoc.GetChildText("/query/author");
            string isbn = xmlDoc.GetChildText("/query/isbn");
            var books = BookstoreDAL.FindBookByTitleAuthorAndIsbn(title, author, isbn);

            if (books.Count == 0)
            {
                Console.WriteLine("Nothing found");
            }
            else
            {
                foreach (var item in books)
                {
                    Console.WriteLine("{0} --> {1} reviews", item.Title, item.Reviews.Count);
                }
            }
        }

        private static string GetChildText(this XmlNode node, string tagName)
        {
            XmlNode childNode = node.SelectSingleNode(tagName);
            if (childNode == null)
            {
                return null;
            }

            return childNode.InnerText.Trim();
        }
    }
}