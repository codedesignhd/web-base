﻿using CodeDesignES.Constants;
using CodeDesignModels;
using CodeDesignUtilities;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeDesignES
{
    public class PaymentHistoryRepository : ESRepositoryBase<PaymentHistory>
    {
        #region Init
        public PaymentHistoryRepository(string modify_index)
        {
            _index = !string.IsNullOrEmpty(modify_index) ? modify_index : _index;
            ConnectionSettings settings = new ConnectionSettings(connectionPool, sourceSerializer: Nest.JsonNetSerializer.JsonNetSerializer.Default)
                .DefaultIndex(_index)
                .DisableDirectStreaming(true)
                .MaximumRetries(10);
            client = new ElasticClient(settings);
            var ping = client.Ping(p => p.Pretty(true));
            if (ping.ServerError != null && ping.ServerError.Error != null)
            {
                throw new Exception("START ES FIRST");
            }
        }


        private static PaymentHistoryRepository _instance;
        public static PaymentHistoryRepository Instance
        {
            get
            {
                if (_instance is null)
                {
                    _instance = new PaymentHistoryRepository(string.Format("{0}payment_history", prefix_index));
                }
                return _instance;
            }
        }

        #endregion

        #region Features func
        public List<PaymentHistory> GetActivePaymentByUser(string username, Dictionary<string, SortDir> sort = null, string[] fields = null)
        {
            List<PaymentHistory> payments = new List<PaymentHistory>();
            List<QueryContainer> filter = new List<QueryContainer>()
            {
                new TermQuery{Field="payment_status", Value=PaymentStatus.DaThanhToan},
                new LongRangeQuery{Field="ngay_het_han", LessThan=DateTimeUtils.TimeInEpoch()},
            };
            if (username != null)
            {
                filter.Add(new TermQuery { Field = "payment_user.keyword", Value = username });
            }
            SearchRequest request = new SearchRequest(_index)
            {
                Query = new BoolQuery { Filter = filter, Must = CustomMustNot() },
                Size = ESConstants.MaxResultWindow,
                Sort = CustomSort(sort),
                Source = CustomSource(fields),
            };

            var res = client.Search<PaymentHistory>(request);
            if (res.IsValid)
            {
                payments = res.Hits
                    .Select(ToDocument)
                    .ToList();
            }
            return payments;
        }


        #endregion

        #region Service
        public List<PaymentHistory> GetAllPaymentExpired(string[] fields = null)
        {
            List<QueryContainer> filter = new List<QueryContainer>()
            {
                new LongRangeQuery{Field="ngay_het_han",LessThan=CodeDesignUtilities.DateTimeUtils.TimeInEpoch()},
                new TermQuery{Field="payment_status",Value=PaymentStatus.DaThanhToan},
            };

            QueryContainer query = new BoolQuery { Filter = filter, MustNot = CustomMustNot() };

            return GetObjectScroll(query, CustomSource(fields))
                .ToList();
        }
        #endregion
    }
}
