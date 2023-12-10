using Microsoft.EntityFrameworkCore;

namespace TestTask.Models
{
    /// <summary>
    /// Контекст базы данных.
    /// </summary>
    public class ApplicationContext : DbContext
    {
        /// <summary>
        /// Таблица погоды.
        /// </summary>
        public DbSet<Weather> Weathers { get; set; }

        /// <summary>
        /// Таблица файлов.
        /// </summary>
        public DbSet<FileModel> Files { get; set; }

        /// <summary>
        /// Конструктор с созданием базы данных, если она еще не создана.
        /// </summary>
        public ApplicationContext()
        {
            Database.EnsureCreated();
        }

        /// <summary>
        /// Конфигурация подключения к базе данных.
        /// </summary>
        /// <param name="optionsBuilder">Опции конфигурации.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=weatherdb;Trusted_Connection=True;MultipleActiveResultSets=True;TrustServerCertificate=True");
        }
    }
}
