using System;
using System.Collections.Generic;
using System.Text;

namespace CodeDesign.Models
{
    public class PaymentHistory : ModelBase
    {
        public string code { get; set; }
        public string payment_user { get; set; }
        public PaymentUserInfo payment_info { get; set; }
        public List<PaymentTarget> payment_target { get; set; }
        public string package_id { get; set; }
        public long ngay_het_han { get; set; }
        public long ngay_kich_hoat { get; set; }
        public double so_tien_thu { get; set; }
        public double so_tien_xac_nhan { get; set; }
        public bool vat { get; set; }
        public PaymentStatus status { get; set; }
    }
    public class PaymentUserInfo
    {
        public string bank_account { get; set; }
        public string fullname { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string address { get; set; }
    }
    public class PaymentTarget
    {
        public string id { get; set; }
        public Role role { get; set; }
    }

}
