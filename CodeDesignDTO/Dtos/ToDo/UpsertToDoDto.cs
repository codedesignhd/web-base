using CodeDesign.Models;
using System;

namespace CodeDesign.DTO.Dtos.ToDo
{
    public class UpsertToDoDto
    {
        public string title { get; set; }
        public string ngay_ket_thuc { get; set; }
        public TrangThaiThucHien trang_thai_thuc_hien { get; set; }
    }
}
