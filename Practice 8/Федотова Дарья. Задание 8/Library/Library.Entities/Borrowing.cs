using System;

public class Borrowing
{
    public int ReaderNr { get; set; }

    public int Isbn { get; set; }

    public int CopyNumber { get; set; }

    public DateTime ReturnDate { get; set; }

    public override string ToString()
    {
        return $"Borrowing: ReaderId = {ReaderNr}, ISBN = {Isbn}, CopyNumber = {CopyNumber}, " +
               $"ReturnDate = {ReturnDate.Date}";
    }
}