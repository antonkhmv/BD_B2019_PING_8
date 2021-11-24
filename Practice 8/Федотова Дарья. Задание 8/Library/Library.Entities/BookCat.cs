public class BookCat
{
    public int Isbn { get; set; }

    public string CategoryName { get; set; }
    
    public override string ToString()
    {
        return $"BookCat: ISBN = {Isbn}, CategoryName = {CategoryName}";
    }
}