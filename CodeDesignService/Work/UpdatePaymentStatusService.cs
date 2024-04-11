using CodeDesign.ES;
using CodeDesign.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeDesign.Services.Work
{
    internal class UpdatePaymentStatusService
    {
        private readonly ILogger<UpdatePaymentStatusService> _logger;
        public UpdatePaymentStatusService(ILogger<UpdatePaymentStatusService> logger)
        {
            _logger = logger;
        }

        public void Run()
        {
            //Lấy tất cả những payment hết hạn nhưng vẫn đánh trạng thái đã thanh toán (kích hoạt)
            string[] fields = new string[] { "payment_user" };
            List<PaymentHistory> expiredPayments = PaymentHistoryRepository.Instance.GetAllPaymentExpired(fields);

            _logger.LogInformation("Tìm thấy {0} payments hết hạn", expiredPayments.Count);
            ConcurrentBag<object> docs = new ConcurrentBag<object>();
            long epoch = Utilities.DateTimeUtils.TimeInEpoch();
            Parallel.ForEach(expiredPayments, payment =>
            {
                docs.Add(new
                {
                    id = payment.id,
                    payment_status = PaymentStatus.HetHan,
                    ngay_sua = epoch,
                });
            });
            List<string> successIds = PaymentHistoryRepository.Instance.UpdateMany(docs.ToList());
            _logger.LogInformation("Update trạng thái thành công cho {0} payments hết hạn", successIds.Count);
        }
    }
}
