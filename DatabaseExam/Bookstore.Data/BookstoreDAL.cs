using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Data
{
    public class BookstoreDAL
    {
        public static List<Review> FindReviewByAuthor(string authorName)
        {
            BookstoreEntities context = new BookstoreEntities();
            
            var result = (from r in context.Reviews.Include("Author").Include("Book").Include("Book.Authors")
                          where r.Author.Name == authorName
                          select r).ToList();               
            var otherresult = result.OrderBy(r => r.CreationDate).ThenBy(r => r.Text);

            return otherresult.ToList();           
        }

        public static List<Review> FindReviewByDate(string start, string end)
        {
            BookstoreEntities context = new BookstoreEntities();
            
            DateTime startDate = DateTime.Parse(start);
            DateTime endDate = DateTime.Parse(end);

            var result = (from r in context.Reviews.Include("Author").Include("Book").Include("Book.Authors")
                          where r.CreationDate >= startDate && r.CreationDate <= endDate
                          select r).ToList();
            var otherResult = result.OrderBy(r => r.CreationDate).ThenBy(r => r.Text);

            return otherResult.ToList();           
        }

        public static IList<Book> FindBookByTitleAuthorAndIsbn(string title, string author, string isbn)
        {
            BookstoreEntities context = new BookstoreEntities();
            var booksquery =
                            from b in context.Books
                            select b;

            if (title != null)
            {
                booksquery =
                            from b in booksquery
                            where b.Title.ToLower() == title.ToLower()
                            select b;
            }

            if (author != null)
            {
                booksquery = booksquery.Where(
                    b => b.Authors.Any(t => t.Name.ToLower() == author.ToLower()));
            }

            if (isbn != null)
            {
                booksquery =
                            from b in booksquery
                            where b.ISBN == isbn
                            select b;
            }
           
            booksquery = booksquery.OrderBy(b => b.Title);

            return booksquery.ToList();
        }

        // BookstoreEntities
        public static void AddBookComplex(string title, string isbn, string price, string webSite,
            List<string> authorsList, List<Review> reviewsList)
        {
            BookstoreEntities context = new BookstoreEntities();
            using (context)
            {
                List<Author> parsedAuthors = new List<Author>();
                foreach (var item in authorsList)
                {
                    Author addedAuthor = CreateOrLoadAuthor(context, item);
                    parsedAuthors.Add(addedAuthor);
                }

                List<Review> parsedReviews = new List<Review>();
                foreach (var item in reviewsList)
                {
                    if (item != null)
                    {
                        Author addedAuthor = null;
                        if (item.Author != null)
                        {
                            addedAuthor = CreateOrLoadAuthor(context, item.Author.Name);
                        }
                        
                        Review addedReview = new Review()
                        {
                            Text = item.Text,
                            CreationDate = item.CreationDate,
                            Author = addedAuthor
                        };

                        parsedReviews.Add(addedReview);
                    }
                }

                decimal? parsedPrice = null;
                if (price != null)
                {
                    parsedPrice = decimal.Parse(price, CultureInfo.InvariantCulture);
                }
                Book addedBook = new Book()
                {
                    Title = title,
                    Price = parsedPrice,
                    Website = webSite,
                    ISBN = isbn
                };

                foreach (var item in parsedReviews)
                {
                    addedBook.Reviews.Add(item);
                }

                foreach (var item in parsedAuthors)
                {
                    addedBook.Authors.Add(item);
                }

                context.Books.Add(addedBook);
                context.SaveChanges();
            }
        }

        public static void AddBook(string author, string isbn,
            string price, string webSite, string title)
        {
            BookstoreEntities context = new BookstoreEntities();
            using (context)
            {
                Author addedAuthor = CreateOrLoadAuthor(context, author);

                decimal? parsedPrice = null;
                if (price != null)
                {
                    parsedPrice = decimal.Parse(price, CultureInfo.InvariantCulture);
                }
                
                Book addedBook = new Book()
                {
                    ISBN = isbn,
                    Price = parsedPrice,
                    Website = webSite,
                    Title = title,
                };

                addedBook.Authors.Add(addedAuthor);

                context.Books.Add(addedBook);
                context.SaveChanges();
            }
        }

        private static Author CreateOrLoadAuthor(
            BookstoreEntities context, string authorName)
        {
            Author existingUser =
                (from u in context.Authors
                 where u.Name.ToLower() == authorName.ToLower()
                 select u).FirstOrDefault();
            if (existingUser != null)
            {
                return existingUser;
            }

            Author newAuthor = new Author();
            newAuthor.Name = authorName;
            context.Authors.Add(newAuthor);
            context.SaveChanges();

            return newAuthor;
        }
    }
}