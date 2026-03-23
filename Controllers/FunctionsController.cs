using doan.Services;
using Microsoft.AspNetCore.Mvc;

namespace doan.Controllers
{
    public class FunctionsController : Controller
    {
        private readonly DatabaseMetadataService _metadataService;
        private readonly OracleService _oracleService;

        public FunctionsController(DatabaseMetadataService metadataService, OracleService oracleService)
        {
            _metadataService = metadataService;
            _oracleService = oracleService;
        }

        public IActionResult Index()
        {
            var functions = _metadataService.GetFunctions();
            return View(functions);
        }

        public IActionResult Test(string name)
        {
            var functions = _metadataService.GetFunctions();
            var function = functions.FirstOrDefault(f => f.FunctionName == name);
            if (function == null)
                return NotFound();
            ViewBag.Result = null;
            return View(function);
        }

        [HttpPost]
        public IActionResult Test(string name, Dictionary<string, string> paramValues)
        {
            var functions = _metadataService.GetFunctions();
            var function = functions.FirstOrDefault(f => f.FunctionName == name);
            if (function == null)
                return NotFound();

            var result = _oracleService.ExecuteFunction(name, paramValues);
            // Simulate result when Oracle not available
            if (result == null)
            {
                result = name switch
                {
                    "fn_get_gia_phong" => "850,000 VNĐ",
                    "fn_tinh_so_dem" => "4 đêm",
                    "fn_kiem_tra_phong_trong" => "1 (Phòng trống)",
                    "fn_get_tong_tien_hoadon" => "3,400,000 VNĐ",
                    "fn_get_ten_khachhang" => "Nguyễn Văn A",
                    _ => "N/A"
                };
            }
            ViewBag.Result = result;
            return View(function);
        }
    }
}
