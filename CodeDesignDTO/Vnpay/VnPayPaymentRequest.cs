namespace CodeDesign.Dtos.Vnpay
{
    public class VnPayPaymentRequest : VnPayRequestBase
    {

        public VnPayCurrCode CurrCode { get; set; }

        public VnPayBankCode BankCode { get; set; }
        public decimal Amount { get; set; }

        public VnPayLocale Locale { get; set; }
    }
}
