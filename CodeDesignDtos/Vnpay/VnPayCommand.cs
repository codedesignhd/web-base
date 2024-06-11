using System;
using System.Collections.Generic;
using System.Text;

namespace CodeDesignDtos.Vnpay
{
    public abstract class VnPayCommand
    {
        public const string Payment = "pay";
        public const string Query = "querydr";
    }
    public abstract class VnPayConst
    {
        public const string DateTimeFormat = "yyyyMMddHHmmss";
    }
}
