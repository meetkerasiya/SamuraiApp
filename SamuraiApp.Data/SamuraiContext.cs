using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SamuraiApp.Domain;

namespace SamuraiApp.Data
{
    public class SamuraiContext : DbContext
    {
        public DbSet<Samurai> Samurais { get; set; }

        public DbSet<Battle> Battles { get; set; }
        public DbSet<Quote> Quotes { get; set; }

        private StreamWriter _writer=new StreamWriter("EFCoreLog.txt",append:true);
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {


            optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB; Initial Catalog=SamuraiAppData")
                    .LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name },
                                    LogLevel.Information)
                    .EnableSensitiveDataLogging();
                    //.LogTo(_writer.WriteLine);
                    //if i remove this above comment it'll override previous call to LogTo for console
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Samurai>()
                .HasMany(s => s.Battles)
                .WithMany(b => b.Samurais)
                .UsingEntity<BattleSamurai>
                (bs => bs.HasOne<Battle>().WithMany(),
                bs => bs.HasOne<Samurai>().WithMany())
                .Property(bs => bs.DateJoined)
                .HasDefaultValueSql("getdate()");
        }
    }
}
