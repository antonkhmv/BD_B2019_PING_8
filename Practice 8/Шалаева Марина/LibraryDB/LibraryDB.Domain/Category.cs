using System.ComponentModel.DataAnnotations;

namespace LibraryDB.Domain
{
    public class Category
    {
        // Имя категории.
        [Key] public string CategoryName { get; set; }

        // Родительская категория.
        public Category ParentCat { get; set; }
    }
}