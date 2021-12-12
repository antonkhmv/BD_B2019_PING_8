using System;
using System.Collections.Generic;
using System.Linq;
using LibraryDB.Domain;

namespace LibraryDB.Application
{
    public class BookService
    {
        private readonly LibraryDbContext _db;

        public BookService(LibraryDbContext db)
        {
            _db = db;
        }

        public List<Book> GetAllBooks()
            => _db.Books.ToList();

        public Book FindBookByIsbn(int isbn)
            => _db.Books.FirstOrDefault(o => o.Isbn == isbn)!;

        public List<Book> FindBookByCategoryName(string categoryName)
        {
            var isbns = (from bookCat in _db.BookCats
                where bookCat.CategoryName == categoryName
                select bookCat.Isbn).ToList();
            return isbns.Select(FindBookByIsbn).ToList();
        }

        private void AddNewBook(Book book)
        {
            _db.Books.Add(book);
            _db.SaveChanges();
        }

        public void AddNewBook(int isbn, string title, string author,
            int pagesNum, int pubYear, string pubName)
        {
            var res = _db.Books.FirstOrDefault(o => o.Isbn == isbn);
            if (res != null)
                throw new Exception("Книга с таким ISBN уже присутствует в базе данных!");
            Book book = new()
            {
                Isbn = isbn,
                Title = title,
                Author = author,
                PagesNum = pagesNum,
                PubYear = pubYear,
                PubName = pubName
            };
            AddNewBook(book);
        }

        public void DeleteBookByIsbn(int isbn)
        {
            var res = (from book in _db.Books
                where book.Isbn == isbn
                select book).ToList();
            if (res == null || res.Count <= 0)
                throw new Exception("Нельзя удалить книгу, которой нет в базе данных!");

            CopyService copyService = new CopyService(_db);
            copyService.DeleteCopyByIsbn(isbn);

            var bookCats = (from bookCat in _db.BookCats
                where bookCat.Isbn == isbn
                select bookCat).ToList();
            foreach (var bookCat in bookCats)
                _db.BookCats.Remove(bookCat);

            BorrowingService borrowingService = new BorrowingService(_db);
            borrowingService.DeleteBorrowingByBookIsbn(isbn);

            _db.Books.Remove(res[0]);
            _db.SaveChanges();
        }

        public void UpdateBooksIsbn(int oldIsbn, int newIsbn)
        {
            var editingBook = _db.Books.FirstOrDefault(b => b.Isbn == oldIsbn);
            var bookWithNewIsbn = _db.Books.FirstOrDefault(b => b.Isbn == newIsbn);
            if (editingBook == null)
                throw new Exception("Нельзя изменить книгу, которой нет в базе данных!");

            if (bookWithNewIsbn != null)
                throw new Exception("Нельзя изменить ISBN книги на ISBN, который уже есть в базе данных!");

            editingBook.Isbn = newIsbn;
            _db.Books.Update(editingBook);

            var copies = (from copy in _db.Copies
                where copy.Isbn == oldIsbn
                select copy).ToList();
            foreach (var item in copies)
            {
                item.Isbn = newIsbn;
                _db.Copies.Update(item);
            }

            var bookCats = (from bookCat in _db.BookCats
                where bookCat.Isbn == oldIsbn
                select bookCat).ToList();
            foreach (var item in bookCats)
            {
                item.Isbn = newIsbn;
                _db.BookCats.Update(item);
            }

            var borrowings = (from borrow in _db.Borrowings
                where borrow.Isbn == oldIsbn
                select borrow).ToList();
            foreach (var item in borrowings)
            {
                item.Isbn = newIsbn;
                _db.Borrowings.Update(item);
            }

            _db.SaveChanges();
        }

        public void UpdateBooksTitle(int isbn, string title)
        {
            var editingBook = _db.Books.FirstOrDefault(b => b.Isbn == isbn);
            if (editingBook == null)
                throw new Exception("Нельзя изменить книгу, которой нет в базе данных!");
            editingBook.Title = title;
            _db.Books.Update(editingBook);
            _db.SaveChanges();
        }

        public void UpdateBooksAuthor(int isbn, string author)
        {
            var editingBook = _db.Books.FirstOrDefault(b => b.Isbn == isbn);
            if (editingBook == null)
                throw new Exception("Нельзя изменить книгу, которой нет в базе данных!");
            editingBook.Author = author;
            _db.Books.Update(editingBook);
            _db.SaveChanges();
        }

        public void UpdateBooksPagesNumber(int isbn, int pagesNum)
        {
            var editingBook = _db.Books.FirstOrDefault(b => b.Isbn == isbn);
            if (editingBook == null)
                throw new Exception("Нельзя изменить книгу, которой нет в базе данных!");
            editingBook.PagesNum = pagesNum;
            _db.Books.Update(editingBook);
            _db.SaveChanges();
        }

        public void UpdateBooksPubYear(int isbn, int pubYear)
        {
            var editingBook = _db.Books.FirstOrDefault(b => b.Isbn == isbn);
            if (editingBook == null)
                throw new Exception("Нельзя изменить книгу, которой нет в базе данных!");
            if (pubYear > 2021 || pubYear <= 1800)
                throw new Exception("Был указан недопустимый год издания книги!");
            editingBook.PubYear = pubYear;
            _db.Books.Update(editingBook);
            _db.SaveChanges();
        }
    }
}