using Microsoft.EntityFrameworkCore;

namespace BookManagement.Models
{
    public class BookDbContext : DbContext
    {
        public BookDbContext(DbContextOptions<BookDbContext> options)
            : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }

        public DbSet<BookImage> BookImages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                .Property(b => b.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<BookImage>()
                .HasOne(image => image.Book)
                .WithMany(book => book.Images)
                .HasForeignKey(image => image.BookId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}