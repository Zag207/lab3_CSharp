using Microsoft.EntityFrameworkCore;

namespace Lab3_CSharp.Models
{
    // TODO: Сделать инициализацию миграций при первом запуске,
    public class ApplicationContext : DbContext
    {
        public DbSet<Philosopher> Philosophers { get; set; } = null!;
        public DbSet<View> Views { get; set; } = null!;
        public DbSet<Work> Works { get; set; } = null!;
        public DbSet<Country> Countries { get; set; } = null!;

        public ApplicationContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=lab3;" +
                                     "Username=postgres;Password=admin1234");
        }
    }
    /*public class CountryContext : DbContext
    {
        public DbSet<Country> Countries { get; set; } = null!;

        public CountryContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=lab3;" +
                                     "Username=postgres;Password=admin1234");
        }
    }
    public class ViewContext : DbContext
    {
        public DbSet<View> Views { get; set; } = null!;

        public ViewContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=lab3;" +
                                     "Username=postgres;Password=admin1234");
        }
    }
    public class ViewPhilosophersContext : DbContext
    {
        public DbSet<View> Views { get; set; } = null!;
        public DbSet<Philosopher> Philosophers { get; set; } = null!;

        public ViewPhilosophersContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=lab3;" +
                                     "Username=postgres;Password=admin1234");
        }
    }
    public class WorkPhilosophersContext : DbContext
    {
        public DbSet<Work> Works { get; set; } = null!;
        public DbSet<Philosopher> Philosophers { get; set; } = null!;

        public WorkPhilosophersContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=lab3;" +
                                     "Username=postgres;Password=admin1234");
        }
    }
    public class WorkContext : DbContext
    {
        public DbSet<Work> Works { get; set; } = null!;

        public WorkContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=lab3;" +
                                     "Username=postgres;Password=admin1234");
        }
    }*/
}
