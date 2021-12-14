using System.ComponentModel.DataAnnotations;

namespace LibraryDB.Domain
{
    public class Book
    {
        // Идентификационный номер книги.
        [Key] public int Isbn { get; set; }

        // Название книги.
        public string Title { get; set; }

        // Автор книги.
        public string Author { get; set; }

        // Количество страниц в книге.
        public int PagesNum { get; set; }

        // Год публикации книги.
        public int PubYear { get; set; }

        // Название издательства, выпустившего книгу.
        public string PubName { get; set; }

        public override string ToString()
            => $"Book's ISBN = {Isbn}; Title = {Title}; " +
               $"Author = {Author}; PagesNum = {PagesNum}; PubYear = {PubYear}; " +
               $"PubName = {PubName}";
    }
}