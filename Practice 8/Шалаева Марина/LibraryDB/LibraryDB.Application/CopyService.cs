using System;
using System.Collections.Generic;
using System.Linq;
using LibraryDB.Domain;

namespace LibraryDB.Application
{
    public class CopyService
    {
        private readonly LibraryDbContext _db;

        public CopyService(LibraryDbContext db)
        {
            _db = db;
        }

        public List<Copy> GetAllCopies()
            => _db.Copies.ToList();

        public Copy FindCopyByIsbnAndCopyNumber(int isbn, int copyNum)
            => _db.Copies.FirstOrDefault(o => o.Isbn == isbn &&
                                              o.CopyNumber == copyNum)!;

        public List<Copy> FindCopiesByIsbn(int isbn)
            => (from copy in _db.Copies
                where copy.Isbn == isbn
                select copy)!.ToList();

        private void AddNewCopy(Copy copy)
        {
            _db.Copies.Add(copy);
            _db.SaveChanges();
        }

        public void AddNewCopy(int isbn, int shelfPosition)
        {
            // TODO: Не забыть использовать try-catch при вызове функции!

            var res = (from item in _db.Copies
                where item.Isbn == isbn
                select item.CopyNumber).ToList();

            if (res == null || res.Count == 0)
                throw new Exception("Книги с таким ISBN нет в базе данных!");

            Copy copy = new()
            {
                Isbn = isbn,
                CopyNumber = res.Max() + 1,
                ShelfPosition = shelfPosition
            };
            AddNewCopy(copy);
        }

        public void DeleteCopyByIsbn(int isbn)
        {
            var res = (from copy in _db.Copies
                where copy.Isbn == isbn
                select copy).ToList();
            if (res == null || res.Count <= 0)
                throw new Exception("Нельзя удалить копии той книги, которой нет в базе данных!");

            foreach (var copy in res)
                _db.Copies.Remove(copy);

            _db.SaveChanges();
        }

        public void DeleteCopyByIsbnAndCopyNumber(int isbn, int copyNumber)
        {
            var res = (from copy in _db.Copies
                where copy.Isbn == isbn
                      && copy.CopyNumber == copyNumber
                select copy).ToList();

            if (res == null || res.Count <= 0)
                throw new Exception("Нельзя удалить копию, которой нет в базе данных!");

            foreach (var copy in res)
                _db.Copies.Remove(copy);

            _db.SaveChanges();
        }

        public void UpdateCopyNumber(int isbn, int oldCopyNumber, int newCopyNumber)
        {
            var editingCopy = _db.Copies.FirstOrDefault(c => (c.Isbn == isbn) &&
                                                             (c.CopyNumber == oldCopyNumber));
            if (editingCopy == null)
                return;
            editingCopy.CopyNumber = newCopyNumber;
            _db.Copies.Update(editingCopy);

            var borrowings = (from borrow in _db.Borrowings
                where borrow.Isbn == isbn &&
                      borrow.CopyNumber == oldCopyNumber
                select borrow).ToList();

            foreach (var item in borrowings)
            {
                item.CopyNumber = newCopyNumber;
                _db.Borrowings.Update(item);
            }

            _db.SaveChanges();
        }

        public void UpdateCopyShelfPosition(int isbn, int oldShelfPosition, int newShelfPosition)
        {
            var editingCopy = _db.Copies.FirstOrDefault(c => (c.Isbn == isbn) &&
                                                             (c.ShelfPosition == oldShelfPosition));
            if (editingCopy == null)
                return;
            editingCopy.ShelfPosition = newShelfPosition;
            _db.Copies.Update(editingCopy);

            _db.SaveChanges();
        }
    }
}