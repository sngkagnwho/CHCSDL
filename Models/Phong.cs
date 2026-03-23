using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace doan.Models
{
    [Table("PHONG")]
    public class Phong
    {
        [Key]
        [Column("MAPHONG")]
        public int MaPhong { get; set; }

        [Required]
        [StringLength(20)]
        [Column("SOPHONG")]
        [Display(Name = "Số Phòng")]
        public string SoPhong { get; set; } = string.Empty;

        [Column("MALOAIPHONG")]
        [Display(Name = "Loại Phòng")]
        public int MaLoaiPhong { get; set; }

        [Column("TANG")]
        [Display(Name = "Tầng")]
        public int Tang { get; set; }

        [StringLength(50)]
        [Column("TRANGTHAI")]
        [Display(Name = "Trạng Thái")]
        public string TrangThai { get; set; } = "AVAILABLE";

        [Column("GIANGAY")]
        [Display(Name = "Giá Theo Ngày")]
        [DataType(DataType.Currency)]
        public decimal? GiaNgay { get; set; }

        [StringLength(1000)]
        [Column("MOTA")]
        [Display(Name = "Mô Tả")]
        public string? MoTa { get; set; }

        [ForeignKey("MaLoaiPhong")]
        public virtual LoaiPhong? LoaiPhong { get; set; }

        public virtual ICollection<ChiTietDatPhong> ChiTietDatPhongs { get; set; } = new List<ChiTietDatPhong>();
    }
}
