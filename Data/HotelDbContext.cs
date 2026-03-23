using Microsoft.EntityFrameworkCore;
using doan.Models;

namespace doan.Data
{
    public class HotelDbContext : DbContext
    {
        public HotelDbContext(DbContextOptions<HotelDbContext> options) : base(options)
        {
        }

        public DbSet<Khachhang> Khachhangs { get; set; }
        public DbSet<Phong> Phongs { get; set; }
        public DbSet<LoaiPhong> LoaiPhongs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Khachhang>().ToTable("KHACHHANG");
            modelBuilder.Entity<Phong>().ToTable("PHONG");
            modelBuilder.Entity<LoaiPhong>().ToTable("LOAIPHONG");

            modelBuilder.Entity<Phong>()
                .HasOne(p => p.LoaiPhong)
                .WithMany(lp => lp.Phongs)
                .HasForeignKey(p => p.MaLoaiPhong)
                .IsRequired(false);
        }
    }
}
