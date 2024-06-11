using CodeDesignModels;

namespace CodeDesignDtos.Vnpay
{
    public class VnPayPaymentRequest : VnPayRequestBase
    {
        public CurrCode CurrCode { get; set; }
        public BankCode BankCode { get; set; }
        public decimal Amount { get; set; }

        public Locale Locale { get; set; }
    }
}
