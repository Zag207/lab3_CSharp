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
}
