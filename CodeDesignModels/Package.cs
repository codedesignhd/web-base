using System;
using System.Collections.Generic;
using System.Text;

namespace CodeDesign.Models
{
    /// <summary>
    /// Gói dịch vụ của user
    /// </summary>
    public class Package : ModelBase
    {
        /// <summary>
        /// Tên dịch vụ
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// Mô tả dịch vụ
        /// </summary>
        public string description { get; set; }
        public string quyen_loi { get; set; }
        /// <summary>
        /// Số ngày sử dụng
        /// </summary>
        public int so_ngay { get; set; }
        public decimal money { get; set; }
        /// <summary>
        /// Chiết khấu %
        /// </summary>
        public int chiet_khau { get; set; }
        /// <summary>
        /// Giá khuyến mại
        /// </summary>
        public double khuyen_mai { get; set; }
        /// <summary>
        /// Số lượng dịch vụ được sử dụng
        /// </summary>
        public int so_luong { get; set; }
        public PackageType type { get; set; }
        public KhoangThoiGian time_unit { get; set; }
    }



    public class PackageDetail : ModelBase
    {
        /// <summary>
        /// Mã gói dịch vụ tự gen
        /// </summary>
        public string ma { get; set; }
        /// <summary>
        /// Id đối tượng mua hàng
        /// </summary>
        public string id_obj { get; set; }
        //Thông tin cá nhân người mua
        public InfoObj info_obj { get; set; }
        //Thông tin đối tượng nhận target

        public List<ObjTarget> id_obj_target { get; set; }
        public string id_dich_vu { get; set; }
        public string id_dich_vu_tang_kem { get; set; }
        public long ngay_het_han { get; set; }
        public long ngay_kich_hoat { get; set; }
        public int so_luong_con { get; set; }
        /// <summary>
        /// Số tiền thực thu
        /// </summary>
        public double so_tien_thu { get; set; }
        /// <summary>
        /// Số tiền kế toán xác nhận
        /// </summary>
        public double so_tien_xac_nhan { get; set; }

        public bool vat { get; set; }
        /// <summary>
        /// Số tiền giảm 
        /// </summary>
        public double so_tien_giam { get; set; }
        public string note { get; set; }
        public string ip { get; set; }
        /// <summary>
        /// Nếu là loại dịch vụ SUB thì set id của cha vào field này để có mối quan hệ
        /// </summary>
        public string owner { get; set; }
        public long ngay_thanh_toan { get; set; }

        /// <summary>
        /// Add dịch vụ trước người dùng có thể dùng mà để kích hoạt
        /// </summary>
        public string ma_kich_hoat { get; set; }
        public int thoi_gian_thu_kich_hoat { get; set; }
        public string ma_gift_code { get; set; }
    }
    public class InfoObj
    {
        public string ho_ten { get; set; }
        public string dien_thoai { get; set; }
        public string email { get; set; }
        public string dia_chi { get; set; }
    }
    public class ObjTarget
    {
        public string id { get; set; }
        
    }
}