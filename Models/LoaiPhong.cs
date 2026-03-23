using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace doan.Models
{
    [Table("LOAIPHONG")]
    public class LoaiPhong
    {
        [Key]
        [Column("MALOAIPHONG")]
        public int MaLoaiPhong { get; set; }

        [Required]
        [StringLength(100)]
        [Column("TENLOAIPHONG")]
        [Display(Name = "Tên Loại Phòng")]
        public string TenLoaiPhong { get; set; } = string.Empty;

        [Column("GIACOBAN")]
        [Display(Name = "Giá Cơ Bản")]
        [DataType(DataType.Currency)]
        public decimal GiaCoban { get; set; }

        [StringLength(500)]
        [Column("MOTA")]
        [Display(Name = "Mô Tả")]
        public string? MoTa { get; set; }

        [Column("SOGIUONG")]
        [Display(Name = "Số Giường")]
        public int SoGiuong { get; set; } = 1;

        public virtual ICollection<Phong> Phongs { get; set; } = new List<Phong>();
    }
}
