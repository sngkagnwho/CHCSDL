using doan.Data;
using doan.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace doan.Controllers
{
    public class KhachHangController : Controller
    {
        private readonly HotelDbContext _context;

        public KhachHangController(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? search)
        {
            var query = _context.KhachHangs.AsQueryable();
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(k => k.TenKhachHang.Contains(search)
                    || k.Cmnd.Contains(search)
                    || (k.Sdt != null && k.Sdt.Contains(search)));
            }
            ViewBag.Search = search;
            return View(await query.OrderBy(k => k.TenKhachHang).ToListAsync());
        }

        public async Task<IActionResult> Details(int id)
        {
            var khachHang = await _context.KhachHangs
                .Include(k => k.DatPhongs)
                .FirstOrDefaultAsync(k => k.MaKhachHang == id);
            if (khachHang == null) return NotFound();
            return View(khachHang);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(KhachHang khachHang)
        {
            if (ModelState.IsValid)
            {
                _context.Add(khachHang);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Thêm khách hàng thành công!";
                return RedirectToAction(nameof(Index));
            }
            return View(khachHang);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var khachHang = await _context.KhachHangs.FindAsync(id);
            if (khachHang == null) return NotFound();
            return View(khachHang);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, KhachHang khachHang)
        {
            if (id != khachHang.MaKhachHang) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(khachHang);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Cập nhật khách hàng thành công!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.KhachHangs.Any(k => k.MaKhachHang == id)) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(khachHang);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var khachHang = await _context.KhachHangs.FindAsync(id);
            if (khachHang == null) return NotFound();
            return View(khachHang);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var khachHang = await _context.KhachHangs.FindAsync(id);
            if (khachHang != null)
            {
                _context.KhachHangs.Remove(khachHang);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Xóa khách hàng thành công!";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
