using System.ComponentModel.DataAnnotations;

namespace LibraryDB.Domain
{
    public class Publisher
    {
        // Имя издательства.
        [Key] public string PubName { get; set; }

        // Адрес издательства.
        public string PubAddress { get; set; }

        public override string ToString()
            => $"Publisher's name: {PubName}; address: {PubAddress}";
    }
}