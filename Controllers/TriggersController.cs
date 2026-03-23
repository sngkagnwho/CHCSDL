using doan.Services;
using Microsoft.AspNetCore.Mvc;

namespace doan.Controllers
{
    public class TriggersController : Controller
    {
        private readonly DatabaseMetadataService _metadataService;

        public TriggersController(DatabaseMetadataService metadataService)
        {
            _metadataService = metadataService;
        }

        public IActionResult Index()
        {
            var triggers = _metadataService.GetTriggers();
            return View(triggers);
        }
    }
}
