using System;
using System.Collections.Generic;
using System.Text;

namespace CodeDesign.Models
{
    public class PaymentHistory : ModelBase
    {
        public string ma { get; set; }
        public string payment_user { get; set; }
        public PaymentUserInfo payment_info { get; set; }
        public List<PaymentTarget> payment_target { get; set; }
        public string package_id { get; set; }
        public long ngay_het_han { get; set; }
        public long ngay_kich_hoat { get; set; }
        public double so_tien_thu { get; set; }
        public double so_tien_xac_nhan { get; set; }
        public bool vat { get; set; }
        public PaymentStatus payment_status { get; set; }
    }
    public class PaymentUserInfo
    {
        public string bank_account { get; set; }
        public string ho_ten { get; set; }
        public string dien_thoai { get; set; }
        public string email { get; set; }
        public string dia_chi { get; set; }
    }
    public class PaymentTarget
    {
        public string id { get; set; }
        public Role role { get; set; }
    }

}
