using doan.Services;
using Microsoft.AspNetCore.Mvc;

namespace doan.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ReportService _reportService;

        public DashboardController(ReportService reportService)
        {
            _reportService = reportService;
        }

        public IActionResult Index()
        {
            var model = _reportService.GetDashboardData();
            return View(model);
        }
    }
}
