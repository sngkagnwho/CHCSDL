using doan.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace doan.Controllers
{
    public class PhongsController : Controller
    {
        private readonly HotelDbContext _context;

        public PhongsController(HotelDbContext context)
        {
            _context = context;
        }

        // GET: Phongs
        public async Task<IActionResult> Index(string? status)
        {
            var query = _context.Phongs.Include(p => p.LoaiPhong).AsQueryable();

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(p => p.TrangThai == status);
            }

            ViewBag.Status = status;
            var list = await query.OrderBy(p => p.SoPhong).ToListAsync();
            return View(list);
        }
    }
}
