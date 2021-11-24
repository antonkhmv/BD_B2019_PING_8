using System;
using System.Collections.Generic;
using System.Linq;

namespace Library.Entities
{
    public class LibraryService
    {
        private readonly LibraryContext _dbLibraryContext;

        public LibraryService(LibraryContext dbLibraryContext)
        {
            _dbLibraryContext = dbLibraryContext;
        }

        public void PrintAllTables()
        {
            bool infoExist = false;

            List<Book> books = _dbLibraryContext.Book.ToList();
            if (books != null && books.Count > 0)
            {
                infoExist = true;
                foreach (var book in books)
                    Console.WriteLine(book.ToString());
                Console.WriteLine();
            }

            List<BookCat> bookCats = _dbLibraryContext.BookCat.ToList();
            if (bookCats != null && bookCats.Count > 0)
            {
                infoExist = true;
                foreach (var bookCat in bookCats)
                    Console.WriteLine(bookCat.ToString());
                Console.WriteLine();
            }

            List<Borrowing> borrowings = _dbLibraryContext.Borrowing.ToList();
            if (borrowings != null && borrowings.Count > 0)
            {
                infoExist = true;
                foreach (var borrowing in borrowings)
                    Console.WriteLine(borrowing.ToString());
                Console.WriteLine();
            }

            List<Category> categories = _dbLibraryContext.Category.ToList();
            if (categories != null && categories.Count > 0)
            {
                infoExist = true;
                foreach (var category in categories)
                    Console.WriteLine(category.ToString());
                Console.WriteLine();
            }

            List<Copy> copies = _dbLibraryContext.Copy.ToList();
            if (copies != null && copies.Count > 0)
            {
                infoExist = true;
                foreach (var copy in copies)
                    Console.WriteLine(copy.ToString());
                Console.WriteLine();
            }

            List<Publisher> publishers = _dbLibraryContext.Publisher.ToList();
            if (publishers != null && publishers.Count > 0)
            {
                infoExist = true;
                foreach (var publisher in publishers)
                    Console.WriteLine(publisher.ToString());
                Console.WriteLine();
            }

            List<Reader> readers = _dbLibraryContext.Reader.ToList();
            if (readers != null && readers.Count > 0)
            {
                infoExist = true;
                foreach (var reader in readers)
                    Console.WriteLine(reader.ToString());
                Console.WriteLine();
            }

            if (!infoExist)
                Console.WriteLine("В базе данных нет ни одной записи");
        }

        public void PrintAllBooks()
        {
            BookService bookService = new BookService(_dbLibraryContext);
            List<Book> books = bookService.FindAllBooks();
            if (books != null && books.Count > 0)
            {
                foreach (var book in books)
                    Console.WriteLine(book.ToString());
            }
            else
            {
                Console.WriteLine("В базе данных нет ни одной книги");
            }
        }

        public void PrintAllCopies()
        {
            CopyService copyService = new CopyService(_dbLibraryContext);
            List<Copy> copies = copyService.FindAllCopies();
            if (copies != null && copies.Count > 0)
            {
                foreach (var copy in copies)
                    Console.WriteLine(copy.ToString());
            }
            else
            {
                Console.WriteLine("В базе данных нет ни одной копии");
            }
        }

        public void PrintAllBorrowings()
        {
            BorrowingService borrowingService = new BorrowingService(_dbLibraryContext);
            List<Borrowing> borrowings = borrowingService.FindAllBorrowings();
            if (borrowings != null && borrowings.Count > 0)
            {
                foreach (var borrowing in borrowings)
                    Console.WriteLine(borrowing.ToString());
            }
            else
            {
                Console.WriteLine("В базе данных нет ни одного бронирования");
            }
        }
    }
}