using CodeDesign.Models;
using System;

namespace CodeDesign.Dtos
{
    public class UpsertToDoDto
    {
        public string title { get; set; }
        public string ngay_ket_thuc { get; set; }
        public TrangThaiThucHien trang_thai_thuc_hien { get; set; }
    }
}
