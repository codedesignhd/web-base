using System;
using System.Collections.Generic;
using System.Text;

namespace CodeDesign.Dtos.Vnpay
{
    public class VnPayQueryRequest : VnPayRequestBase
    {
        public long vnp_TransactionDate { get; set; }
    }
}
