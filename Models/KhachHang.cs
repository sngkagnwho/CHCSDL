using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace doan.Models
{
    [Table("KHACHHANG")]
    public class KhachHang
    {
        [Key]
        [Column("MAKHACHHANG")]
        public int MaKhachHang { get; set; }

        [Required]
        [StringLength(100)]
        [Column("TENKHACHHANG")]
        [Display(Name = "Họ Tên")]
        public string TenKhachHang { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        [Column("CMND")]
        [Display(Name = "CMND/CCCD")]
        public string Cmnd { get; set; } = string.Empty;

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

        [StringLength(300)]
        [Column("DIACHI")]
        [Display(Name = "Địa Chỉ")]
        public string? DiaChi { get; set; }

        [StringLength(50)]
        [Column("QUOCTICH")]
        [Display(Name = "Quốc Tịch")]
        public string? QuocTich { get; set; }

        [Column("NGAYSINH")]
        [Display(Name = "Ngày Sinh")]
        [DataType(DataType.Date)]
        public DateTime? NgaySinh { get; set; }

        public virtual ICollection<DatPhong> DatPhongs { get; set; } = new List<DatPhong>();
    }
}
