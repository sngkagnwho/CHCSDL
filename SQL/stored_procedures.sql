/* ================================================================
   STORED PROCEDURES - HỆ THỐNG QUẢN LÝ KHÁCH SẠN
   Run as MANA: sqlplus mana/mana123
   ================================================================ */

-- ================================================================
-- PROCEDURE 1: TÍNH DOANH THU NGÀY
-- ================================================================
CREATE OR REPLACE PROCEDURE sp_tinh_doanh_thu_ngay (
    p_ngay              IN  DATE,
    p_tong_doanh_thu    OUT NUMBER,
    p_so_hoadon         OUT NUMBER,
    p_so_khachhang      OUT NUMBER
)
AS
BEGIN
    SELECT
        NVL(SUM(hd.TongTien), 0),
        COUNT(DISTINCT hd.MaHoaDon),
        COUNT(DISTINCT dp.MaKhachHang)
    INTO
        p_tong_doanh_thu,
        p_so_hoadon,
        p_so_khachhang
    FROM
        HOADON hd
        JOIN DATPHONG dp ON hd.MaDatPhong = dp.MaDatPhong
    WHERE
        TRUNC(hd.NgayLap) = TRUNC(p_ngay);

    DBMS_OUTPUT.PUT_LINE('Doanh thu ngày ' || TO_CHAR(p_ngay, 'DD/MM/YYYY') || ': ' || p_tong_doanh_thu);
    DBMS_OUTPUT.PUT_LINE('Số hóa đơn: ' || p_so_hoadon);
    DBMS_OUTPUT.PUT_LINE('Số khách hàng: ' || p_so_khachhang);

EXCEPTION WHEN OTHERS THEN
    DBMS_OUTPUT.PUT_LINE('Lỗi: ' || SQLERRM);
    p_tong_doanh_thu := 0;
    p_so_hoadon := 0;
    p_so_khachhang := 0;
END sp_tinh_doanh_thu_ngay;
/

-- ================================================================
-- PROCEDURE 2: TÍNH DOANH THU THÁNG
-- ================================================================
CREATE OR REPLACE PROCEDURE sp_tinh_doanh_thu_thang (
    p_thang                 IN  NUMBER,
    p_nam                   IN  NUMBER,
    p_tong_doanh_thu        OUT NUMBER,
    p_so_hoadon             OUT NUMBER,
    p_doanh_thu_trung_binh  OUT NUMBER
)
AS
BEGIN
    SELECT
        NVL(SUM(hd.TongTien), 0),
        COUNT(DISTINCT hd.MaHoaDon),
        NVL(ROUND(AVG(hd.TongTien), 2), 0)
    INTO
        p_tong_doanh_thu,
        p_so_hoadon,
        p_doanh_thu_trung_binh
    FROM
        HOADON hd
    WHERE
        EXTRACT(MONTH FROM hd.NgayLap) = p_thang
        AND EXTRACT(YEAR FROM hd.NgayLap) = p_nam;

    DBMS_OUTPUT.PUT_LINE('Doanh thu tháng ' || p_thang || '/' || p_nam || ': ' || p_tong_doanh_thu);
    DBMS_OUTPUT.PUT_LINE('Số hóa đơn: ' || p_so_hoadon);
    DBMS_OUTPUT.PUT_LINE('Doanh thu trung bình: ' || p_doanh_thu_trung_binh);

EXCEPTION WHEN OTHERS THEN
    DBMS_OUTPUT.PUT_LINE('Lỗi: ' || SQLERRM);
    p_tong_doanh_thu := 0;
    p_so_hoadon := 0;
    p_doanh_thu_trung_binh := 0;
END sp_tinh_doanh_thu_thang;
/

-- ================================================================
-- PROCEDURE 3: THỐNG KÊ PHÒNG TRỐNG
-- ================================================================
CREATE OR REPLACE PROCEDURE sp_thong_ke_phong_trong (
    p_so_phong_trong        OUT NUMBER,
    p_so_phong_dang_su_dung OUT NUMBER,
    p_so_phong_bao_tri      OUT NUMBER,
    p_tong_phong            OUT NUMBER
)
AS
BEGIN
    SELECT COUNT(*) INTO p_so_phong_trong
    FROM PHONG WHERE TrangThai = 'AVAILABLE';

    SELECT COUNT(DISTINCT cdp.MaPhong) INTO p_so_phong_dang_su_dung
    FROM CHITIET_DATPHONG cdp
    JOIN DATPHONG dp ON cdp.MaDatPhong = dp.MaDatPhong
    WHERE dp.TrangThai IN ('PENDING', 'CONFIRMED', 'CHECKED_IN');

    SELECT COUNT(*) INTO p_so_phong_bao_tri
    FROM PHONG WHERE TrangThai = 'MAINTENANCE';

    SELECT COUNT(*) INTO p_tong_phong FROM PHONG;

    DBMS_OUTPUT.PUT_LINE('=== THỐNG KÊ PHÒNG ===');
    DBMS_OUTPUT.PUT_LINE('Phòng trống: ' || p_so_phong_trong || '/' || p_tong_phong);
    DBMS_OUTPUT.PUT_LINE('Phòng đang sử dụng: ' || p_so_phong_dang_su_dung);
    DBMS_OUTPUT.PUT_LINE('Phòng bảo trì: ' || p_so_phong_bao_tri);

EXCEPTION WHEN OTHERS THEN
    DBMS_OUTPUT.PUT_LINE('Lỗi: ' || SQLERRM);
    p_so_phong_trong := 0;
    p_so_phong_dang_su_dung := 0;
    p_so_phong_bao_tri := 0;
    p_tong_phong := 0;
END sp_thong_ke_phong_trong;
/

-- ================================================================
-- PROCEDURE 4: TÍNH LƯƠNG NHÂN VIÊN
-- ================================================================
CREATE OR REPLACE PROCEDURE sp_tinh_luong_nhanvien (
    p_ma_nhanvien   IN  NUMBER,
    p_thang         IN  NUMBER,
    p_nam           IN  NUMBER,
    p_luong_co_ban  OUT NUMBER,
    p_so_ngay_lam   OUT NUMBER,
    p_luong_thuc_te OUT NUMBER
)
AS
BEGIN
    SELECT Luong INTO p_luong_co_ban
    FROM NHANVIEN WHERE MaNhanVien = p_ma_nhanvien;

    -- Nếu không có bảng chấm công, trả về 26 ngày làm việc mặc định
    p_so_ngay_lam := 26;

    p_luong_thuc_te := ROUND((p_luong_co_ban / 26) * p_so_ngay_lam, 2);

    DBMS_OUTPUT.PUT_LINE('=== TÍNH LƯƠNG NHÂN VIÊN ===');
    DBMS_OUTPUT.PUT_LINE('Nhân viên ID: ' || p_ma_nhanvien);
    DBMS_OUTPUT.PUT_LINE('Lương cơ bản: ' || p_luong_co_ban);
    DBMS_OUTPUT.PUT_LINE('Số ngày làm tháng ' || p_thang || '/' || p_nam || ': ' || p_so_ngay_lam);
    DBMS_OUTPUT.PUT_LINE('Lương thực tế: ' || p_luong_thuc_te);

EXCEPTION WHEN OTHERS THEN
    DBMS_OUTPUT.PUT_LINE('Lỗi: ' || SQLERRM);
    p_luong_co_ban := 0;
    p_so_ngay_lam := 0;
    p_luong_thuc_te := 0;
END sp_tinh_luong_nhanvien;
/

-- ================================================================
-- PROCEDURE 5: CẬP NHẬT TRẠNG THÁI HÓA ĐƠN
-- ================================================================
CREATE OR REPLACE PROCEDURE sp_cap_nhat_hoadon (
    p_ma_hoadon IN NUMBER
)
AS
    v_tong_tien     NUMBER;
    v_da_thanhtoan  NUMBER;
    v_trang_thai    VARCHAR2(50);
BEGIN
    SELECT TongTien INTO v_tong_tien
    FROM HOADON WHERE MaHoaDon = p_ma_hoadon;

    SELECT NVL(SUM(SoTien), 0) INTO v_da_thanhtoan
    FROM THANHTOAN WHERE MaHoaDon = p_ma_hoadon;

    IF v_da_thanhtoan >= v_tong_tien THEN
        v_trang_thai := 'PAID';
    ELSIF v_da_thanhtoan > 0 THEN
        v_trang_thai := 'PARTIAL';
    ELSE
        v_trang_thai := 'UNPAID';
    END IF;

    UPDATE HOADON
    SET ThanhToan = v_da_thanhtoan, TrangThai = v_trang_thai
    WHERE MaHoaDon = p_ma_hoadon;

    COMMIT;

    DBMS_OUTPUT.PUT_LINE('✓ Hóa đơn #' || p_ma_hoadon || ' cập nhật: ' || v_trang_thai);

EXCEPTION WHEN OTHERS THEN
    DBMS_OUTPUT.PUT_LINE('Lỗi: ' || SQLERRM);
    ROLLBACK;
END sp_cap_nhat_hoadon;
/

PROMPT ✅ All stored procedures created successfully!

-- ================================================================
-- USAGE EXAMPLES
-- ================================================================

PROMPT
PROMPT === CÁCH SỬ DỤNG ===
PROMPT
PROMPT -- Doanh thu hôm nay:
PROMPT DECLARE v_dt NUMBER; v_hd NUMBER; v_kh NUMBER;
PROMPT BEGIN sp_tinh_doanh_thu_ngay(TRUNC(SYSDATE), v_dt, v_hd, v_kh); END;
PROMPT /
PROMPT
PROMPT -- Doanh thu tháng 3/2026:
PROMPT DECLARE v_dt NUMBER; v_hd NUMBER; v_tb NUMBER;
PROMPT BEGIN sp_tinh_doanh_thu_thang(3, 2026, v_dt, v_hd, v_tb); END;
PROMPT /
PROMPT
PROMPT -- Thống kê phòng:
PROMPT DECLARE v_t NUMBER; v_sd NUMBER; v_bt NUMBER; v_tg NUMBER;
PROMPT BEGIN sp_thong_ke_phong_trong(v_t, v_sd, v_bt, v_tg); END;
PROMPT /
