using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace doan.Models
{
    [Table("DATPHONG")]
    public class DatPhong
    {
        [Key]
        [Column("MADATPHONG")]
        public int MaDatPhong { get; set; }

        [Column("MAKHACHHANG")]
        [Display(Name = "Khách Hàng")]
        public int MaKhachHang { get; set; }

        [Column("NGAYDATPHONG")]
        [Display(Name = "Ngày Đặt")]
        [DataType(DataType.Date)]
        public DateTime NgayDatPhong { get; set; } = DateTime.Today;

        [Column("NGAYCHECKIN")]
        [Display(Name = "Ngày Check-in")]
        [DataType(DataType.Date)]
        public DateTime NgayCheckIn { get; set; }

        [Column("NGAYCHECKOUT")]
        [Display(Name = "Ngày Check-out")]
        [DataType(DataType.Date)]
        public DateTime NgayCheckOut { get; set; }

        [StringLength(50)]
        [Column("TRANGTHAI")]
        [Display(Name = "Trạng Thái")]
        public string TrangThai { get; set; } = "PENDING";

        [StringLength(500)]
        [Column("GHICHU")]
        [Display(Name = "Ghi Chú")]
        public string? GhiChu { get; set; }

        [Column("TONGTIEN")]
        [Display(Name = "Tổng Tiền")]
        [DataType(DataType.Currency)]
        public decimal? TongTien { get; set; }

        [ForeignKey("MaKhachHang")]
        public virtual KhachHang? KhachHang { get; set; }

        public virtual ICollection<ChiTietDatPhong> ChiTietDatPhongs { get; set; } = new List<ChiTietDatPhong>();
        public virtual HoaDon? HoaDon { get; set; }
    }
}
