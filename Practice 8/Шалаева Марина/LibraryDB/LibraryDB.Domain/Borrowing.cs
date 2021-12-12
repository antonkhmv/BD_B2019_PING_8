using System;

namespace LibraryDB.Domain
{
    public class Borrowing
    {
        // Идентификационный номер читателя. 
        public int ReaderNr { get; set; }

        // Идентификационный номер книги данной копии.
        public int Isbn { get; set; }

        // Номер взятой копии.
        public int CopyNumber { get; set; }
        
        // Дата возврата копии книги.
        public DateTime ReturnDate{ get; set; }
        
        public override string ToString()
            => $"Borrowing's ReaderNr = {ReaderNr}; ISBN = {Isbn}; CopyNumber = {CopyNumber}; " +
               $"ReturnDate = {ReturnDate.Date}";
    }
}