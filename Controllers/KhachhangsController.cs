using doan.Data;
using doan.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace doan.Controllers
{
    public class KhachhangsController : Controller
    {
        private readonly HotelDbContext _context;

        public KhachhangsController(HotelDbContext context)
        {
            _context = context;
        }

        // GET: Khachhangs
        public async Task<IActionResult> Index(string? search)
        {
            var query = _context.Khachhangs.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(k =>
                    k.HoTen.Contains(search) ||
                    (k.SoDienThoai != null && k.SoDienThoai.Contains(search)) ||
                    (k.Email != null && k.Email.Contains(search)) ||
                    (k.CCCD != null && k.CCCD.Contains(search)));
            }

            ViewBag.Search = search;
            var list = await query.OrderBy(k => k.MaKhachHang).ToListAsync();
            return View(list);
        }

        // GET: Khachhangs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Khachhangs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HoTen,SoDienThoai,Email,CCCD")] Khachhang khachhang)
        {
            if (ModelState.IsValid)
            {
                khachhang.CreatedAt = DateTime.Now;
                khachhang.UpdatedAt = DateTime.Now;
                _context.Add(khachhang);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Thêm khách hàng thành công!";
                return RedirectToAction(nameof(Index));
            }
            return View(khachhang);
        }

        // GET: Khachhangs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var khachhang = await _context.Khachhangs.FindAsync(id);
            if (khachhang == null)
                return NotFound();

            return View(khachhang);
        }

        // POST: Khachhangs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaKhachHang,HoTen,SoDienThoai,Email,CCCD,CreatedAt")] Khachhang khachhang)
        {
            if (id != khachhang.MaKhachHang)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    khachhang.UpdatedAt = DateTime.Now;
                    _context.Update(khachhang);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Cập nhật khách hàng thành công!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Khachhangs.Any(k => k.MaKhachHang == id))
                        return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(khachhang);
        }

        // GET: Khachhangs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var khachhang = await _context.Khachhangs.FirstOrDefaultAsync(k => k.MaKhachHang == id);
            if (khachhang == null)
                return NotFound();

            return View(khachhang);
        }

        // POST: Khachhangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var khachhang = await _context.Khachhangs.FindAsync(id);
            if (khachhang != null)
            {
                _context.Khachhangs.Remove(khachhang);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Xóa khách hàng thành công!";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
