using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace doan.Models
{
    [Table("HOADON")]
    public class HoaDon
    {
        [Key]
        [Column("MAHOADON")]
        public int MaHoaDon { get; set; }

        [Column("MADATPHONG")]
        [Display(Name = "Đặt Phòng")]
        public int MaDatPhong { get; set; }

        [Column("NGAYLAP")]
        [Display(Name = "Ngày Lập")]
        [DataType(DataType.Date)]
        public DateTime NgayLap { get; set; } = DateTime.Today;

        [Column("TONGTIEN")]
        [Display(Name = "Tổng Tiền")]
        [DataType(DataType.Currency)]
        public decimal TongTien { get; set; }

        [Column("GIAMGIA")]
        [Display(Name = "Giảm Giá (%)")]
        public decimal GiamGia { get; set; } = 0;

        [Column("THUEVAT")]
        [Display(Name = "Thuế VAT (%)")]
        public decimal ThueVat { get; set; } = 10;

        [Column("THANHTOAN")]
        [Display(Name = "Đã Thanh Toán")]
        [DataType(DataType.Currency)]
        public decimal ThanhToanTien { get; set; }

        [StringLength(50)]
        [Column("TRANGTHAI")]
        [Display(Name = "Trạng Thái")]
        public string TrangThai { get; set; } = "UNPAID";

        [StringLength(500)]
        [Column("GHICHU")]
        [Display(Name = "Ghi Chú")]
        public string? GhiChu { get; set; }

        [ForeignKey("MaDatPhong")]
        public virtual DatPhong? DatPhong { get; set; }

        public virtual ICollection<ThanhToan> ThanhToans { get; set; } = new List<ThanhToan>();
    }
}
