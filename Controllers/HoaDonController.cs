using doan.Data;
using doan.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace doan.Controllers
{
    public class HoaDonController : Controller
    {
        private readonly HotelDbContext _context;

        public HoaDonController(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? trangThai)
        {
            var query = _context.HoaDons
                .Include(h => h.DatPhong)
                    .ThenInclude(d => d!.KhachHang)
                .AsQueryable();
            if (!string.IsNullOrWhiteSpace(trangThai))
            {
                query = query.Where(h => h.TrangThai == trangThai);
            }
            ViewBag.TrangThai = trangThai;
            ViewBag.TrangThaiList = new SelectList(new[] { "UNPAID", "PAID", "PARTIAL" });
            return View(await query.OrderByDescending(h => h.NgayLap).ToListAsync());
        }

        public async Task<IActionResult> Details(int id)
        {
            var hoaDon = await _context.HoaDons
                .Include(h => h.DatPhong)
                    .ThenInclude(d => d!.KhachHang)
                .Include(h => h.ThanhToans)
                .FirstOrDefaultAsync(h => h.MaHoaDon == id);
            if (hoaDon == null) return NotFound();
            return View(hoaDon);
        }

        public IActionResult Create()
        {
            ViewBag.DatPhongList = new SelectList(
                _context.DatPhongs.Include(d => d.KhachHang)
                    .Where(d => d.TrangThai == "CONFIRMED" || d.TrangThai == "CHECKED_IN")
                    .OrderByDescending(d => d.NgayDatPhong)
                    .Select(d => new { d.MaDatPhong, Ten = d.MaDatPhong + " - " + d.KhachHang!.TenKhachHang }),
                "MaDatPhong", "Ten");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HoaDon hoaDon)
        {
            if (ModelState.IsValid)
            {
                hoaDon.ThanhToanTien = 0;
                _context.Add(hoaDon);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Tạo hóa đơn thành công!";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.DatPhongList = new SelectList(
                _context.DatPhongs.Include(d => d.KhachHang)
                    .Where(d => d.TrangThai == "CONFIRMED" || d.TrangThai == "CHECKED_IN")
                    .Select(d => new { d.MaDatPhong, Ten = d.MaDatPhong + " - " + d.KhachHang!.TenKhachHang }),
                "MaDatPhong", "Ten", hoaDon.MaDatPhong);
            return View(hoaDon);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var hoaDon = await _context.HoaDons.FindAsync(id);
            if (hoaDon == null) return NotFound();
            ViewBag.DatPhongList = new SelectList(
                _context.DatPhongs.Include(d => d.KhachHang)
                    .Select(d => new { d.MaDatPhong, Ten = d.MaDatPhong + " - " + d.KhachHang!.TenKhachHang }),
                "MaDatPhong", "Ten", hoaDon.MaDatPhong);
            return View(hoaDon);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, HoaDon hoaDon)
        {
            if (id != hoaDon.MaHoaDon) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hoaDon);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Cập nhật hóa đơn thành công!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.HoaDons.Any(h => h.MaHoaDon == id)) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.DatPhongList = new SelectList(
                _context.DatPhongs.Include(d => d.KhachHang)
                    .Select(d => new { d.MaDatPhong, Ten = d.MaDatPhong + " - " + d.KhachHang!.TenKhachHang }),
                "MaDatPhong", "Ten", hoaDon.MaDatPhong);
            return View(hoaDon);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var hoaDon = await _context.HoaDons
                .Include(h => h.DatPhong)
                    .ThenInclude(d => d!.KhachHang)
                .FirstOrDefaultAsync(h => h.MaHoaDon == id);
            if (hoaDon == null) return NotFound();
            return View(hoaDon);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hoaDon = await _context.HoaDons.FindAsync(id);
            if (hoaDon != null)
            {
                _context.HoaDons.Remove(hoaDon);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Xóa hóa đơn thành công!";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
