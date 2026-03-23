using doan.Models;
using doan.Services;
using Microsoft.AspNetCore.Mvc;

namespace doan.Controllers
{
    public class ProceduresController : Controller
    {
        private readonly OracleService _oracleService;
        private readonly DatabaseMetadataService _metadataService;

        public ProceduresController(OracleService oracleService, DatabaseMetadataService metadataService)
        {
            _oracleService = oracleService;
            _metadataService = metadataService;
        }

        public IActionResult Index()
        {
            var procedures = _metadataService.GetProcedures();
            return View(procedures);
        }

        public IActionResult Execute(string name)
        {
            var procedures = _metadataService.GetProcedures();
            var procedure = procedures.FirstOrDefault(p => p.ProcedureName == name);
            if (procedure == null)
                return NotFound();
            ViewBag.Result = null;
            return View(procedure);
        }

        [HttpPost]
        public IActionResult Execute(string name, string? p_ngay, int? p_thang, int? p_nam)
        {
            var procedures = _metadataService.GetProcedures();
            var procedure = procedures.FirstOrDefault(p => p.ProcedureName == name);
            if (procedure == null)
                return NotFound();

            ExecutionResult? result = null;

            switch (name)
            {
                case "sp_tinh_doanh_thu_ngay":
                    var ngay = DateTime.TryParse(p_ngay, out var d) ? d : DateTime.Today;
                    result = _oracleService.ExecuteSpTinhDoanhThuNgay(ngay);
                    break;
                case "sp_tinh_doanh_thu_thang":
                    result = _oracleService.ExecuteSpTinhDoanhThuThang(p_thang ?? DateTime.Today.Month, p_nam ?? DateTime.Today.Year);
                    break;
                case "sp_thong_ke_phong_trong":
                    result = _oracleService.ExecuteSpThongKePhongTrong();
                    break;
                case "sp_tinh_luong_nhanvien":
                    result = _oracleService.ExecuteSpTinhLuongNhanVien(p_thang ?? DateTime.Today.Month, p_nam ?? DateTime.Today.Year);
                    break;
                case "sp_cap_nhat_baocao":
                    result = _oracleService.ExecuteSpCapNhatBaoCao(p_thang ?? DateTime.Today.Month, p_nam ?? DateTime.Today.Year);
                    break;
                default:
                    result = new ExecutionResult { Success = false, Message = "Procedure không hỗ trợ thực thi trực tiếp từ web." };
                    break;
            }

            ViewBag.Result = result;
            return View(procedure);
        }
    }
}
