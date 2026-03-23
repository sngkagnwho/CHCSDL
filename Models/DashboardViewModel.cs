namespace doan.Models
{
    public class DashboardViewModel
    {
        public decimal DoanhThuNgay { get; set; }
        public int SoHoaDonNgay { get; set; }
        public int SoKhachNgay { get; set; }
        public decimal DoanhThuThang { get; set; }
        public int SoHoaDonThang { get; set; }
        public int SoPhongTrong { get; set; }
        public int SoPhongDangDung { get; set; }
        public int TongSoPhong { get; set; }
        public int TongKhachHang { get; set; }
        public List<DailyRevenue> DoanhThuNgayTrongThang { get; set; } = new();
        public List<RoomStatus> ThongKePhong { get; set; } = new();
        public int TongProcedures { get; set; }
        public int TongTriggers { get; set; }
        public int TongFunctions { get; set; }
        public int TongPackages { get; set; }
    }

    public class DailyRevenue
    {
        public string Ngay { get; set; } = string.Empty;
        public decimal DoanhThu { get; set; }
        public int SoHoaDon { get; set; }
    }

    public class RoomStatus
    {
        public string LoaiPhong { get; set; } = string.Empty;
        public int SoPhongTrong { get; set; }
        public int SoPhongDangDung { get; set; }
    }
}
