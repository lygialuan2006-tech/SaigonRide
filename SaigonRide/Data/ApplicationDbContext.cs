using Microsoft.EntityFrameworkCore;
using SaigonRide.Models.entities;

namespace SaigonRide.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Station> Stations { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<RentalTransaction> RentalTransactions { get; set; }

        // THÊM ĐOẠN NÀY VÀO ĐỂ SỬA LỖI CASCADE PATH:
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Ngăn chặn xóa tự động (Cascade Delete) từ Vehicle sang Transaction
            modelBuilder.Entity<RentalTransaction>()
                .HasOne(rt => rt.Vehicle)
                .WithMany()
                .HasForeignKey(rt => rt.VehicleId)
                .OnDelete(DeleteBehavior.Restrict);

            // Ngăn chặn xóa tự động (Cascade Delete) từ Station sang Transaction
            modelBuilder.Entity<RentalTransaction>()
                .HasOne(rt => rt.ReturnStation)
                .WithMany()
                .HasForeignKey(rt => rt.ReturnStationId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}