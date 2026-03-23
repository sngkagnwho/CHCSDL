/* ================================================================
   HOTEL MANAGEMENT SYSTEM - ORACLE DATABASE SCHEMA
   Run as SYSDBA: sqlplus / as sysdba
   ================================================================ */

-- Create user MANA (schema owner)
CREATE USER mana IDENTIFIED BY mana123;
GRANT CONNECT, RESOURCE, DBA TO mana;
GRANT CREATE SESSION TO mana;

-- Connect as MANA
CONNECT mana/mana123;

-- ================================================================
-- TABLE 1: LOAIPHONG (Room Types)
-- ================================================================
CREATE SEQUENCE seq_loaiphong START WITH 1 INCREMENT BY 1;

CREATE TABLE LOAIPHONG (
    MaLoaiPhong     NUMBER PRIMARY KEY,
    TenLoaiPhong    VARCHAR2(100) NOT NULL,
    GiaCoban        NUMBER(12,2) DEFAULT 0,
    MoTa            VARCHAR2(500),
    SoGiuong        NUMBER DEFAULT 1
);

CREATE OR REPLACE TRIGGER trg_loaiphong_id
    BEFORE INSERT ON LOAIPHONG
    FOR EACH ROW
BEGIN
    IF :NEW.MaLoaiPhong IS NULL THEN
        SELECT seq_loaiphong.NEXTVAL INTO :NEW.MaLoaiPhong FROM dual;
    END IF;
END;
/

-- ================================================================
-- TABLE 2: PHONG (Rooms)
-- ================================================================
CREATE SEQUENCE seq_phong START WITH 1 INCREMENT BY 1;

CREATE TABLE PHONG (
    MaPhong         NUMBER PRIMARY KEY,
    SoPhong         VARCHAR2(20) NOT NULL UNIQUE,
    MaLoaiPhong     NUMBER REFERENCES LOAIPHONG(MaLoaiPhong),
    Tang            NUMBER DEFAULT 1,
    TrangThai       VARCHAR2(50) DEFAULT 'AVAILABLE' CHECK (TrangThai IN ('AVAILABLE','OCCUPIED','MAINTENANCE')),
    GiaNgay         NUMBER(12,2),
    MoTa            VARCHAR2(1000)
);

CREATE OR REPLACE TRIGGER trg_phong_id
    BEFORE INSERT ON PHONG
    FOR EACH ROW
BEGIN
    IF :NEW.MaPhong IS NULL THEN
        SELECT seq_phong.NEXTVAL INTO :NEW.MaPhong FROM dual;
    END IF;
END;
/

-- ================================================================
-- TABLE 3: KHACHHANG (Customers)
-- ================================================================
CREATE SEQUENCE seq_khachhang START WITH 1 INCREMENT BY 1;

CREATE TABLE KHACHHANG (
    MaKhachHang     NUMBER PRIMARY KEY,
    TenKhachHang    VARCHAR2(100) NOT NULL,
    Cmnd            VARCHAR2(20) NOT NULL UNIQUE,
    Sdt             VARCHAR2(15),
    Email           VARCHAR2(200),
    DiaChi          VARCHAR2(300),
    QuocTich        VARCHAR2(50),
    NgaySinh        DATE
);

CREATE OR REPLACE TRIGGER trg_khachhang_id
    BEFORE INSERT ON KHACHHANG
    FOR EACH ROW
BEGIN
    IF :NEW.MaKhachHang IS NULL THEN
        SELECT seq_khachhang.NEXTVAL INTO :NEW.MaKhachHang FROM dual;
    END IF;
END;
/

-- ================================================================
-- TABLE 4: DATPHONG (Bookings)
-- ================================================================
CREATE SEQUENCE seq_datphong START WITH 1 INCREMENT BY 1;

CREATE TABLE DATPHONG (
    MaDatPhong      NUMBER PRIMARY KEY,
    MaKhachHang     NUMBER REFERENCES KHACHHANG(MaKhachHang),
    NgayDatPhong    DATE DEFAULT SYSDATE,
    NgayCheckIn     DATE NOT NULL,
    NgayCheckOut    DATE NOT NULL,
    TrangThai       VARCHAR2(50) DEFAULT 'PENDING' CHECK (TrangThai IN ('PENDING','CONFIRMED','CHECKED_IN','CHECKED_OUT','CANCELLED')),
    GhiChu          VARCHAR2(500),
    TongTien        NUMBER(12,2),
    CONSTRAINT chk_dates CHECK (NgayCheckOut > NgayCheckIn)
);

CREATE OR REPLACE TRIGGER trg_datphong_id
    BEFORE INSERT ON DATPHONG
    FOR EACH ROW
BEGIN
    IF :NEW.MaDatPhong IS NULL THEN
        SELECT seq_datphong.NEXTVAL INTO :NEW.MaDatPhong FROM dual;
    END IF;
END;
/

-- ================================================================
-- TABLE 5: CHITIET_DATPHONG (Booking Details)
-- ================================================================
CREATE SEQUENCE seq_chitietchitiet START WITH 1 INCREMENT BY 1;

CREATE TABLE CHITIET_DATPHONG (
    MaChiTiet       NUMBER PRIMARY KEY,
    MaDatPhong      NUMBER REFERENCES DATPHONG(MaDatPhong) ON DELETE CASCADE,
    MaPhong         NUMBER REFERENCES PHONG(MaPhong),
    GiaThue         NUMBER(12,2) DEFAULT 0
);

CREATE OR REPLACE TRIGGER trg_chitiet_id
    BEFORE INSERT ON CHITIET_DATPHONG
    FOR EACH ROW
BEGIN
    IF :NEW.MaChiTiet IS NULL THEN
        SELECT seq_chitietchitiet.NEXTVAL INTO :NEW.MaChiTiet FROM dual;
    END IF;
END;
/

-- ================================================================
-- TABLE 6: HOADON (Invoices)
-- ================================================================
CREATE SEQUENCE seq_hoadon START WITH 1 INCREMENT BY 1;

CREATE TABLE HOADON (
    MaHoaDon        NUMBER PRIMARY KEY,
    MaDatPhong      NUMBER UNIQUE REFERENCES DATPHONG(MaDatPhong),
    NgayLap         DATE DEFAULT SYSDATE,
    TongTien        NUMBER(12,2) DEFAULT 0,
    GiamGia         NUMBER(5,2) DEFAULT 0,
    ThueVat         NUMBER(5,2) DEFAULT 10,
    ThanhToan       NUMBER(12,2) DEFAULT 0,
    TrangThai       VARCHAR2(50) DEFAULT 'UNPAID' CHECK (TrangThai IN ('UNPAID','PARTIAL','PAID')),
    GhiChu          VARCHAR2(500)
);

CREATE OR REPLACE TRIGGER trg_hoadon_id
    BEFORE INSERT ON HOADON
    FOR EACH ROW
BEGIN
    IF :NEW.MaHoaDon IS NULL THEN
        SELECT seq_hoadon.NEXTVAL INTO :NEW.MaHoaDon FROM dual;
    END IF;
END;
/

-- ================================================================
-- TABLE 7: THANHTOAN (Payments)
-- ================================================================
CREATE SEQUENCE seq_thanhtoan START WITH 1 INCREMENT BY 1;

CREATE TABLE THANHTOAN (
    MaThanhToan     NUMBER PRIMARY KEY,
    MaHoaDon        NUMBER REFERENCES HOADON(MaHoaDon),
    NgayThanhToan   DATE DEFAULT SYSDATE,
    SoTien          NUMBER(12,2) NOT NULL,
    PhuongThuc      VARCHAR2(50) DEFAULT 'CASH' CHECK (PhuongThuc IN ('CASH','CARD','TRANSFER','OTHER')),
    GhiChu          VARCHAR2(200)
);

CREATE OR REPLACE TRIGGER trg_thanhtoan_id
    BEFORE INSERT ON THANHTOAN
    FOR EACH ROW
BEGIN
    IF :NEW.MaThanhToan IS NULL THEN
        SELECT seq_thanhtoan.NEXTVAL INTO :NEW.MaThanhToan FROM dual;
    END IF;
END;
/

-- ================================================================
-- TABLE 8: NHANVIEN (Employees)
-- ================================================================
CREATE SEQUENCE seq_nhanvien START WITH 1 INCREMENT BY 1;

CREATE TABLE NHANVIEN (
    MaNhanVien      NUMBER PRIMARY KEY,
    TenNhanVien     VARCHAR2(100) NOT NULL,
    ChucVu          VARCHAR2(50),
    Sdt             VARCHAR2(15),
    Email           VARCHAR2(200),
    Luong           NUMBER(12,2) DEFAULT 0,
    NgayVaoLam      DATE,
    TrangThai       VARCHAR2(20) DEFAULT 'ACTIVE' CHECK (TrangThai IN ('ACTIVE','INACTIVE'))
);

CREATE OR REPLACE TRIGGER trg_nhanvien_id
    BEFORE INSERT ON NHANVIEN
    FOR EACH ROW
BEGIN
    IF :NEW.MaNhanVien IS NULL THEN
        SELECT seq_nhanvien.NEXTVAL INTO :NEW.MaNhanVien FROM dual;
    END IF;
END;
/

-- ================================================================
-- SAMPLE DATA
-- ================================================================

-- Loại phòng
INSERT INTO LOAIPHONG (TenLoaiPhong, GiaCoban, MoTa, SoGiuong) VALUES ('Phòng Đơn', 500000, 'Phòng tiêu chuẩn 1 giường', 1);
INSERT INTO LOAIPHONG (TenLoaiPhong, GiaCoban, MoTa, SoGiuong) VALUES ('Phòng Đôi', 800000, 'Phòng tiêu chuẩn 2 giường', 2);
INSERT INTO LOAIPHONG (TenLoaiPhong, GiaCoban, MoTa, SoGiuong) VALUES ('Suite', 2000000, 'Phòng cao cấp, view đẹp', 1);
INSERT INTO LOAIPHONG (TenLoaiPhong, GiaCoban, MoTa, SoGiuong) VALUES ('Phòng VIP', 3500000, 'Phòng hạng nhất, đầy đủ tiện nghi', 1);

-- Phòng
INSERT INTO PHONG (SoPhong, MaLoaiPhong, Tang, TrangThai, GiaNgay) VALUES ('101', 1, 1, 'AVAILABLE', 500000);
INSERT INTO PHONG (SoPhong, MaLoaiPhong, Tang, TrangThai, GiaNgay) VALUES ('102', 1, 1, 'AVAILABLE', 500000);
INSERT INTO PHONG (SoPhong, MaLoaiPhong, Tang, TrangThai, GiaNgay) VALUES ('103', 2, 1, 'AVAILABLE', 800000);
INSERT INTO PHONG (SoPhong, MaLoaiPhong, Tang, TrangThai, GiaNgay) VALUES ('201', 2, 2, 'AVAILABLE', 800000);
INSERT INTO PHONG (SoPhong, MaLoaiPhong, Tang, TrangThai, GiaNgay) VALUES ('202', 3, 2, 'AVAILABLE', 2000000);
INSERT INTO PHONG (SoPhong, MaLoaiPhong, Tang, TrangThai, GiaNgay) VALUES ('301', 4, 3, 'AVAILABLE', 3500000);

-- Nhân viên
INSERT INTO NHANVIEN (TenNhanVien, ChucVu, Sdt, Email, Luong, NgayVaoLam) VALUES ('Nguyễn Văn An', 'Quản lý', '0901234567', 'an@hotel.vn', 15000000, DATE '2020-01-01');
INSERT INTO NHANVIEN (TenNhanVien, ChucVu, Sdt, Email, Luong, NgayVaoLam) VALUES ('Trần Thị Bình', 'Lễ tân', '0912345678', 'binh@hotel.vn', 8000000, DATE '2021-03-15');
INSERT INTO NHANVIEN (TenNhanVien, ChucVu, Sdt, Email, Luong, NgayVaoLam) VALUES ('Lê Văn Cường', 'Bảo vệ', '0923456789', 'cuong@hotel.vn', 6000000, DATE '2022-06-01');

COMMIT;

PROMPT ✅ Database schema and sample data created successfully!
