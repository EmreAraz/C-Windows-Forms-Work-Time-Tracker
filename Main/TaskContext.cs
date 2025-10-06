using Microsoft.EntityFrameworkCore;

namespace WorkTime
{
    public class TaskContext : DbContext
    {
        public DbSet<Task> Tasks { get; set; }  // Görevler tablosu
        public DbSet<ApplicationUser> Users { get; set; }  // Kullanıcılar tablosu
        public DbSet<Holiday> Holidays { get; set; }  // Tatil günleri tablosu
        public DbSet<WorkLog> WorkLogs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Veritabanı bağlantı dizesi
            optionsBuilder.UseSqlServer(@";");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Tasks tablosu için yapılandırma
            modelBuilder.Entity<Task>()
                .ToTable("Tasks")
                .HasOne(t => t.User)
                .WithMany(u => u.Tasks)
                .HasForeignKey(t => t.UserId);

            // Users tablosu için yapılandırma
            modelBuilder.Entity<ApplicationUser>()
                .ToTable("Users");

            // Holiday tablosu için yapılandırma
            modelBuilder.Entity<Holiday>()
                .ToTable("Holidays")
                .HasKey(h => h.Id); // Tatil tablosu için birincil anahtar
        }
    }
}
