using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace doan.Models
{
    [Table("NHANVIEN")]
    public class NhanVien
    {
        [Key]
        [Column("MANHANVIEN")]
        public int MaNhanVien { get; set; }

        [Required]
        [StringLength(100)]
        [Column("TENNHANVIEN")]
        [Display(Name = "Họ Tên")]
        public string TenNhanVien { get; set; } = string.Empty;

        [StringLength(50)]
        [Column("CHUCVU")]
        [Display(Name = "Chức Vụ")]
        public string? ChucVu { get; set; }

        [StringLength(15)]
        [Column("SDT")]
        [Display(Name = "Số Điện Thoại")]
        [DataType(DataType.PhoneNumber)]
        public string? Sdt { get; set; }

        [StringLength(200)]
        [Column("EMAIL")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [Column("LUONG")]
        [Display(Name = "Lương Cơ Bản")]
        [DataType(DataType.Currency)]
        public decimal Luong { get; set; }

        [Column("NGAYVAOLAM")]
        [Display(Name = "Ngày Vào Làm")]
        [DataType(DataType.Date)]
        public DateTime? NgayVaoLam { get; set; }

        [StringLength(20)]
        [Column("TRANGTHAI")]
        [Display(Name = "Trạng Thái")]
        public string TrangThai { get; set; } = "ACTIVE";
    }
}
