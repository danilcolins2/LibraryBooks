using Microsoft.EntityFrameworkCore;


namespace LibraryBooks.Models.DbModels
{
    public class LibraryContext : DbContext
    {
        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<ImageFile> Files => Set<ImageFile>();
        public DbSet<Book> Books => Set<Book>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Borrow> Borrows => Set<Borrow>();

        // Метод OnModelCreating для настройки модели данных
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    // Настройки для таблицы Books
        //    modelBuilder.Entity<Book>()
        //        .Property(b => b.Title)
        //        .IsRequired();

        //    // Настройки для таблицы Users
        //    modelBuilder.Entity<User>()
        //        .Property(u => u.Email)
        //        .IsRequired()
        //        .HasMaxLength(256);

        //    // Настройки для таблицы Borrows
        //    modelBuilder.Entity<Borrow>()
        //        .HasOne(b => b.Book)
        //        .WithMany(b => b.Borrows)
        //        .HasForeignKey(b => b.BookId);

        //    modelBuilder.Entity<Borrow>()
        //        .HasOne(b => b.User)
        //        .WithMany(u => u.Borrows)
        //        .HasForeignKey(b => b.UserId);
        //}
    }
}

