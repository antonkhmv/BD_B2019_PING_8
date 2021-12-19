using System;
using System.Collections.Generic;
using System.Linq;

public class CopyService
{
    private readonly LibraryContext _dbLibraryContext;

    public CopyService(LibraryContext dbLibraryContext)
    {
        _dbLibraryContext = dbLibraryContext;
    }

    public List<Copy> FindAllCopies()
    {
        List<Copy> copies = _dbLibraryContext.Copy.ToList();
        if (copies == null || copies.Count == 0)
            throw new Exception("В базе данных нет ни одной копии книги");
        return copies;
    }

    public Copy FindCopyByHoleKey(int isbn, int copyNum)
    {
        Copy copy = _dbLibraryContext.Copy
            .FirstOrDefault(o => o.Isbn == isbn &&
                                 o.CopyNumber == copyNum);
        if (copy == null) throw new Exception("Данной копии книги не существует");
        return copy;
    }

    public List<Copy> FindCopiesByIsbn(int isbn)
    {
        List<Copy> copies =
            (from copy in _dbLibraryContext.Copy
                where copy.Isbn == isbn
                select copy).ToList();
        if (copies == null || copies.Count == 0)
            throw new Exception("В базе данных нет ни одной копии книги с таким ISBN");
        return copies;
    }

    public void UpdateShelfPosition(int isbn, int copyNum, int newShelfPosition)
    {
        Copy copy = FindCopyByHoleKey(isbn, copyNum);
        copy.ShelfPosition = newShelfPosition;
        _dbLibraryContext.Copy.Update(copy);
        _dbLibraryContext.SaveChanges();
    }

    public void AddCopy(int isbn, int shelfPosition)
    {
        List<int> copies =
            (from item in _dbLibraryContext.Copy
                where item.Isbn == isbn
                select item.CopyNumber).ToList();
        if (copies == null || copies.Count == 0)
            throw new Exception("Книги с таким ISBN не существует");
        Copy copy = new Copy()
        {
            Isbn = isbn,
            CopyNumber = copies.Max() + 1,
            ShelfPosition = shelfPosition
        };
        _dbLibraryContext.Copy.Add(copy);
        _dbLibraryContext.SaveChanges();
    }

    public void RemoveBooksCopies(int isbn)
    {
        List<Copy> copies = FindCopiesByIsbn(isbn);
        foreach (Copy copy in copies)
            _dbLibraryContext.Copy.Remove(copy);
        _dbLibraryContext.SaveChanges();
    }

    public void RemoveCopyByIsbnAndCopyNumber(int isbn, int copyNumber)
    {
        Copy copy = FindCopyByHoleKey(isbn, copyNumber);
        _dbLibraryContext.Copy.Remove(copy);
        _dbLibraryContext.SaveChanges();
    }
}