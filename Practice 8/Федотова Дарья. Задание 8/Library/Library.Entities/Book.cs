public class Book
{
    public int Isbn { get; set; }

    public string Title { get; set; }

    public string Author { get; set; }

    public int PagesNum { get; set; }

    public int PubYear { get; set; }

    public string PubName { get; set; }

    public override string ToString()
    {
        return $"Book: ISBN = {Isbn}, Title = {Title}, Author = {Author}," +
               $" PagesNum = {PagesNum}, PubYear = {PubYear}, PubName = {PubName}";
    }
}