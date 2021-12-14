using System;
using System.Collections.Generic;
using System.Linq;
using LibraryDB.Domain;

namespace LibraryDB.Application
{
    public class BorrowingService
    {
        private readonly LibraryDbContext _db;

        public BorrowingService(LibraryDbContext db)
        {
            _db = db;
        }

        public List<Borrowing> GetAllBorrowings()
            => _db.Borrowings.ToList();

        public Borrowing FindBorrowingByHoleKey(int readerNum, int isbn, int copyNum)
            => _db.Borrowings.FirstOrDefault(o => o.ReaderNr == readerNum &&
                                                  o.Isbn == isbn &&
                                                  o.CopyNumber == copyNum)!;

        public List<int> FindBooksIsbnWhichWereBorrowedByReader(int readerNum)
            => (from borrow in _db.Borrowings
                where borrow.ReaderNr == readerNum
                select borrow.Isbn)!.ToList();

        public List<int> FindNumbersOfReadersWhoBorrowedBook(int isbn)
            => (from borrow in _db.Borrowings
                where borrow.Isbn == isbn
                select borrow.ReaderNr)!.ToList();

        public List<int> FindNumbersOfReadersWhoBorrowedCopy(int isbn, int copyNum)
            => (from borrow in _db.Borrowings
                where borrow.Isbn == isbn &&
                      borrow.CopyNumber == copyNum
                select borrow.ReaderNr)!.ToList();

        public Borrowing FindBorrowingByReader(int isbn)
            => _db.Borrowings.FirstOrDefault(o => o.Isbn == isbn)!;

        private void AddNewBorrowing(Borrowing borrowing)
        {
            _db.Borrowings.Add(borrowing);
            _db.SaveChanges();
        }

        public void AddNewBorrowing(int readerNr, int isbn, int copyNumber, DateTime returnDate)
        {
            var res = _db.Borrowings.FirstOrDefault(b => b.ReaderNr == readerNr &&
                                                         b.Isbn == isbn &&
                                                         b.CopyNumber == copyNumber);
            if (res != null)
                throw new Exception("Нельзя добавить бронирование, которое уже есть в базе данных!");

            Borrowing borrowing = new()
            {
                ReaderNr = readerNr,
                Isbn = isbn,
                CopyNumber = copyNumber,
                ReturnDate = returnDate
            };
            AddNewBorrowing(borrowing);
        }

        public void DeleteBorrowing(int readerNr, int isbn, int copyNumber)
        {
            var res = (from borrow in _db.Borrowings
                where borrow.ReaderNr == readerNr &&
                      borrow.Isbn == isbn &&
                      borrow.CopyNumber == copyNumber
                select borrow).ToList();

            if (res == null || res.Count <= 0)
                throw new Exception("Нельзя удалить бронирование, которого нет в базе данных!");

            _db.Borrowings.Remove(res[0]);
            _db.SaveChanges();
        }

        public void DeleteBorrowingByReader(int readerNr)
        {
            var res = (from borrow in _db.Borrowings
                where borrow.ReaderNr == readerNr
                select borrow).ToList();

            if (res == null || res.Count <= 0)
                throw new Exception("Нельзя удалить бронирование, котороого нет в базе данных!");
            foreach (var borrow in res)
                _db.Borrowings.Remove(borrow);

            _db.SaveChanges();
        }

        public void DeleteBorrowingByBookIsbn(int isbn)
        {
            var res = (from borrow in _db.Borrowings
                where borrow.Isbn == isbn
                select borrow).ToList();

            if (res == null || res.Count <= 0)
                throw new Exception("Нельзя удалить информацию о бронировании экземпляров" +
                                    " книги, которой нет в базе данных!");
            foreach (var borrow in res)
                _db.Borrowings.Remove(borrow);

            _db.SaveChanges();
        }

        public void DeleteBorrowingByCopy(int isbn, int copyNumber)
        {
            var res = (from borrow in _db.Borrowings
                where borrow.Isbn == isbn &&
                      borrow.CopyNumber == copyNumber
                select borrow).ToList();

            if (res == null || res.Count <= 0)
                throw new Exception("Нельзя удалить информацию о бронировании " +
                                    "экземпляра книги, которого нет в базе данных!");
            foreach (var borrow in res)
            {
                _db.Borrowings.Remove(borrow);
            }

            _db.SaveChanges();
        }
    }
}