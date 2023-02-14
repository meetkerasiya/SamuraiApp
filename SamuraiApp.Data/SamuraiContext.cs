using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SamuraiApp.Domain;

namespace SamuraiApp.Data
{
    public class SamuraiContext : DbContext
    {
        public DbSet<Samurai> Samurais { get; set; }

        public DbSet<Quote> Quotes { get; set; }

        private StreamWriter _writer=new StreamWriter("EFCoreLog.txt",append:true);
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {


            optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB; Initial Catalog=SamuraiAppData")
                    .LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name },
                                    LogLevel.Information)
                    .EnableSensitiveDataLogging();
                    //.LogTo(_writer.WriteLine);
            
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
