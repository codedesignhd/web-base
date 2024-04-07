using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeDesign.Models;
using Elasticsearch.Net;
using Nest;

namespace CodeDesign.ES
{
    public class EmailRepository : IESRepository
    {
        #region Init
        private static string _default_index;

        public EmailRepository(string modify_index)
        {
            _default_index = !string.IsNullOrEmpty(modify_index) ? modify_index : _default_index;
            ConnectionSettings settings = new ConnectionSettings(connectionPool, sourceSerializer: Nest.JsonNetSerializer.JsonNetSerializer.Default).DefaultIndex(_default_index).DisableDirectStreaming(true).MaximumRetries(10);
            client = new ElasticClient(settings);
            var ping = client.Ping(p => p.Pretty(true));
            if (ping.ServerError != null && ping.ServerError.Error != null)
            {
                throw new Exception("START ES FIRST");
            }
        }


        private static EmailRepository _instance;
        public static EmailRepository Instance
        {
            get
            {
                if (_instance is null)
                {
                    _default_index = string.Format("{0}_email", prefix_index);
                    _instance = new EmailRepository(_default_index);
                }
                return _instance;
            }
        }

        #endregion


        #region CRUD
        public (bool, string) Index(Email mail)
        {
            return Index<Email>(mail);
        }

        public bool Update(string id, object obj)
        {
            return Update<Email>(id, obj);
        }

        public bool Delete(string id)
        {
            return Delete<Email>(id);
        }

        public List<Email> GetAll(string[] fields = null)
        {
            SourceFilter so = new SourceFilter()
            {
                Includes = fields,
            };
            return GetObjectScroll<Email>(null, so).ToList();
        }

        public Email Get(string id, string[] fields = null)
        {
            return Get<Email>(id, fields);
        }

        public IEnumerable<Email> GetMailResend(int max_error, string[] fields = null)
        {
            List<QueryContainer> filter = new List<QueryContainer>()
            {
                new LongRangeQuery { Field = "so_lan_loi", LessThanOrEqualTo = max_error },
                new TermQuery { Field = "trang_thai_gui",Value=TrangThaiMail.LOI},
            };
            QueryContainer query = new QueryContainer(new BoolQuery { Filter = filter, MustNot = CustomMustNot() });
            return GetObjectScroll<Email>(query, CustomSource(fields));
        }
        #endregion
    }
}
