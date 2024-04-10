using System;
using System.Collections.Generic;
using System.Text;

namespace CodeDesign.Models
{
    public enum ThuocTinhHeThong
    {
        DEMO = -10000,
        DA_TONG_HOP = -1,
        GoiCuocHienThi = 1,



    }
    public enum SortDir
    {
        ALL = 0,
        Asc = 1,
        Desc = 2,
    }
    public enum TrangThaiDuLieu
    {
        XOA = -1,
        ALL = 0,
    }

    public enum TrangThaiThucHien
    {
        ALL = 0,
        OPEN = 1,
        IN_PROGRESS = 2,
        CANCEL = 3,
        DONE = 4,
    }
    public enum Role
    {
        ALL = 0,
        SYS = 1,
        USER = 2,
        GUEST = 3,
        ADMIN = 4,
    }
    public enum TrangThaiMail
    {
        ALL = 0,
        THANH_CONG = 1,
        LOI = 2,
    }



    public enum DOI_TUONG
    {
        TAI_KHOAN = 1,

    }

    public enum ACTION
    {
        ALL = 0,
        VIEW = 1,
        CREATE = 2,
        UPDATE = 3,
        DELETE = 4,
    }

    public enum DurationUnit
    {
        ALL = 0,
        NGAY = 1,
        TUAN = 2,
        THANG = 3,
        NAM = 4,
        FULL = 5,
    }

    public enum PaymentStatus
    {
        All = 0,
        DA_THANH_TOAN = 1,
        CHO_THANH_TOAN = 2,
        HET_HAN = 3,
    }
}
