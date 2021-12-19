using Microsoft.EntityFrameworkCore;

public class LibraryContext : DbContext
{
    public DbSet<Book> Book { get; set; }
    public DbSet<BookCat> BookCat { get; set; }
    public DbSet<Borrowing> Borrowing { get; set; }
    public DbSet<Category> Category { get; set; }
    public DbSet<Copy> Copy { get; set; }
    public DbSet<Publisher> Publisher { get; set; }
    public DbSet<Reader> Reader { get; set; }

    public LibraryContext()
    {
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=Library;Username=postgres;Password=1234");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>().HasKey(o => new {o.Isbn});
        modelBuilder.Entity<Category>().HasKey(o => new {o.CategoryName});
        modelBuilder.Entity<Copy>().HasKey(o => new {o.Isbn, o.CopyNumber});
        modelBuilder.Entity<Borrowing>().HasKey(o => new {o.ReaderNr, o.Isbn, o.CopyNumber});
        modelBuilder.Entity<BookCat>().HasKey(o => new {o.Isbn, o.CategoryName});
        modelBuilder.Entity<Publisher>().HasKey(o => new {o.PubName});
    }
}