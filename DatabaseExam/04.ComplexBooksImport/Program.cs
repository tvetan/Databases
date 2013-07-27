using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml;
using Bookstore.Data;

namespace _04.ComplexBooksImport
{
    public static class Program
    {
        static void Main()
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load("../../test4.xml");
                string xPathQueryMain = "/catalog/book";
                XmlNodeList books = xmlDoc.SelectNodes(xPathQueryMain);

                foreach (XmlNode book in books)
                {
                    string title = book.GetChildText("title");

                    if (title == null)
                    {
                        throw new ArgumentException("The title is mandatory");
                    }

                    string isbn = book.GetChildText("isbn");
                    string price = book.GetChildText("price");
                    string webSite = book.GetChildText("web-site");
                    var authors = book.GetChildElements("authors");
                    List<string> authorsList = new List<string>();

                    if (authors != null)
                    {
                        foreach (XmlNode tag in authors)
                        {
                            authorsList.Add(tag.InnerText);
                        }
                    }

                    var reviews = book.GetChildElements("reviews");
                    List<Review> reviewsList = new List<Review>();

                    if (reviews != null)
                    {
                        foreach (XmlNode tag in reviews)
                        {
                            string innerText = tag.InnerText;
                            string author = tag.GetAttibuteText("author");

                            Author addedAuthor = null;
                            if (author != null)
                            {
                                addedAuthor = new Author()
                                {
                                    Name = author
                                };
                            }

                            string dateString = tag.GetAttibuteText("date");

                            DateTime addedDate = DateTime.Now;

                            if (dateString != null)
                            {
                                addedDate = DateTime.Parse(dateString);
                            }

                            Review addedReview = new Review()
                            {
                                Text = innerText,
                                CreationDate = addedDate,
                                Author = addedAuthor
                            };

                            reviewsList.Add(addedReview);
                        }
                    }

                    TransactionScope tran = new TransactionScope(
                        TransactionScopeOption.Required,
                        new TransactionOptions()
                        {
                            IsolationLevel = IsolationLevel.RepeatableRead
                        });

                    using (tran)
                    {
                        BookstoreDAL.AddBookComplex(title, isbn, price, webSite, authorsList, reviewsList);
                        tran.Complete();
                    }
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
            catch (DbEntityValidationException exp)
            {
                Console.WriteLine("There was a problem with the validation of the xml and the transaction was returned");
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

        private static XmlNode GetChildElements(this XmlNode node, string tagName)
        {
            XmlNode childNode = node.SelectSingleNode(tagName);
            if (childNode == null)
            {
                return null;
            }

            return childNode;
        }

        private static string GetAttibuteText(this XmlNode node, string tagName)
        {
            XmlAttribute childNode = node.Attributes[tagName];
            if (childNode == null)
            {
                return null;
            }

            return childNode.InnerText;
        }
    }
}