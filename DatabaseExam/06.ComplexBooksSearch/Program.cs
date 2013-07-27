using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Bookstore.Data;
using _07.LogsData;
using _07.LogsModel;

namespace _06.ComplexBooksSearch
{
    static class Program
    {
        // 1 SELECT for each search-result if the part with 07 is commented
        static void Main()
        {
            string fileName = "../../reviews-search-results.xml";

            using (XmlTextWriter writer =
                new XmlTextWriter(fileName, Encoding.UTF8))
            {
                writer.Formatting = Formatting.Indented;
                writer.IndentChar = '\t';
                writer.Indentation = 1;

                writer.WriteStartDocument();
                writer.WriteStartElement("search-results");

                ProcessSearchQueries(writer);

                writer.WriteEndDocument();
            }
        }

        private static void ProcessSearchQueries(XmlTextWriter writer)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("../../test1.xml");

            //// 07 start
            //LogsContext logsContext = new LogsContext();
            ////  07 end

            foreach (XmlNode query in xmlDoc.SelectNodes("/review-queries/query"))
            {
                //// 07 start
                //string logText = "<query type=\"" + query.Attributes["type"].Value + "\">" + query.InnerXml + "</query>";
                //SearchLog log = new SearchLog()
                //{
                //    Date = DateTime.Now,
                //    QueryXml = logText
                //};
                //logsContext.Logs.Add(log);
                //logsContext.SaveChanges();
                //// 07 end

                List<Review> resultReviews = new List<Review>();

                if (query.Attributes["type"].Value == "by-period")
                {
                    string start = query.GetChildText("start-date");
                    string end = query.GetChildText("end-date");

                    resultReviews = BookstoreDAL.FindReviewByDate(start, end);
                }
                else
                {
                    string authorName = query.GetChildText("author-name");
                    resultReviews = BookstoreDAL.FindReviewByAuthor(authorName);
                }
              
                WriteReviews(writer, resultReviews);              
            }
        }

        private static void WriteReviews(
            XmlTextWriter writer, IList<Review> reviews)
        {
            writer.WriteStartElement("result-set");
            foreach (var review in reviews)
            {
                writer.WriteStartElement("review");

                if (review.CreationDate != null)
                {
                    writer.WriteElementString("date",
                        review.CreationDate.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture));
                }

                if (review.Text != null)
                {
                    writer.WriteElementString("content", review.Text);
                }

                if (review.Book != null)
                {
                    writer.WriteStartElement("book");

                    if (review.Book.Title != null)
                    {
                        writer.WriteElementString("title", review.Book.Title);
                    }

                    if (review.Book.Authors != null && review.Book.Authors.Count != 0)
                    {
                        var authors = review.Book.Authors;
                        List<string> authorNames = new List<string>();
                        foreach (var author in authors)
                        {
                            authorNames.Add(author.Name);
                        }

                        authorNames.Sort();
                        writer.WriteElementString("authors", string.Join(", ", authorNames));
                    }

                    if (review.Book.ISBN != null)
                    {
                        writer.WriteElementString("isbn", review.Book.ISBN);
                    }
                   
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
            }

            writer.WriteEndElement();
        }

        private static string GetChildText(
            this XmlNode node, string xpath)
        {
            XmlNode childNode = node.SelectSingleNode(xpath);
            if (childNode == null)
            {
                return null;
            }

            return childNode.InnerText.Trim();
        }
    }
}