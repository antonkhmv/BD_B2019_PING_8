using Microsoft.EntityFrameworkCore;

namespace LibraryDB.Domain
{
    public class LibraryDbContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<BookCat> BookCats { get; set; }
        public DbSet<Borrowing> Borrowings { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Copy> Copies { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Reader> Readers { get; set; }

        public LibraryDbContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=LibraryDb;Username=postgres;Password=1234");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Copy>().HasKey(c => new { c.Isbn, c.CopyNumber });
            modelBuilder.Entity<Borrowing>().HasKey(b => new { b.ReaderNr, b.Isbn, b.CopyNumber });
            modelBuilder.Entity<BookCat>().HasKey(b => new { b.Isbn, b.CategoryName });
        }
    }
}