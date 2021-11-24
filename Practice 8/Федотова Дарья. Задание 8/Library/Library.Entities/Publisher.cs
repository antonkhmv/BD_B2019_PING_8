public class Publisher
{
    public string PubName { get; set; }

    public string PubAddress { get; set; }

    public override string ToString()
    {
        return $"Publisher: Name = {PubName}, Address = {PubAddress}";
    }
}