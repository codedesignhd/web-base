using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeDesignModels;
using Elasticsearch.Net;
using Nest;

namespace CodeDesignES
{
    public class EmailRepository : ESRepositoryBase<Email>
    {
        #region Init
        public EmailRepository(string modify_index)
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


        private static EmailRepository _instance;
        public static EmailRepository Instance
        {
            get
            {
                if (_instance is null)
                {
                    _instance = new EmailRepository(string.Format("{0}_email", prefix_index));
                }
                return _instance;
            }
        }

        #endregion

        #region Service
        public List<Email> GetMailResend(int max_error, string[] fields = null)
        {
            return new List<Email>();
        }
        #endregion
    }
}
