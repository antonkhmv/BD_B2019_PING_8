using System;
using System.Collections.Generic;
using System.Linq;

public class BookService
{
    private readonly LibraryContext _dbLibraryContext;

    public BookService(LibraryContext dbLibraryContext)
    {
        _dbLibraryContext = dbLibraryContext;
    }

    public List<Book> FindAllBooks()
    {
        List<Book> books = _dbLibraryContext.Book.ToList();
        if (books == null || books.Count == 0)
            throw new Exception("В базе данных нет ни одной книги");
        return books;
    }

    public Book FindBookByIsbn(int isbn)
    {
        Book book = _dbLibraryContext.Book
            .FirstOrDefault(b => b.Isbn == isbn);
        if (book == null)
            throw new Exception("Книги с таким ISBN не существует");
        return book;
    }

    public void UpdateBookTitle(int isbn, string title)
    {
        Book book = FindBookByIsbn(isbn);
        book.Title = title;
        _dbLibraryContext.Book.Update(book);
        _dbLibraryContext.SaveChanges();
    }

    public void UpdateBookAuthor(int isbn, string author)
    {
        Book book = FindBookByIsbn(isbn);
        book.Author = author;
        _dbLibraryContext.Book.Update(book);
        _dbLibraryContext.SaveChanges();
    }

    public void UpdateBookPagesNumber(int isbn, int pagesNum)
    {
        Book book = FindBookByIsbn(isbn);
        book.PagesNum = pagesNum;
        _dbLibraryContext.Book.Update(book);
        _dbLibraryContext.SaveChanges();
    }

    public void UpdateBookPubYear(int isbn, int pubYear)
    {
        Book book = FindBookByIsbn(isbn);
        if (pubYear > DateTime.Now.Year || pubYear <= 1700)
            throw new Exception("Указанный год издания книги недопустим");
        book.PubYear = pubYear;
        _dbLibraryContext.Book.Update(book);
        _dbLibraryContext.SaveChanges();
    }

    public void AddBook(int isbn, string title, string author,
        int pagesNum, int pubYear, string pubName)
    {
        Book entity = _dbLibraryContext.Book
            .FirstOrDefault(o => o.Isbn == isbn);
        if (entity != null)
            throw new Exception("Книга с таким ISBN уже существует");
        Publisher publisher = _dbLibraryContext.Publisher.FirstOrDefault(p => p.PubName == pubName);
        if (publisher == null)
            throw new Exception("Издателя с таким именем не существует");
        Book book = new Book()
        {
            Isbn = isbn,
            Title = title,
            Author = author,
            PagesNum = pagesNum,
            PubYear = pubYear,
            PubName = pubName
        };
        _dbLibraryContext.Book.Add(book);
        _dbLibraryContext.SaveChanges();
    }

    public void RemoveBook(int isbn)
    {
        Book book = FindBookByIsbn(isbn);

        CopyService copyService = new CopyService(_dbLibraryContext);
        copyService.RemoveBooksCopies(isbn);

        List<BookCat> bookCats =
            (from bookCat in _dbLibraryContext.BookCat
                where bookCat.Isbn == isbn
                select bookCat).ToList();
        foreach (BookCat bookCat in bookCats)
            _dbLibraryContext.BookCat.Remove(bookCat);

        BorrowingService borrowingService = new BorrowingService(_dbLibraryContext);
        borrowingService.RemoveBookBorrowings(isbn);

        _dbLibraryContext.Book.Remove(book);
        _dbLibraryContext.SaveChanges();
    }
}