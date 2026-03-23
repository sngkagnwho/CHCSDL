using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace doan.Models
{
    [Table("LOAIPHONG")]
    public class LoaiPhong
    {
        [Key]
        [Column("MALOAIPHONG")]
        [Display(Name = "Mã Loại Phòng")]
        public int MaLoaiPhong { get; set; }

        [Column("TENLOAIPHONG")]
        [Display(Name = "Tên Loại Phòng")]
        public string? TenLoaiPhong { get; set; }

        [Column("MOTA")]
        [Display(Name = "Mô Tả")]
        public string? MoTa { get; set; }

        [Column("GIACOBAN")]
        [Display(Name = "Giá Cơ Bản")]
        [DataType(DataType.Currency)]
        public decimal? GiaCoban { get; set; }

        [Column("SUCHUA")]
        [Display(Name = "Sức Chứa")]
        public int? SuChua { get; set; }

        public ICollection<Phong> Phongs { get; set; } = new List<Phong>();
    }
}
