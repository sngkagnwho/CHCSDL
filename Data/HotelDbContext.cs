using Microsoft.EntityFrameworkCore;

namespace doan.Data
{
    public class HotelDbContext : DbContext
    {
        public HotelDbContext(DbContextOptions<HotelDbContext> options) : base(options) { }

        public DbSet<Models.LoaiPhong> LoaiPhongs { get; set; }
        public DbSet<Models.Phong> Phongs { get; set; }
        public DbSet<Models.KhachHang> KhachHangs { get; set; }
        public DbSet<Models.DatPhong> DatPhongs { get; set; }
        public DbSet<Models.ChiTietDatPhong> ChiTietDatPhongs { get; set; }
        public DbSet<Models.HoaDon> HoaDons { get; set; }
        public DbSet<Models.ThanhToan> ThanhToans { get; set; }
        public DbSet<Models.NhanVien> NhanViens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Models.DatPhong>()
                .HasOne(d => d.HoaDon)
                .WithOne(h => h.DatPhong)
                .HasForeignKey<Models.HoaDon>(h => h.MaDatPhong);

            modelBuilder.Entity<Models.LoaiPhong>()
                .HasMany(l => l.Phongs)
                .WithOne(p => p.LoaiPhong)
                .HasForeignKey(p => p.MaLoaiPhong);

            modelBuilder.Entity<Models.DatPhong>()
                .HasMany(d => d.ChiTietDatPhongs)
                .WithOne(c => c.DatPhong)
                .HasForeignKey(c => c.MaDatPhong);

            modelBuilder.Entity<Models.Phong>()
                .HasMany(p => p.ChiTietDatPhongs)
                .WithOne(c => c.Phong)
                .HasForeignKey(c => c.MaPhong);

            modelBuilder.Entity<Models.HoaDon>()
                .HasMany(h => h.ThanhToans)
                .WithOne(t => t.HoaDon)
                .HasForeignKey(t => t.MaHoaDon);
        }
    }
}
