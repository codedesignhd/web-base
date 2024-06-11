using System;
using System.Collections.Generic;
using System.Text;

namespace CodeDesignDtos.Vnpay
{
    public class VnPayPaymentResponse
    {
        public string vnp_TxnRef { get; set; }
        public decimal vnp_Amount { get; set; }
        public string vnp_TransactionNo { get; set; }
        public string vnp_ResponseCode { get; set; }
        public string vnp_TransactionStatus { get; set; }
        public string vnp_SecureHash { get; set; }
    }
}
