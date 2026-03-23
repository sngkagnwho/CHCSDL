using doan.Data;
using doan.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace doan.Controllers
{
    public class PhongController : Controller
    {
        private readonly HotelDbContext _context;

        public PhongController(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? trangThai)
        {
            var query = _context.Phongs.Include(p => p.LoaiPhong).AsQueryable();
            if (!string.IsNullOrWhiteSpace(trangThai))
            {
                query = query.Where(p => p.TrangThai == trangThai);
            }
            ViewBag.TrangThai = trangThai;
            ViewBag.TrangThaiList = new SelectList(new[] { "AVAILABLE", "OCCUPIED", "MAINTENANCE" });
            return View(await query.OrderBy(p => p.SoPhong).ToListAsync());
        }

        public async Task<IActionResult> Details(int id)
        {
            var phong = await _context.Phongs
                .Include(p => p.LoaiPhong)
                .FirstOrDefaultAsync(p => p.MaPhong == id);
            if (phong == null) return NotFound();
            return View(phong);
        }

        public IActionResult Create()
        {
            ViewBag.LoaiPhongList = new SelectList(_context.LoaiPhongs, "MaLoaiPhong", "TenLoaiPhong");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Phong phong)
        {
            if (ModelState.IsValid)
            {
                _context.Add(phong);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Thêm phòng thành công!";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.LoaiPhongList = new SelectList(_context.LoaiPhongs, "MaLoaiPhong", "TenLoaiPhong", phong.MaLoaiPhong);
            return View(phong);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var phong = await _context.Phongs.FindAsync(id);
            if (phong == null) return NotFound();
            ViewBag.LoaiPhongList = new SelectList(_context.LoaiPhongs, "MaLoaiPhong", "TenLoaiPhong", phong.MaLoaiPhong);
            return View(phong);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Phong phong)
        {
            if (id != phong.MaPhong) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(phong);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Cập nhật phòng thành công!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Phongs.Any(p => p.MaPhong == id)) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.LoaiPhongList = new SelectList(_context.LoaiPhongs, "MaLoaiPhong", "TenLoaiPhong", phong.MaLoaiPhong);
            return View(phong);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var phong = await _context.Phongs.Include(p => p.LoaiPhong).FirstOrDefaultAsync(p => p.MaPhong == id);
            if (phong == null) return NotFound();
            return View(phong);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var phong = await _context.Phongs.FindAsync(id);
            if (phong != null)
            {
                _context.Phongs.Remove(phong);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Xóa phòng thành công!";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
