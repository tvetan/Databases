using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Bookstore.Data;

namespace _03.BooksImporting
{
    public static class Program
    {
        static void Main()
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load("../../test5.xml");
                string xPathQuery = "/catalog/book";

                XmlNodeList books = xmlDoc.SelectNodes(xPathQuery);
                foreach (XmlNode book in books)
                {
                    string author = book.GetChildText("author");
                    string title = book.GetChildText("title");

                    if (author == null || title == null)
                    {
                        throw new ArgumentException("The author and the title have to be given");
                    }

                    string isbn = book.GetChildText("isbn");
                    string price = book.GetChildText("price");
                    string webSite = book.GetChildText("web-site");

                    BookstoreDAL.AddBook(author, isbn, price, webSite, title);
                }
            }
            catch (XmlException exp)
            {
                Console.WriteLine(exp.Message);
            }
            catch (ArgumentException exp)
            {
                Console.WriteLine(exp.Message);
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
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