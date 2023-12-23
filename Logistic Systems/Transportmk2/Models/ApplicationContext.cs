using Microsoft.EntityFrameworkCore;

namespace Transportmk2.Models
{
    public class ApplicationContext :DbContext
    {
        public DbSet<Storage> Storages { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();   // создаем базу данных при первом обращении
        }
    }
}
