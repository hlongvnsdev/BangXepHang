using Microsoft.EntityFrameworkCore;
using BangXepHang.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BangXepHang.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<GameScore> GameScores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Duyệt qua tất cả các entity types (kiểu thực thể) trong mô hình
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                // Duyệt qua tất cả các thuộc tính của mỗi entity
                foreach (var property in entityType.GetProperties())
                {
                    // Kiểm tra xem thuộc tính có kiểu DateTime hoặc DateTime? (nullable)
                    if (property.ClrType == typeof(DateTime) || property.ClrType == typeof(DateTime?))
                    {
                        // Đặt ValueConverter để chuyển đổi DateTime thành UTC khi lưu và đọc từ cơ sở dữ liệu
                        property.SetValueConverter(new ValueConverter<DateTime, DateTime>(
                            v => v.ToUniversalTime(), // Chuyển đổi DateTime sang UTC khi lưu
                            v => DateTime.SpecifyKind(v, DateTimeKind.Utc) // Đảm bảo DateTime là UTC khi đọc
                        ));
                    }
                }
            }

            // Configure Customer entity
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Avatar).HasMaxLength(500);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            // Configure GameScore entity
            modelBuilder.Entity<GameScore>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.PlayTime).IsRequired();
                entity.Property(e => e.Score).IsRequired();
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

                // Configure foreign key relationship
                entity.HasOne(e => e.Customer)
                      .WithMany(c => c.GameScores)
                      .HasForeignKey(e => e.CustomerId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Add indexes for better performance
            modelBuilder.Entity<GameScore>()
                .HasIndex(e => e.PlayTime)
                .HasDatabaseName("IX_GameScores_PlayTime");

            modelBuilder.Entity<GameScore>()
                .HasIndex(e => new { e.CustomerId, e.PlayTime })
                .HasDatabaseName("IX_GameScores_CustomerId_PlayTime");
        }
    }
}
