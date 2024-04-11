using System;
using System.Collections.Generic;
using System.Text;

namespace CodeDesign.Models
{
    public enum ThuocTinhHeThong
    {
        Demo = -10000,
        DaTongHop = -1,
        GoiCuocHienThi = 1,



    }
    public enum SortDir
    {
        All = 0,
        Asc = 1,
        Desc = 2,
    }
    public enum TrangThaiDuLieu
    {
        Deleted = -1,
        All = 0,
    }

    public enum Role
    {
        All = 0,
        Sys = 1,
        User = 2,
        Guest = 3,
        Admin = 4,
    }
    public enum TrangThaiMail
    {
        All = 0,
        Success = 1,
        Failed = 2,
    }



    public enum DOI_TUONG
    {
        TaiKhoan = 1,

    }

    public enum ACTION
    {
        All = 0,
        View = 1,
        Create = 2,
        Update = 3,
        Delete = 4,
    }

    public enum DurationUnit
    {
        All = 0,
        Day = 1,
        Week = 2,
        Month = 3,
        Year = 4,
        Full = 5,
    }

    public enum PaymentStatus
    {
        All = 0,
        DaThanhToan = 1,
        ChoThanhToan = 2,
        HetHan = 3,
    }
}
