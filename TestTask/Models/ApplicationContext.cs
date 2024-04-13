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
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
