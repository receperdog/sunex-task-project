using Microsoft.EntityFrameworkCore;
using sunex_task_project.Models;

namespace sunex_task_project.AppDataContext
{

    public class AppDbContext : DbContext
    {
        public DbSet<TaskItem> Tasks { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
    }
}