using System;
using System.Collections.Generic;
using System.Text;

namespace CodeDesign.Dtos.Vnpay
{
    public abstract class VnPayRequestBase
    {
        public string vnp_IpAddr { get; set; }
        public string vnp_OrderInfo { get; set; }
        //public string OrderType { get; set; }
        /// <summary>
        /// Mã đơn hàng
        /// </summary>
        public string vnp_TxnRef { get; set; }
        public long CreatedDate { get; set; }
    }
}
