using doan.Services;
using Microsoft.AspNetCore.Mvc;

namespace doan.Controllers
{
    public class PackagesController : Controller
    {
        private readonly DatabaseMetadataService _metadataService;

        public PackagesController(DatabaseMetadataService metadataService)
        {
            _metadataService = metadataService;
        }

        public IActionResult Index()
        {
            var packages = _metadataService.GetPackages();
            return View(packages);
        }
    }
}
