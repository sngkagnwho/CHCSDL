using doan.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace doan.Controllers
{
    public class LoaiPhongsController : Controller
    {
        private readonly HotelDbContext _context;

        public LoaiPhongsController(HotelDbContext context)
        {
            _context = context;
        }

        // GET: LoaiPhongs
        public async Task<IActionResult> Index()
        {
            var list = await _context.LoaiPhongs
                .Include(lp => lp.Phongs)
                .OrderBy(lp => lp.MaLoaiPhong)
                .ToListAsync();
            return View(list);
        }
    }
}
