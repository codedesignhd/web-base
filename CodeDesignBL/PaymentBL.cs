using CodeDesign.Dtos.Responses;
using CodeDesign.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeDesign.BL
{
    public class PaymentBL : BaseBL
    {


        public PaginatedResponse<PaymentHistory> GetPaymentHistoriesByUser(string user)
        {
            PaginatedResponse<PaymentHistory> response = new PaginatedResponse<PaymentHistory>();
            return response;
        }
    }
}
