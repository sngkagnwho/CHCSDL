using doan.Services;
using Microsoft.AspNetCore.Mvc;

namespace doan.Controllers
{
    public class DatabaseInfoController : Controller
    {
        private readonly DatabaseMetadataService _metadataService;

        public DatabaseInfoController(DatabaseMetadataService metadataService)
        {
            _metadataService = metadataService;
        }

        public IActionResult Index()
        {
            var stats = _metadataService.GetDatabaseStats();
            return View(stats);
        }
    }
}
