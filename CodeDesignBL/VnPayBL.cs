using CodeDesignDtos.Responses;
using CodeDesignDtos.Vnpay;
using CodeDesignModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Utilities = CodeDesignUtilities;
namespace CodeDesignBL
{
    public sealed class VnPayBL
    {
        private const string VERSION = "2.1.0";
        private SortedList<string, string> _requestData = new SortedList<string, string>(new VnPayCompare());
        private SortedList<string, string> _responseData = new SortedList<string, string>(new VnPayCompare());

        private static VnPayBL _instance;

        public static VnPayBL Instance
        {
            get
            {
                if (_instance is null)
                {
                    _instance = new VnPayBL();
                }
                return _instance;
            }
        }



        private void addRequestData(string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                _requestData.Add(key, value);
            }
        }

        private void addResponseData(string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                _responseData.Add(key, value);
            }
        }

        private string getResponseData(string key)
        {
            string retValue;
            if (_responseData.TryGetValue(key, out retValue))
            {
                return retValue;
            }
            else
            {
                return string.Empty;
            }
        }

        #region Request
        private string createRequestUrl(string baseUrl, string vnp_HashSecret)
        {
            StringBuilder data = new StringBuilder();
            foreach (KeyValuePair<string, string> kv in _requestData)
            {
                if (!string.IsNullOrEmpty(kv.Value))
                {
                    data.Append(WebUtility.UrlEncode(kv.Key) + "=" + WebUtility.UrlEncode(kv.Value) + "&");
                }
            }
            string queryString = data.ToString();

            baseUrl += "?" + queryString;
            string signData = queryString;
            if (signData.Length > 0)
            {
                signData = signData.Remove(data.Length - 1, 1);
            }
            string vnp_SecureHash = Utilities.CryptoUtils.HmacSHA512(vnp_HashSecret, signData);
            baseUrl += "vnp_SecureHash=" + vnp_SecureHash;

            return baseUrl;
        }
        #endregion

        #region Response process

        private bool validateSignature(string inputHash, string secretKey)
        {
            string rspRaw = getResponseData();
            string myChecksum = Utilities.CryptoUtils.HmacSHA512(secretKey, rspRaw);
            return myChecksum.Equals(inputHash, StringComparison.InvariantCultureIgnoreCase);
        }
        private string getResponseData()
        {
            StringBuilder data = new StringBuilder();
            if (_responseData.ContainsKey("vnp_SecureHashType"))
            {
                _responseData.Remove("vnp_SecureHashType");
            }
            if (_responseData.ContainsKey("vnp_SecureHash"))
            {
                _responseData.Remove("vnp_SecureHash");
            }
            foreach (KeyValuePair<string, string> kv in _responseData)
            {
                if (!string.IsNullOrEmpty(kv.Value))
                {
                    data.Append(WebUtility.UrlEncode(kv.Key) + "=" + WebUtility.UrlEncode(kv.Value) + "&");
                }
            }
            //remove last '&'
            if (data.Length > 0)
            {
                data.Remove(data.Length - 1, 1);
            }
            return data.ToString();
        }

        #endregion

        #region MyRegion
        public Response<string> CreatePaymentUrl(VnPayPaymentRequest request)
        {
            Response<string> response = new Response<string>();
            //Get Config Info
            string vnp_Returnurl = Utilities.ConfigurationManager.AppSettings["VnPay:vnp_Returnurl"]; //URL nhan ket qua tra ve 
            string vnp_Url = Utilities.ConfigurationManager.AppSettings["VnPay:vnp_Url"]; //URL thanh toan cua VNPAY 
            string vnp_TmnCode = Utilities.ConfigurationManager.AppSettings["VnPay:vnp_TmnCode"]; //Ma định danh merchant kết nối (Terminal Id)
            string vnp_HashSecret = Utilities.ConfigurationManager.AppSettings["VnPay:vnp_HashSecret"]; //Secret Key

            addRequestData("vnp_Version", VERSION);
            addRequestData("vnp_Command", VnPayCommand.Payment);
            addRequestData("vnp_TmnCode", vnp_TmnCode);
            addRequestData("vnp_Amount", Convert.ToString(request.Amount * 100));

            if (request.BankCode == BankCode.VnPayQrCode)
            {
                addRequestData("vnp_BankCode", "VNPAYQR");
            }
            else if (request.BankCode == BankCode.VnPayVnBank)
            {
                addRequestData("vnp_BankCode", "VNBANK");
            }
            else if (request.BankCode == BankCode.VnPayIntCard)
            {
                addRequestData("vnp_BankCode", "INTCARD");
            }
            addRequestData("vnp_CreateDate", Utilities.DateTimeUtils.EpochToTimeString(request.CreatedDate, VnPayConst.DateTimeFormat));
            addRequestData("vnp_CurrCode", "VND");
            addRequestData("vnp_IpAddr", request.vnp_IpAddr);

            if (request.Locale == Locale.Vn)
            {
                addRequestData("vnp_Locale", "vn");
            }
            else if (request.Locale == Locale.En)
            {
                addRequestData("vnp_Locale", "en");
            }
            addRequestData("vnp_OrderInfo", request.vnp_OrderInfo);
            addRequestData("vnp_OrderType", "other"); //default value: other
            addRequestData("vnp_ReturnUrl", vnp_Returnurl);
            addRequestData("vnp_TxnRef", request.vnp_TxnRef);
            response.success = true;
            response.data = createRequestUrl(vnp_Url, vnp_HashSecret);
            return response;
        }

        public Response<string> CheckPaymentResponse(VnPayPaymentResponse response)
        {
            Response<string> res = new Response<string>();
            string vnp_HashSecret = Utilities.ConfigurationManager.AppSettings["VnPay:vnp_HashSecret"];
            bool checkSignature = validateSignature(response.vnp_SecureHash, vnp_HashSecret);
            if (checkSignature)
            {
                if (response.vnp_Amount == 10000)
                {
                    if (response.vnp_ResponseCode == "00" && response.vnp_TransactionStatus == "00")
                    {
                        //Thanh toan thanh cong
                        res.message = "Thanh toán thành công";
                        res.success = true;
                    }
                    else
                    {
                        //Thanh toan khong thanh cong. Ma loi: vnp_ResponseCode
                        res.message = "Thanh toán không thành công";
                    }
                }
                else
                {
                    ///Invalid amount
                    res.message = "Invalid amount";
                }

            }
            return res;
        }

        public async Task<Response> GetPayments(VnPayQueryRequest request)
        {
            Response response = new Response();
            string vnp_Api = Utilities.ConfigurationManager.AppSettings["VnPay:vnp_Api"];
            string vnp_HashSecret = Utilities.ConfigurationManager.AppSettings["VnPay:vnp_HashSecret"]; //Secret KEy
            string vnp_TmnCode = Utilities.ConfigurationManager.AppSettings["VnPay:vnp_TmnCode"]; // Terminal Id

            string vnp_RequestId = DateTime.Now.Ticks.ToString(); //Mã hệ thống merchant tự sinh ứng với mỗi yêu cầu truy vấn giao dịch. Mã này là duy nhất dùng để phân biệt các yêu cầu truy vấn giao dịch. Không được trùng lặp trong ngày.
            string vnp_TransactionDate = Utilities.DateTimeUtils.EpochToTimeString(request.vnp_TransactionDate, ""); ;
            string vnp_CreateDate = DateTime.Now.ToString(VnPayConst.DateTimeFormat);

            string signData = vnp_RequestId + "|" + VERSION + "|" + VnPayCommand.Query + "|" + vnp_TmnCode + "|" + request.vnp_TxnRef + "|" + vnp_TransactionDate + "|" + vnp_CreateDate + "|" + request.vnp_IpAddr + "|" + request.vnp_OrderInfo;
            string vnp_SecureHash = Utilities.CryptoUtils.HmacSHA512(vnp_HashSecret, signData);
            var qdrData = new
            {
                vnp_RequestId,
                vnp_Version = VERSION,
                vnp_Command = VnPayCommand.Query,
                vnp_TmnCode,
                request.vnp_TxnRef,
                request.vnp_OrderInfo,
                vnp_TransactionDate,
                vnp_CreateDate,
                request.vnp_IpAddr,
                vnp_SecureHash
            };
            string payload = JsonConvert.SerializeObject(qdrData);
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                var res = await client.PostAsync(vnp_Api, new StringContent(payload));
                try
                {
                    string strData = "";
                    res.EnsureSuccessStatusCode();
                    using (var streamReader = new StreamReader(await res.Content.ReadAsStreamAsync()))
                    {
                        strData = streamReader.ReadToEnd();
                    }
                }
                catch (Exception ex)
                {
                    response.message = "Lỗi truy vấn";
                }
            }
            return response;
        }
        #endregion

    }

}
