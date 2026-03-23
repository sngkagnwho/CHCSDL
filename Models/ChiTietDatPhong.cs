using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace doan.Models
{
    [Table("CHITIET_DATPHONG")]
    public class ChiTietDatPhong
    {
        [Key]
        [Column("MACHITIET")]
        public int MaChiTiet { get; set; }

        [Column("MADATPHONG")]
        [Display(Name = "Đặt Phòng")]
        public int MaDatPhong { get; set; }

        [Column("MAPHONG")]
        [Display(Name = "Phòng")]
        public int MaPhong { get; set; }

        [Column("GIATHUE")]
        [Display(Name = "Giá Thuê")]
        [DataType(DataType.Currency)]
        public decimal GiaThue { get; set; }

        [ForeignKey("MaDatPhong")]
        public virtual DatPhong? DatPhong { get; set; }

        [ForeignKey("MaPhong")]
        public virtual Phong? Phong { get; set; }
    }
}
