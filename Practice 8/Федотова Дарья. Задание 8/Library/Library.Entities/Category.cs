public class Category
{
    public string CategoryName { get; set; }

    public Category ParentCat { get; set; }

    public override string ToString()
    {
        return ParentCat == null
            ? $"Category: CategoryName = {CategoryName}, " +
              $"ParentCat = {null}"
            : $"Category: CategoryName = {CategoryName}, " +
              $"ParentCat = {ParentCat.CategoryName}";
    }
}