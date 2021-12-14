namespace LibraryDB.Domain
{
    public class Copy
    {
        // Идентификационный номер книги данной копии. 
        public int Isbn { get; set; }

        // Номер копии книги.
        public int CopyNumber { get; set; }

        // Позиция на полке.
        public int ShelfPosition { get; set; }
        
        public override string ToString()
            => $"Copy's ISBN = {Isbn}; CopyNumber = {CopyNumber}; " +
               $"ShelfPosition = {ShelfPosition}";
    }
}