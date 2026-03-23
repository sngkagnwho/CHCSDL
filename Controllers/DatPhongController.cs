using doan.Data;
using doan.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace doan.Controllers
{
    public class DatPhongController : Controller
    {
        private readonly HotelDbContext _context;

        public DatPhongController(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? trangThai)
        {
            var query = _context.DatPhongs
                .Include(d => d.KhachHang)
                .AsQueryable();
            if (!string.IsNullOrWhiteSpace(trangThai))
            {
                query = query.Where(d => d.TrangThai == trangThai);
            }
            ViewBag.TrangThai = trangThai;
            ViewBag.TrangThaiList = new SelectList(new[] { "PENDING", "CONFIRMED", "CHECKED_IN", "CHECKED_OUT", "CANCELLED" });
            return View(await query.OrderByDescending(d => d.NgayDatPhong).ToListAsync());
        }

        public async Task<IActionResult> Details(int id)
        {
            var datPhong = await _context.DatPhongs
                .Include(d => d.KhachHang)
                .Include(d => d.ChiTietDatPhongs)
                    .ThenInclude(c => c.Phong)
                        .ThenInclude(p => p!.LoaiPhong)
                .FirstOrDefaultAsync(d => d.MaDatPhong == id);
            if (datPhong == null) return NotFound();
            return View(datPhong);
        }

        public IActionResult Create()
        {
            ViewBag.KhachHangList = new SelectList(_context.KhachHangs.OrderBy(k => k.TenKhachHang), "MaKhachHang", "TenKhachHang");
            ViewBag.PhongList = new SelectList(_context.Phongs.Where(p => p.TrangThai == "AVAILABLE").Include(p => p.LoaiPhong)
                .OrderBy(p => p.SoPhong), "MaPhong", "SoPhong");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DatPhong datPhong, int[] selectedPhongs)
        {
            if (ModelState.IsValid)
            {
                _context.Add(datPhong);
                await _context.SaveChangesAsync();

                foreach (var maPhong in selectedPhongs)
                {
                    var phong = await _context.Phongs.Include(p => p.LoaiPhong).FirstOrDefaultAsync(p => p.MaPhong == maPhong);
                    if (phong != null)
                    {
                        decimal giaThue = phong.GiaNgay ?? phong.LoaiPhong?.GiaCoban ?? 0;
                        _context.ChiTietDatPhongs.Add(new ChiTietDatPhong
                        {
                            MaDatPhong = datPhong.MaDatPhong,
                            MaPhong = maPhong,
                            GiaThue = giaThue
                        });
                    }
                }
                await _context.SaveChangesAsync();
                TempData["Success"] = "Đặt phòng thành công!";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.KhachHangList = new SelectList(_context.KhachHangs.OrderBy(k => k.TenKhachHang), "MaKhachHang", "TenKhachHang", datPhong.MaKhachHang);
            ViewBag.PhongList = new SelectList(_context.Phongs.Where(p => p.TrangThai == "AVAILABLE").OrderBy(p => p.SoPhong), "MaPhong", "SoPhong");
            return View(datPhong);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var datPhong = await _context.DatPhongs.FindAsync(id);
            if (datPhong == null) return NotFound();
            ViewBag.KhachHangList = new SelectList(_context.KhachHangs.OrderBy(k => k.TenKhachHang), "MaKhachHang", "TenKhachHang", datPhong.MaKhachHang);
            return View(datPhong);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DatPhong datPhong)
        {
            if (id != datPhong.MaDatPhong) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(datPhong);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Cập nhật đặt phòng thành công!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.DatPhongs.Any(d => d.MaDatPhong == id)) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.KhachHangList = new SelectList(_context.KhachHangs.OrderBy(k => k.TenKhachHang), "MaKhachHang", "TenKhachHang", datPhong.MaKhachHang);
            return View(datPhong);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var datPhong = await _context.DatPhongs.Include(d => d.KhachHang).FirstOrDefaultAsync(d => d.MaDatPhong == id);
            if (datPhong == null) return NotFound();
            return View(datPhong);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var datPhong = await _context.DatPhongs.FindAsync(id);
            if (datPhong != null)
            {
                _context.DatPhongs.Remove(datPhong);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Xóa đặt phòng thành công!";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
