using System;
using System.Collections.Generic;
using System.Text;

namespace CodeDesignModels
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
        Admin = 2,
        User = 3,
        Guest = 4,
    }
    public enum TrangThaiMail
    {
        All = 0,
        Success = 1,
        Failed = 2,
    }

    public enum DoiTuong
    {
        TaiKhoan = 1,
    }

    public enum ActionType
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
        ChoThanhToan = 1,
        DaThanhToan = 2,
        HetHan = 3,
    }

    public enum ExternalPaymentSource
    {
        Web = 0,
        VnPay = 1,
        Momo = 2,
        Paypal = 3,
    }

    public enum BankCode
    {
        VnPayQrCode = 1,
        VnPayVnBank = 2,
        VnPayIntCard = 3,
    }
    public enum CurrCode
    {
        Vnd = 1,
        Usd = 2,
    }

    public enum Locale
    {
        Vn = 0,
        En = 1
    }
}
