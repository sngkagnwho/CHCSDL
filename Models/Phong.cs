using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace doan.Models
{
    [Table("PHONG")]
    public class Phong
    {
        [Key]
        [Column("MAPHONG")]
        [Display(Name = "Mã Phòng")]
        public int MaPhong { get; set; }

        [Column("SOPHONG")]
        [Display(Name = "Số Phòng")]
        public string? SoPhong { get; set; }

        [Column("MALOAIPHONG")]
        [Display(Name = "Loại Phòng")]
        public int? MaLoaiPhong { get; set; }

        [Column("TRANGTHAI")]
        [Display(Name = "Trạng Thái")]
        public string? TrangThai { get; set; }

        [Column("TANG")]
        [Display(Name = "Tầng")]
        public int? Tang { get; set; }

        [Column("GIA")]
        [Display(Name = "Giá")]
        [DataType(DataType.Currency)]
        public decimal? Gia { get; set; }

        [ForeignKey("MaLoaiPhong")]
        public LoaiPhong? LoaiPhong { get; set; }
    }
}
