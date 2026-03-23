using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace doan.Models
{
    [Table("KHACHHANG")]
    public class Khachhang
    {
        [Key]
        [Column("MAKHACHHANG")]
        [Display(Name = "Mã Khách Hàng")]
        public int MaKhachHang { get; set; }

        [Column("HOTEN")]
        [Required(ErrorMessage = "Họ tên không được để trống")]
        [StringLength(100, ErrorMessage = "Họ tên không quá 100 ký tự")]
        [Display(Name = "Họ Tên")]
        public string HoTen { get; set; } = string.Empty;

        [Column("SODIENTHOAI")]
        [StringLength(20, ErrorMessage = "Số điện thoại không quá 20 ký tự")]
        [Display(Name = "Số Điện Thoại")]
        public string? SoDienThoai { get; set; }

        [Column("EMAIL")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [StringLength(100, ErrorMessage = "Email không quá 100 ký tự")]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [Column("CCCD")]
        [StringLength(20, ErrorMessage = "CCCD không quá 20 ký tự")]
        [Display(Name = "CCCD")]
        public string? CCCD { get; set; }

        [Column("CREATEDAT")]
        [Display(Name = "Ngày Tạo")]
        public DateTime? CreatedAt { get; set; }

        [Column("UPDATEDAT")]
        [Display(Name = "Ngày Cập Nhật")]
        public DateTime? UpdatedAt { get; set; }
    }
}
