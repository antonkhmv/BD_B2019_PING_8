using System;

public class Reader
{
    public int Id { get; set; }

    public string LastName { get; set; }

    public string FirstName { get; set; }

    public string Address { get; set; }

    public DateTime BirthDate { get; set; }

    public override string ToString()
    {
        return $"Reader: Id = {Id}, LastName = {LastName}, FirstName = {FirstName}, " +
               $"Address = {Address}, BirthDate = {BirthDate.Date}";
    }
}