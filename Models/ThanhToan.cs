using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace doan.Models
{
    [Table("THANHTOAN")]
    public class ThanhToan
    {
        [Key]
        [Column("MATHANHTOAN")]
        public int MaThanhToan { get; set; }

        [Column("MAHOADON")]
        [Display(Name = "Hóa Đơn")]
        public int MaHoaDon { get; set; }

        [Column("NGAYTHANHTOAN")]
        [Display(Name = "Ngày Thanh Toán")]
        [DataType(DataType.Date)]
        public DateTime NgayThanhToan { get; set; } = DateTime.Today;

        [Column("SOTIEN")]
        [Display(Name = "Số Tiền")]
        [DataType(DataType.Currency)]
        public decimal SoTien { get; set; }

        [StringLength(50)]
        [Column("PHUONGTHUC")]
        [Display(Name = "Phương Thức")]
        public string PhuongThuc { get; set; } = "CASH";

        [StringLength(200)]
        [Column("GHICHU")]
        [Display(Name = "Ghi Chú")]
        public string? GhiChu { get; set; }

        [ForeignKey("MaHoaDon")]
        public virtual HoaDon? HoaDon { get; set; }
    }
}
