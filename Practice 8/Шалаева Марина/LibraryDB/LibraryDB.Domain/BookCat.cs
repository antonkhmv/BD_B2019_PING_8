namespace LibraryDB.Domain
{
    public class BookCat
    {
        // Идентификационный номер книги.
        public int Isbn { get; set; }
        
        // Имя категории.
        public string CategoryName { get; set; }
    }
}