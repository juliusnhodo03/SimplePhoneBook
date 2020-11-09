using Microsoft.EntityFrameworkCore;
using SimplePhoneBook.API.Data.Entities;

namespace SimplePhoneBook.API.Data
{
    public class PhoneBookContext : DbContext
    {
        public PhoneBookContext(DbContextOptions<PhoneBookContext> options) : base(options)
        {
        }

        public virtual DbSet<Entities.PhoneBook> PhoneBooks { get; set; }
        public virtual DbSet<Entry> Entries { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // PhoneBook
            modelBuilder.Entity<Entities.PhoneBook>(g =>
            {
                g.HasKey(e => e.PhoneBookId);
                g.ToTable("PhoneBook");
            });

            modelBuilder.Entity<Entities.PhoneBook>()
                .HasMany(c => c.Entries)
                .WithOne(e => e.PhoneBook)
                .IsRequired();

            modelBuilder.Entity<Entry>()
                .HasIndex(b => b.PhoneNumber)
                .IsUnique();

            modelBuilder.Entity<Entry>()
                .HasIndex(b => b.Name);

            // Entry
            modelBuilder.Entity<Entry>(b =>
            {
                b.ToTable("Entry");
                b.HasKey(e => e.EntryId);
            });
        }
    }
}
