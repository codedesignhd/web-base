using CodeDesign.Dtos.Responses;
using CodeDesign.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeDesign.BL
{
    public class PaymentBL : BaseBL
    {

        private static PaymentBL _instance;

        public static PaymentBL Instance
        {
            get
            {
                if (_instance is null)
                {
                    _instance = new PaymentBL();
                }
                return _instance;
            }
        }

        public PaginatedResponse<PaymentHistory> GetPaymentHistoriesByUser(string user)
        {
            PaginatedResponse<PaymentHistory> response = new PaginatedResponse<PaymentHistory>();
            return response;
        }
    }
}
