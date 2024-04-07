using System;
using System.Collections.Generic;
using System.Text;

namespace CodeDesign.Models
{
    public enum SortDir
    {
        ALL = 0,
        ACS = 1,
        DECS = 2,
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

    public enum ThuocTinhHeThong
    {
        DEMO = -10000,
        DA_TONG_HOP = -1,
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
}
