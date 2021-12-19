using System;
using System.Collections.Generic;
using System.Linq;

public class BorrowingService
{
    private readonly LibraryContext _dbLibraryContext;

    public BorrowingService(LibraryContext dbLibraryContext)
    {
        _dbLibraryContext = dbLibraryContext;
    }

    public List<Borrowing> FindAllBorrowings()
    {
        List<Borrowing> borrowings = _dbLibraryContext.Borrowing.ToList();
        if (borrowings == null || borrowings.Count == 0)
            throw new Exception("В базе данных нет ни одного бронирования");
        return borrowings;
    }

    public Borrowing FindBorrowingByHoleKey(int readerNum, int isbn, int copyNum)
    {
        return _dbLibraryContext.Borrowing
            .FirstOrDefault(borrowing => borrowing.ReaderNr == readerNum &&
                                         borrowing.Isbn == isbn &&
                                         borrowing.CopyNumber == copyNum);
    }

    public List<int> FindBorrowedBooks(int readerNum)
    {
        List<int> isbns =
            (from borrowing in _dbLibraryContext.Borrowing
                where borrowing.ReaderNr == readerNum
                select borrowing.Isbn).ToList();
        if (isbns == null || isbns.Count == 0)
            throw new Exception("Читатель с таким номером не брал ни одной книги");
        return isbns;
    }

    public List<int> FindReadersWhoBorrowedBook(int isbn)
    {
        List<int> readersId =
            (from borrowing in _dbLibraryContext.Borrowing
                where borrowing.Isbn == isbn
                select borrowing.ReaderNr).ToList();
        if (readersId == null || readersId.Count == 0)
            throw new Exception("Данную книгу ни разу не бронировали");
        return readersId;
    }

    public List<int> FindReadersWhoBorrowedCopy(int isbn, int copyNum)
    {
        List<int> readersId =
            (from borrowing in _dbLibraryContext.Borrowing
                where borrowing.Isbn == isbn &&
                      borrowing.CopyNumber == copyNum
                select borrowing.ReaderNr).ToList();
        if (readersId == null || readersId.Count == 0)
            throw new Exception("Данную копию книги ни разу не бронировали");
        return readersId;
    }

    public void UpdateBorrowingReturnDate(int readerNr, int isbn, int copyNumber, DateTime newReturnDate)
    {
        Borrowing borrowing = FindBorrowingByHoleKey(readerNr, isbn, copyNumber);
        if (borrowing == null) throw new Exception("Данного бронирования не существует");
        borrowing.ReturnDate = newReturnDate;
        _dbLibraryContext.Borrowing.Update(borrowing);
        _dbLibraryContext.SaveChanges();
    }

    public void AddBorrowing(int readerNr, int isbn, int copyNumber, DateTime returnDate)
    {
        Borrowing entity = FindBorrowingByHoleKey(readerNr, isbn, copyNumber);
        if (entity != null) throw new Exception("Нельзя добавить существующее бронирование");
        Borrowing borrowing = new Borrowing()
        {
            ReaderNr = readerNr,
            Isbn = isbn,
            CopyNumber = copyNumber,
            ReturnDate = returnDate
        };
        _dbLibraryContext.Borrowing.Add(borrowing);
        _dbLibraryContext.SaveChanges();
    }

    public void RemoveBorrowing(int readerNr, int isbn, int copyNumber)
    {
        Borrowing borrowing = FindBorrowingByHoleKey(readerNr, isbn, copyNumber);
        if (borrowing == null) throw new Exception("Данного бронирования не существует");
        _dbLibraryContext.Borrowing.Remove(borrowing);
        _dbLibraryContext.SaveChanges();
    }

    public void RemoveReaderBorrowings(int readerNr)
    {
        List<Borrowing> borrowings =
            (from borrow in _dbLibraryContext.Borrowing
                where borrow.ReaderNr == readerNr
                select borrow).ToList();
        if (borrowings == null || borrowings.Count <= 0) throw new Exception("Данного бронирования не существует");
        foreach (Borrowing borrowing in borrowings)
            _dbLibraryContext.Borrowing.Remove(borrowing);
        _dbLibraryContext.SaveChanges();
    }

    public void RemoveBookBorrowings(int isbn)
    {
        List<Borrowing> borrowings =
            (from borrow in _dbLibraryContext.Borrowing
                where borrow.Isbn == isbn
                select borrow).ToList();
        if (borrowings == null || borrowings.Count <= 0) throw new Exception("Данную книгу не бронировали");
        foreach (Borrowing borrowing in borrowings)
            _dbLibraryContext.Borrowing.Remove(borrowing);
        _dbLibraryContext.SaveChanges();
    }

    public void RemoveCopyBorrowing(int isbn, int copyNumber)
    {
        List<Borrowing> borrowings =
            (from borrow in _dbLibraryContext.Borrowing
                where borrow.Isbn == isbn &&
                      borrow.CopyNumber == copyNumber
                select borrow).ToList();
        if (borrowings == null || borrowings.Count <= 0) throw new Exception("Данную копию не бронировали");
        foreach (Borrowing borrowing in borrowings)
            _dbLibraryContext.Borrowing.Remove(borrowing);
        _dbLibraryContext.SaveChanges();
    }
}