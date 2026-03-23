using doan.Models;

namespace doan.Services
{
    public class ReportService
    {
        private readonly OracleService _oracleService;

        public ReportService(OracleService oracleService)
        {
            _oracleService = oracleService;
        }

        public DashboardViewModel GetDashboardData()
        {
            var today = DateTime.Today;
            var ngayResult = _oracleService.ExecuteSpTinhDoanhThuNgay(today);
            var phongResult = _oracleService.ExecuteSpThongKePhongTrong();
            var thangResult = _oracleService.ExecuteSpTinhDoanhThuThang(today.Month, today.Year);

            decimal.TryParse(ngayResult.OutputValues.GetValueOrDefault("Doanh Thu")?.ToString()?.Replace(",",""), out decimal doanhThuNgay);
            int.TryParse(ngayResult.OutputValues.GetValueOrDefault("Số Hóa Đơn")?.ToString(), out int soHoaDonNgay);
            int.TryParse(ngayResult.OutputValues.GetValueOrDefault("Số Khách")?.ToString(), out int soKhachNgay);
            int.TryParse(phongResult.OutputValues.GetValueOrDefault("Số Phòng Trống")?.ToString(), out int soPhongTrong);
            int.TryParse(phongResult.OutputValues.GetValueOrDefault("Số Phòng Đang Dùng")?.ToString(), out int soPhongDangDung);
            decimal.TryParse(thangResult.OutputValues.GetValueOrDefault("Doanh Thu Tháng")?.ToString()?.Replace(",",""), out decimal doanhThuThang);
            int.TryParse(thangResult.OutputValues.GetValueOrDefault("Số Hóa Đơn")?.ToString(), out int soHoaDonThang);

            // Build daily revenue for chart (last 7 days)
            var dailyRevenues = new List<DailyRevenue>();
            for (int i = 6; i >= 0; i--)
            {
                var d = today.AddDays(-i);
                var r = _oracleService.ExecuteSpTinhDoanhThuNgay(d);
                decimal.TryParse(r.OutputValues.GetValueOrDefault("Doanh Thu")?.ToString()?.Replace(",",""), out decimal dt);
                int.TryParse(r.OutputValues.GetValueOrDefault("Số Hóa Đơn")?.ToString(), out int hd);
                dailyRevenues.Add(new DailyRevenue
                {
                    Ngay = d.ToString("dd/MM"),
                    DoanhThu = dt,
                    SoHoaDon = hd
                });
            }

            var roomStatuses = new List<RoomStatus>
            {
                new RoomStatus { LoaiPhong = "Phòng Đơn", SoPhongTrong = 4, SoPhongDangDung = 8 },
                new RoomStatus { LoaiPhong = "Phòng Đôi", SoPhongTrong = 5, SoPhongDangDung = 10 },
                new RoomStatus { LoaiPhong = "Phòng Suite", SoPhongTrong = 2, SoPhongDangDung = 6 },
                new RoomStatus { LoaiPhong = "Phòng VIP", SoPhongTrong = 1, SoPhongDangDung = 4 }
            };

            return new DashboardViewModel
            {
                DoanhThuNgay = doanhThuNgay,
                SoHoaDonNgay = soHoaDonNgay,
                SoKhachNgay = soKhachNgay,
                SoPhongTrong = soPhongTrong,
                SoPhongDangDung = soPhongDangDung,
                TongSoPhong = soPhongTrong + soPhongDangDung,
                DoanhThuThang = doanhThuThang,
                SoHoaDonThang = soHoaDonThang,
                TongKhachHang = 245,
                DoanhThuNgayTrongThang = dailyRevenues,
                ThongKePhong = roomStatuses,
                TongProcedures = 5,
                TongTriggers = 7,
                TongFunctions = 5,
                TongPackages = 2
            };
        }
    }
}
