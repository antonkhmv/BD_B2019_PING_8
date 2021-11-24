public class Copy
{
    public int Isbn { get; set; }

    public int CopyNumber { get; set; }

    public int ShelfPosition { get; set; }

    public override string ToString()
    {
        return $"Copy: ISBN = {Isbn}, CopyNumber = {CopyNumber}, ShelfPosition = {ShelfPosition}";
    }
}