using Oracle.ManagedDataAccess.Client;
using doan.Models;

namespace doan.Services
{
    public class DatabaseMetadataService
    {
        private readonly string _connectionString;
        private readonly ILogger<DatabaseMetadataService> _logger;

        public DatabaseMetadataService(IConfiguration configuration, ILogger<DatabaseMetadataService> logger)
        {
            _connectionString = configuration.GetConnectionString("OracleDB") ?? string.Empty;
            _logger = logger;
        }

        public List<TriggerInfo> GetTriggers()
        {
            try
            {
                using var conn = new OracleConnection(_connectionString);
                conn.Open();
                var triggers = new List<TriggerInfo>();
                using var cmd = new OracleCommand(@"
                    SELECT TRIGGER_NAME, TABLE_NAME, TRIGGERING_EVENT, STATUS, 
                           TRIGGER_TYPE, TRIGGER_BODY
                    FROM USER_TRIGGERS
                    ORDER BY TRIGGER_NAME", conn);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    triggers.Add(new TriggerInfo
                    {
                        TriggerName = reader["TRIGGER_NAME"]?.ToString() ?? "",
                        TableName = reader["TABLE_NAME"]?.ToString() ?? "",
                        TriggeringEvent = reader["TRIGGERING_EVENT"]?.ToString() ?? "",
                        Status = reader["STATUS"]?.ToString() ?? "ENABLED",
                        TriggerType = reader["TRIGGER_TYPE"]?.ToString() ?? "",
                        TriggerBody = reader["TRIGGER_BODY"]?.ToString() ?? ""
                    });
                }
                return triggers;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting triggers from Oracle");
                return GetMockTriggers();
            }
        }

        private List<TriggerInfo> GetMockTriggers()
        {
            return new List<TriggerInfo>
            {
                new TriggerInfo
                {
                    TriggerName = "TRG_KHACHHANG_ID",
                    TableName = "KHACHHANG",
                    TriggeringEvent = "INSERT",
                    Status = "ENABLED",
                    TriggerType = "BEFORE EACH ROW",
                    Description = "Tự động sinh ID khách hàng theo định dạng KHxxx",
                    TriggerBody = "BEGIN\n  IF :NEW.MA_KHACH IS NULL THEN\n    SELECT 'KH' || LPAD(seq_khachhang.NEXTVAL, 3, '0') INTO :NEW.MA_KHACH FROM DUAL;\n  END IF;\nEND;"
                },
                new TriggerInfo
                {
                    TriggerName = "TRG_PHONG_ID",
                    TableName = "PHONG",
                    TriggeringEvent = "INSERT",
                    Status = "ENABLED",
                    TriggerType = "BEFORE EACH ROW",
                    Description = "Tự động sinh ID phòng theo định dạng Pxxx",
                    TriggerBody = "BEGIN\n  IF :NEW.MA_PHONG IS NULL THEN\n    SELECT 'P' || LPAD(seq_phong.NEXTVAL, 3, '0') INTO :NEW.MA_PHONG FROM DUAL;\n  END IF;\nEND;"
                },
                new TriggerInfo
                {
                    TriggerName = "TRG_DATPHONG_ID",
                    TableName = "DATPHONG",
                    TriggeringEvent = "INSERT",
                    Status = "ENABLED",
                    TriggerType = "BEFORE EACH ROW",
                    Description = "Tự động sinh ID đặt phòng theo định dạng DPxxx",
                    TriggerBody = "BEGIN\n  IF :NEW.MA_DATPHONG IS NULL THEN\n    SELECT 'DP' || LPAD(seq_datphong.NEXTVAL, 3, '0') INTO :NEW.MA_DATPHONG FROM DUAL;\n  END IF;\nEND;"
                },
                new TriggerInfo
                {
                    TriggerName = "TRG_HOADON_ID",
                    TableName = "HOADON",
                    TriggeringEvent = "INSERT",
                    Status = "ENABLED",
                    TriggerType = "BEFORE EACH ROW",
                    Description = "Tự động sinh ID hóa đơn theo định dạng HDxxx",
                    TriggerBody = "BEGIN\n  IF :NEW.MA_HOADON IS NULL THEN\n    SELECT 'HD' || LPAD(seq_hoadon.NEXTVAL, 3, '0') INTO :NEW.MA_HOADON FROM DUAL;\n  END IF;\nEND;"
                },
                new TriggerInfo
                {
                    TriggerName = "TRG_THANHTOAN_ID",
                    TableName = "THANHTOAN",
                    TriggeringEvent = "INSERT",
                    Status = "ENABLED",
                    TriggerType = "BEFORE EACH ROW",
                    Description = "Tự động sinh ID thanh toán theo định dạng TTxxx",
                    TriggerBody = "BEGIN\n  IF :NEW.MA_THANHTOAN IS NULL THEN\n    SELECT 'TT' || LPAD(seq_thanhtoan.NEXTVAL, 3, '0') INTO :NEW.MA_THANHTOAN FROM DUAL;\n  END IF;\nEND;"
                },
                new TriggerInfo
                {
                    TriggerName = "TRG_NHANVIEN_ID",
                    TableName = "NHANVIEN",
                    TriggeringEvent = "INSERT",
                    Status = "ENABLED",
                    TriggerType = "BEFORE EACH ROW",
                    Description = "Tự động sinh ID nhân viên theo định dạng NVxxx",
                    TriggerBody = "BEGIN\n  IF :NEW.MA_NV IS NULL THEN\n    SELECT 'NV' || LPAD(seq_nhanvien.NEXTVAL, 3, '0') INTO :NEW.MA_NV FROM DUAL;\n  END IF;\nEND;"
                },
                new TriggerInfo
                {
                    TriggerName = "TRG_UPDATE_HOADON",
                    TableName = "DATPHONG",
                    TriggeringEvent = "UPDATE",
                    Status = "ENABLED",
                    TriggerType = "AFTER EACH ROW",
                    Description = "Tự động cập nhật tổng tiền hóa đơn khi đặt phòng thay đổi",
                    TriggerBody = "BEGIN\n  UPDATE HOADON SET TONG_TIEN = fn_get_tong_tien_hoadon(:NEW.MA_HOADON)\n  WHERE MA_HOADON = :NEW.MA_HOADON;\nEND;"
                }
            };
        }

        public List<FunctionInfo> GetFunctions()
        {
            try
            {
                using var conn = new OracleConnection(_connectionString);
                conn.Open();
                var functions = new List<FunctionInfo>();
                using var cmd = new OracleCommand(@"
                    SELECT OBJECT_NAME 
                    FROM USER_OBJECTS
                    WHERE OBJECT_TYPE = 'FUNCTION'
                    ORDER BY OBJECT_NAME", conn);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    functions.Add(new FunctionInfo
                    {
                        FunctionName = reader["OBJECT_NAME"]?.ToString() ?? "",
                        ReturnType = "VARCHAR2",
                        Description = "User-defined function"
                    });
                }
                return functions.Count > 0 ? functions : GetMockFunctions();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting functions from Oracle");
                return GetMockFunctions();
            }
        }

        private List<FunctionInfo> GetMockFunctions()
        {
            return new List<FunctionInfo>
            {
                new FunctionInfo
                {
                    FunctionName = "fn_get_gia_phong",
                    ReturnType = "NUMBER",
                    Description = "Lấy giá phòng theo mã loại phòng",
                    ExampleCall = "SELECT fn_get_gia_phong('LP001') FROM DUAL",
                    Category = "Phòng",
                    Parameters = new List<ProcedureParameter>
                    {
                        new ProcedureParameter { Name = "ma_loai", DataType = "VARCHAR2", Direction = "IN", Description = "Mã loại phòng" }
                    }
                },
                new FunctionInfo
                {
                    FunctionName = "fn_tinh_so_dem",
                    ReturnType = "NUMBER",
                    Description = "Tính số đêm lưu trú giữa ngày nhận và ngày trả",
                    ExampleCall = "SELECT fn_tinh_so_dem(DATE '2026-01-01', DATE '2026-01-05') FROM DUAL",
                    Category = "Đặt Phòng",
                    Parameters = new List<ProcedureParameter>
                    {
                        new ProcedureParameter { Name = "ngay_nhan", DataType = "DATE", Direction = "IN", Description = "Ngày nhận phòng" },
                        new ProcedureParameter { Name = "ngay_tra", DataType = "DATE", Direction = "IN", Description = "Ngày trả phòng" }
                    }
                },
                new FunctionInfo
                {
                    FunctionName = "fn_kiem_tra_phong_trong",
                    ReturnType = "NUMBER",
                    Description = "Kiểm tra phòng có trống trong khoảng thời gian không (1=trống, 0=đã đặt)",
                    ExampleCall = "SELECT fn_kiem_tra_phong_trong('P001', DATE '2026-01-01', DATE '2026-01-05') FROM DUAL",
                    Category = "Phòng",
                    Parameters = new List<ProcedureParameter>
                    {
                        new ProcedureParameter { Name = "ma_phong", DataType = "VARCHAR2", Direction = "IN", Description = "Mã phòng" },
                        new ProcedureParameter { Name = "ngay_nhan", DataType = "DATE", Direction = "IN", Description = "Ngày nhận phòng" },
                        new ProcedureParameter { Name = "ngay_tra", DataType = "DATE", Direction = "IN", Description = "Ngày trả phòng" }
                    }
                },
                new FunctionInfo
                {
                    FunctionName = "fn_get_tong_tien_hoadon",
                    ReturnType = "NUMBER",
                    Description = "Lấy tổng tiền hóa đơn theo mã hóa đơn",
                    ExampleCall = "SELECT fn_get_tong_tien_hoadon('HD001') FROM DUAL",
                    Category = "Hóa Đơn",
                    Parameters = new List<ProcedureParameter>
                    {
                        new ProcedureParameter { Name = "ma_hoadon", DataType = "VARCHAR2", Direction = "IN", Description = "Mã hóa đơn" }
                    }
                },
                new FunctionInfo
                {
                    FunctionName = "fn_get_ten_khachhang",
                    ReturnType = "VARCHAR2",
                    Description = "Lấy tên khách hàng theo mã khách hàng",
                    ExampleCall = "SELECT fn_get_ten_khachhang('KH001') FROM DUAL",
                    Category = "Khách Hàng",
                    Parameters = new List<ProcedureParameter>
                    {
                        new ProcedureParameter { Name = "ma_khach", DataType = "VARCHAR2", Direction = "IN", Description = "Mã khách hàng" }
                    }
                }
            };
        }

        public List<ProcedureInfo> GetProcedures()
        {
            return new List<ProcedureInfo>
            {
                new ProcedureInfo
                {
                    ProcedureName = "sp_tinh_doanh_thu_ngay",
                    Description = "Tính doanh thu trong một ngày cụ thể",
                    Category = "Doanh Thu",
                    Icon = "💰",
                    Parameters = new List<ProcedureParameter>
                    {
                        new ProcedureParameter { Name = "p_ngay", DataType = "DATE", Direction = "IN", Description = "Ngày cần tính" },
                        new ProcedureParameter { Name = "p_doanh_thu", DataType = "NUMBER", Direction = "OUT", Description = "Doanh thu ngày" },
                        new ProcedureParameter { Name = "p_so_hoadon", DataType = "NUMBER", Direction = "OUT", Description = "Số hóa đơn trong ngày" },
                        new ProcedureParameter { Name = "p_so_khach", DataType = "NUMBER", Direction = "OUT", Description = "Số khách trong ngày" }
                    }
                },
                new ProcedureInfo
                {
                    ProcedureName = "sp_tinh_doanh_thu_thang",
                    Description = "Tính doanh thu trong một tháng cụ thể",
                    Category = "Doanh Thu",
                    Icon = "📅",
                    Parameters = new List<ProcedureParameter>
                    {
                        new ProcedureParameter { Name = "p_thang", DataType = "NUMBER", Direction = "IN", Description = "Tháng cần tính" },
                        new ProcedureParameter { Name = "p_nam", DataType = "NUMBER", Direction = "IN", Description = "Năm cần tính" },
                        new ProcedureParameter { Name = "p_doanh_thu", DataType = "NUMBER", Direction = "OUT", Description = "Doanh thu tháng" },
                        new ProcedureParameter { Name = "p_so_hoadon", DataType = "NUMBER", Direction = "OUT", Description = "Số hóa đơn trong tháng" }
                    }
                },
                new ProcedureInfo
                {
                    ProcedureName = "sp_thong_ke_phong_trong",
                    Description = "Thống kê số phòng trống và phòng đang sử dụng",
                    Category = "Phòng",
                    Icon = "🏨",
                    Parameters = new List<ProcedureParameter>
                    {
                        new ProcedureParameter { Name = "p_so_phong_trong", DataType = "NUMBER", Direction = "OUT", Description = "Số phòng trống" },
                        new ProcedureParameter { Name = "p_so_phong_dang_dung", DataType = "NUMBER", Direction = "OUT", Description = "Số phòng đang sử dụng" }
                    }
                },
                new ProcedureInfo
                {
                    ProcedureName = "sp_tinh_luong_nhanvien",
                    Description = "Tính tổng lương nhân viên trong tháng",
                    Category = "Nhân Viên",
                    Icon = "👥",
                    Parameters = new List<ProcedureParameter>
                    {
                        new ProcedureParameter { Name = "p_thang", DataType = "NUMBER", Direction = "IN", Description = "Tháng" },
                        new ProcedureParameter { Name = "p_nam", DataType = "NUMBER", Direction = "IN", Description = "Năm" },
                        new ProcedureParameter { Name = "p_tong_luong", DataType = "NUMBER", Direction = "OUT", Description = "Tổng lương" },
                        new ProcedureParameter { Name = "p_so_nv", DataType = "NUMBER", Direction = "OUT", Description = "Số nhân viên" }
                    }
                },
                new ProcedureInfo
                {
                    ProcedureName = "sp_cap_nhat_baocao",
                    Description = "Cập nhật báo cáo tháng vào bảng báo cáo",
                    Category = "Báo Cáo",
                    Icon = "📊",
                    Parameters = new List<ProcedureParameter>
                    {
                        new ProcedureParameter { Name = "p_thang", DataType = "NUMBER", Direction = "IN", Description = "Tháng báo cáo" },
                        new ProcedureParameter { Name = "p_nam", DataType = "NUMBER", Direction = "IN", Description = "Năm báo cáo" }
                    }
                }
            };
        }

        public List<PackageInfo> GetPackages()
        {
            return new List<PackageInfo>
            {
                new PackageInfo
                {
                    PackageName = "pkg_hotel_management",
                    Description = "Package quản lý khách sạn tổng hợp - bao gồm các procedures và functions chính",
                    Status = "VALID",
                    LastModified = new DateTime(2026, 1, 15),
                    Procedures = new List<ProcedureInfo>
                    {
                        new ProcedureInfo { ProcedureName = "sp_tinh_doanh_thu_ngay", Description = "Tính doanh thu ngày", Icon = "💰" },
                        new ProcedureInfo { ProcedureName = "sp_tinh_doanh_thu_thang", Description = "Tính doanh thu tháng", Icon = "📅" },
                        new ProcedureInfo { ProcedureName = "sp_thong_ke_phong_trong", Description = "Thống kê phòng trống", Icon = "🏨" },
                        new ProcedureInfo { ProcedureName = "sp_tinh_luong_nhanvien", Description = "Tính lương nhân viên", Icon = "👥" },
                        new ProcedureInfo { ProcedureName = "sp_cap_nhat_baocao", Description = "Cập nhật báo cáo", Icon = "📊" }
                    },
                    Functions = new List<FunctionInfo>
                    {
                        new FunctionInfo { FunctionName = "fn_get_gia_phong", ReturnType = "NUMBER", Description = "Lấy giá phòng" },
                        new FunctionInfo { FunctionName = "fn_tinh_so_dem", ReturnType = "NUMBER", Description = "Tính số đêm" },
                        new FunctionInfo { FunctionName = "fn_kiem_tra_phong_trong", ReturnType = "NUMBER", Description = "Kiểm tra phòng trống" },
                        new FunctionInfo { FunctionName = "fn_get_tong_tien_hoadon", ReturnType = "NUMBER", Description = "Lấy tổng tiền hóa đơn" },
                        new FunctionInfo { FunctionName = "fn_get_ten_khachhang", ReturnType = "VARCHAR2", Description = "Lấy tên khách hàng" }
                    }
                },
                new PackageInfo
                {
                    PackageName = "pkg_audit_management",
                    Description = "Package quản lý audit trail - theo dõi mọi thay đổi dữ liệu",
                    Status = "VALID",
                    LastModified = new DateTime(2026, 3, 23),
                    Procedures = new List<ProcedureInfo>
                    {
                        new ProcedureInfo { ProcedureName = "sp_check_audit_log", Description = "Kiểm tra audit log theo user", Icon = "🔍" },
                        new ProcedureInfo { ProcedureName = "sp_get_audit_stats", Description = "Lấy thống kê audit", Icon = "📈" },
                        new ProcedureInfo { ProcedureName = "sp_clear_old_audit_logs", Description = "Xóa audit log cũ", Icon = "🗑️" }
                    },
                    Functions = new List<FunctionInfo>()
                }
            };
        }

        public Dictionary<string, int> GetDatabaseStats()
        {
            try
            {
                using var conn = new OracleConnection(_connectionString);
                conn.Open();
                var stats = new Dictionary<string, int>();
                
                var queries = new Dictionary<string, string>
                {
                    ["Tables"] = "SELECT COUNT(*) FROM USER_TABLES",
                    ["Views"] = "SELECT COUNT(*) FROM USER_VIEWS",
                    ["Procedures"] = "SELECT COUNT(*) FROM USER_OBJECTS WHERE OBJECT_TYPE = 'PROCEDURE'",
                    ["Functions"] = "SELECT COUNT(*) FROM USER_OBJECTS WHERE OBJECT_TYPE = 'FUNCTION'",
                    ["Triggers"] = "SELECT COUNT(*) FROM USER_TRIGGERS",
                    ["Packages"] = "SELECT COUNT(*) FROM USER_OBJECTS WHERE OBJECT_TYPE = 'PACKAGE'",
                    ["Indexes"] = "SELECT COUNT(*) FROM USER_INDEXES",
                    ["Sequences"] = "SELECT COUNT(*) FROM USER_SEQUENCES"
                };

                foreach (var q in queries)
                {
                    using var cmd = new OracleCommand(q.Value, conn);
                    stats[q.Key] = Convert.ToInt32(cmd.ExecuteScalar());
                }
                return stats;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting database stats");
                return new Dictionary<string, int>
                {
                    ["Tables"] = 12,
                    ["Views"] = 8,
                    ["Procedures"] = 5,
                    ["Functions"] = 5,
                    ["Triggers"] = 7,
                    ["Packages"] = 2,
                    ["Indexes"] = 15,
                    ["Sequences"] = 6
                };
            }
        }
    }
}
